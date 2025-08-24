<template>
    <div class="result-page">
        <n-h1>Результаты гонки</n-h1>

        <n-table :bordered="false" :single-line="false" class="results-table">
            <thead>
                <tr>
                    <th>🏅</th>
                    <th>Игрок</th>
                    <th>Масть</th>
                    <th>Победитель</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="player in sortedPlayers" :key="player.UserId">
                    <td class="pos-cell">
                        <template v-if="player.Position === 1">🥇</template>
                        <template v-else-if="player.Position === 2">🥈</template>
                        <template v-else-if="player.Position === 3">🥉</template>
                        <template v-else>{{ player.Position }}</template>
                    </td>
                    <td class="name-cell">{{ player.FullName ?? '—' }}</td>
                    <td class="suit-cell">{{ suitName(player.BetSuit) }}</td>
                    <td class="winner-cell">
                        <span v-if="player.IsWinner">✅</span>
                        <span v-else>—</span>
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
import { suitName } from '~/utils/game-utils';
import { RouteName } from '~/interfaces/app/routes';
import type { GetGameResultResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-game-result/get-game-result-response-dto';

const router = useRouter();
const route = useRoute();
const gamesStore = useGamesStore();

const players = ref<GetGameResultResponseDto[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);

/**
 * Сортировка: по Position (1..n). Если Position отсутствует — ставим в конец.
 */
const sortedPlayers = computed(() => {
    return [...players.value].sort((a, b) => {
        const aPos = typeof a.Position === 'number' ? a.Position : Number.POSITIVE_INFINITY;
        const bPos = typeof b.Position === 'number' ? b.Position : Number.POSITIVE_INFINITY;
        return aPos - bPos;
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

        // Вызов store — адаптируй аргументы под твой store если нужно.
        // Ожидаем, что store.getGameResult возвращает { DataValues: GetGameResultResponseDto[] } или схожую структуру.
        const response = await gamesStore.getGameResult({ Data: { Id: gameId } });

        const dataArray: GetGameResultResponseDto[] = response?.DataValues ?? [];

        // Нормализуем типы и защитимся от undefined
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

.winner-cell {
    width: 80px;
    text-align: center;
}
</style>
