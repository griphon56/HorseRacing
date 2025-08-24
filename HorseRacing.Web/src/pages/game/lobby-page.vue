<template>
    <div class="lobby-page">
        <n-h1>Лобби игры</n-h1>
        <div v-if="game">
            <div class="game-title"><strong>Игра:</strong> {{ game.GameName }}</div>
        </div>
        <n-table :bordered="false" :single-line="false" class="players-table">
            <thead>
                <tr>
                    <th>Игрок</th>
                    <th>Ставка</th>
                    <th>Масть</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="player in players" :key="player.UserId">
                    <td>{{ player.FullName }}</td>
                    <td>{{ player.BetAmount }}</td>
                    <td>{{ suitName(player.BetSuit) }}</td>
                </tr>
            </tbody>
        </n-table>

        <div class="start-button-wrapper" v-if="players.length === MAX_PLAYERS">
            <n-button type="primary" @click="goToRace"> Начать гонку </n-button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { NH1, NTable } from 'naive-ui';
import { useRoute, useRouter } from 'vue-router';
import { useAuthStore } from '~/stores/auth-store';
import { useGamesStore } from '~/stores/games-store';
import { signalRService } from '~/core/signalr/signalr-service';
import { suitName } from '~/utils/game-utils';
import type { GameUserDto } from '~/interfaces/api/contracts/model/game/dto/game-user-dto';
import type { GetLobbyUsersWithBetsResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-lobby-users-with-bets/get-lobby-users-with-bets-response-dto';
import { RouteName } from '~/interfaces';

const MAX_PLAYERS = 4;

const route = useRoute();
const router = useRouter();
const gamesStore = useGamesStore();
const authStore = useAuthStore();
const game = ref<GetLobbyUsersWithBetsResponseDto | null>(null);
const players = ref<GameUserDto[]>([]);

async function loadLobby() {
    const gameId = route.params.id as string;

    const checkResponse = await gamesStore.checkPlayerConnectedToGame({
        Data: {
            GameId: gameId,
            UserId: authStore.user?.Id ?? '',
        },
    });

    if (!checkResponse?.Data?.IsConnected) {
        console.log('User is not connected to the game. Redirecting to join page...');
        router.push({ name: RouteName.JoinGame, params: { id: gameId } });
        return;
    }

    const usersResponse = await gamesStore.getLobbyUsersWithBets({ Data: { Id: gameId } });
    game.value = usersResponse?.Data || null;
    players.value = usersResponse?.Data?.Players || [];
}

function goToRace() {
    const gameId = route.params.id as string;
    router.push({ name: RouteName.Race, params: { id: gameId } });
}

onMounted(async () => {
    await signalRService.onLobbyPlayerListUpdated(async () => {
        await loadLobby();
    });

    const gameId = route.params.id as string;

    await signalRService.joinToGame(gameId);

    await loadLobby();
});

onBeforeUnmount(() => {
    signalRService.offLobbyPlayerListUpdated();
    signalRService.offEvent('GoToRaceEvent');
});
</script>

<style scoped>
.lobby-page {
    padding: 20px;
    max-width: 600px;
    margin: 0 auto;
}
.game-title {
    margin-bottom: 20px;
    font-size: 18px;
}
.players-table {
    margin-bottom: 30px;
}
.timer-block {
    margin-top: 30px;
    text-align: center;
}
</style>
