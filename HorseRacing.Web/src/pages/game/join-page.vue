<template>
    <div class="join-game-page">
        <n-h1>Присоединиться к игре</n-h1>

        <div v-if="gameInfo" class="game-header">
            <strong>Комната:</strong> {{ gameInfo.Name }}
            <span v-if="gameInfo.Mode">({{ gameModeName(gameInfo.Mode) }})</span>
        </div>

        <n-form ref="formRef" :model="form" :rules="rules" label-placement="top">
            <n-form-item label="Ставка" path="betAmount">
                <n-input-number
                    v-model:value="form.betAmount"
                    :min="1"
                    placeholder="Введите ставку"
                    :disabled="isBetDisabled"
                />
            </n-form-item>
            <n-form-item label="Масть" path="betSuit">
                <n-select
                    v-model:value="form.betSuit"
                    :options="suitOptions"
                    placeholder="Выберите масть"
                />
            </n-form-item>
        </n-form>
        <n-space class="mt-4">
            <n-button @click="goBack">Назад</n-button>
            <n-button type="primary" :loading="loading" @click="onJoinGame"
                >Присоединиться</n-button
            >
        </n-space>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, reactive } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import {
    NForm,
    NFormItem,
    NInputNumber,
    NSelect,
    NButton,
    NH1,
    NSpace,
    type FormInst,
    type FormRules,
} from 'naive-ui';
import { useGamesStore } from '~/stores/games-store';
import { useAuthStore } from '~/stores/auth-store';
import { RouteName } from '~/interfaces';
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';
import { signalRService } from '~/core/signalr/signalr-service';
import { SuitOptions, gameModeName } from '~/utils/game-utils';
import type { GetGameResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-game/get-game-response-dto';
import { GameModeType } from '~/interfaces/api/contracts/model/game/enums/game-mode-type-enum';

const suitOptions = ref([...SuitOptions]);

const gamesStore = useGamesStore();
const authStore = useAuthStore();
const router = useRouter();
const route = useRoute();

const loading = ref(false);
const formRef = ref<FormInst | null>(null);
const form = reactive({
    gameId: '',
    betAmount: 1,
    betSuit: SuitType.Diamonds,
});

const rules: FormRules = {
    betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
    betSuit: { required: true, type: 'number', message: 'Выберите масть', trigger: ['change'] },
};

const gameInfo = ref<GetGameResponseDto | null>(null);
const isBetDisabled = ref(false);

/**
 * Получает информацию по игре. Адаптируется к разным форматам ответа (Data / DataValues / plain).
 */
async function fetchGameInfo(gameId: string) {
    try {
        const resp = await gamesStore.getGameById({ Data: { Id: gameId } });
        if (!resp || !resp.Data) return;

        gameInfo.value = resp.Data;

        if (gameInfo.value.Mode == GameModeType.Classic) {
            form.betAmount = gameInfo.value.DefaultBet ?? 1;
            isBetDisabled.value = true;
        } else {
            isBetDisabled.value = false;
        }
    } catch (err) {
        console.warn('fetchGameInfo error', err);
    }
}

async function fetchAvailableSuits(gameId: string) {
    const response = await gamesStore.getAvailableSuit({ Data: { Id: gameId } });
    const availableSuits = response?.DataValues || [];

    const availableSuitKeys = availableSuits.map((suit: any) => suit.Suit);

    suitOptions.value = suitOptions.value.filter(opt =>
        availableSuitKeys.includes(SuitType[opt.value])
    );

    if (suitOptions.value.length > 0) {
        form.betSuit = suitOptions.value[0].value;
    }
}

function goBack() {
    router.push({ name: RouteName.Games });
}

async function onJoinGame() {
    if (!formRef.value) return;
    loading.value = true;
    try {
        await formRef.value.validate();

        await gamesStore.joinGameWithBet({
            Data: {
                GameId: form.gameId,
                UserId: authStore.user?.Id || '',
                BetAmount: form.betAmount,
                BetSuit: form.betSuit as SuitType,
            },
        });

        router.push({ name: RouteName.Lobby, params: { id: form.gameId } });
    } finally {
        loading.value = false;
    }
}

onMounted(async () => {
    if (route.params.id) {
        form.gameId = String(route.params.id);
        await fetchGameInfo(form.gameId);

        await fetchAvailableSuits(form.gameId);

        await signalRService.joinToGame(form.gameId);

        await signalRService.onAvailableSuitsUpdated(async () => {
            await fetchAvailableSuits(route.params.id as string);
        });
    }
});

onBeforeUnmount(() => {
    signalRService.offAvailableSuitsUpdated();
});
</script>

<style scoped>
.join-game-page {
    padding: 20px;
    max-width: 400px;
    margin: 0 auto;
}
.mt-4 {
    margin-top: 20px;
}
</style>
