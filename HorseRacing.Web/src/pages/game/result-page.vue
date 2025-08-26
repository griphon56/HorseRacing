<template>
    <div class="result-page">
        <n-h1>Результаты гонки</n-h1>

        <n-table :bordered="false" :single-line="false" class="results-table">
            <thead>
                <tr>
                    <th>🏅</th>
                    <th>Игрок</th>
                    <th>Масть</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="player in sortedPlayers" :key="player.UserId">
                    <td class="pos-cell">
                        <template v-if="player.Place === 1">🥇</template>
                        <template v-else-if="player.Place === 2">🥈</template>
                        <template v-else-if="player.Place === 3">🥉</template>
                        <template v-else>{{ player.Place }}</template>
                    </td>
                    <td class="name-cell">{{ player.FullName ?? '—' }}</td>
                    <td class="suit-cell">
                        {{ suitName(player.BetSuit) }} {{ suitIcon(player.BetSuit) }}
                    </td>
                </tr>
            </tbody>
        </n-table>

        <n-space class="mt-4">
            <n-button type="primary" @click="goToGames">Выход</n-button>
        </n-space>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { NButton, NSpace, NH1, NTable } from 'naive-ui';
import { useGamesStore } from '~/stores/games-store';
import { suitIcon, suitName } from '~/utils/game-utils';
import { RouteName } from '~/interfaces/app/routes';
import type { GetGameResultResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-game-result/get-game-result-response-dto';
import type { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';

const router = useRouter();
const route = useRoute();
const gamesStore = useGamesStore();

const players = ref<GetGameResultResponseDto[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);

const sortedPlayers = computed(() => {
    return [...players.value].sort((a, b) => {
        const aPlace = typeof a.Place === 'number' ? a.Place : Number.POSITIVE_INFINITY;
        const bPlace = typeof b.Place === 'number' ? b.Place : Number.POSITIVE_INFINITY;
        return aPlace - bPlace;
    });
});

function goToGames() {
    router.push({ name: RouteName.Games });
}

onMounted(async () => {
    loading.value = true;
    error.value = null;
    try {
        const gameId = String(route.params.id ?? '');
        if (!gameId) {
            error.value = 'Не указан id игры';
            return;
        }

        const response = await gamesStore.getGameResult({ Data: { Id: gameId } });
        const dataArray: GetGameResultResponseDto[] = response?.DataValues ?? [];

        players.value = Array.isArray(dataArray) ? dataArray : [];
    } catch (e: any) {
        console.error('getGameResult failed', e);
        error.value = e?.message ?? 'Ошибка загрузки результатов';
    } finally {
        loading.value = false;
    }
});
</script>

<style scoped>
.result-page {
    padding: 20px;
    max-width: 720px;
    margin: 0 auto;
}

.results-table {
    margin-bottom: 30px;
    width: 100%;
}

.mt-4 {
    margin-top: 20px;
    text-align: center;
}

.pos-cell {
    width: 64px;
    text-align: center;
    font-weight: 600;
}

.name-cell {
    min-width: 180px;
}

.suit-cell {
    width: 120px;
    text-align: center;
}
</style>
