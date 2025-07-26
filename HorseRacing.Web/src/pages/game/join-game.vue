<template>
    <div class="join-game-page">
        <n-h1>Присоединиться к игре</n-h1>
        <n-form ref="formRef" :model="form" :rules="rules" label-placement="top">
            <n-form-item label="Ставка" path="betAmount">
                <n-input-number
                    v-model:value="form.betAmount"
                    :min="1"
                    placeholder="Введите ставку"
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
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
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
} from 'naive-ui'
import { useGamesStore } from '~/stores/games-store'
import { useAuthStore } from '~/stores/auth-store'
import { RouteName } from '~/interfaces'
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum'
import { signalRService } from '~/core/signalr/signalr-service'

const suitOptions = ref([
    { label: 'Бубны', value: SuitType.Diamonds },
    { label: 'Черви', value: SuitType.Hearts },
    { label: 'Пики', value: SuitType.Spades },
    { label: 'Трефы', value: SuitType.Clubs },
])

const gamesStore = useGamesStore()
const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()

const loading = ref(false)
const formRef = ref<FormInst | null>(null)
const form = ref({
    gameId: '',
    betAmount: 1,
    betSuit: SuitType.Diamonds,
})

const rules: FormRules = {
    betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
    betSuit: { required: true, type: 'number', message: 'Выберите масть', trigger: ['change'] },
}

onMounted(async () => {
    if (route.params.id) {
        form.value.gameId = String(route.params.id)
        await fetchAvailableSuits(form.value.gameId)
        await signalRService.connectToHub('commonHub')

        await signalRService.onEvent('commonHub', 'UpdateAvailableSuits', async () => {
            console.log('Получено обновление списка доступных мастей');
            await fetchAvailableSuits(route.params.id as string); // повторно запрашиваем масти
        });
    }
})
onUnmounted(() => {
  signalRService.offEvent('commonHub', 'UpdateAvailableSuits');
});

async function fetchAvailableSuits(gameId: string) {
  const response = await gamesStore.getAvailableSuit({ Data: { Id: gameId } });
  const availableSuits = response?.DataValues || [];

  const availableSuitKeys = availableSuits.map((suit: any) => suit.Suit);

  suitOptions.value = suitOptions.value.filter(opt =>
    availableSuitKeys.includes(SuitType[opt.value])
  );

    if (suitOptions.value.length > 0) {
        form.value.betSuit = suitOptions.value[0].value
    }
}

function goBack() {
    router.push({ name: RouteName.Games })
}

async function onJoinGame() {
    if (!formRef.value) return
    loading.value = true
    try {
        await formRef.value.validate()

        await signalRService.joinToGame('commonHub', form.value.gameId)
        console.log('Joined game! Redirecting to lobby...')

        await gamesStore.joinGameWithBet({
            Data: {
                GameId: form.value.gameId,
                UserId: authStore.user?.Id || '',
                BetAmount: form.value.betAmount,
                BetSuit: form.value.betSuit as SuitType,
            },
        })

        router.push({ name: RouteName.Lobby })
    } finally {
        loading.value = false
    }
}
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
