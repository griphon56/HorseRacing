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
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { NH1, NTable } from 'naive-ui'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '~/stores/auth-store';
import { useGamesStore } from '~/stores/games-store'
import { signalRService } from '~/core/signalr/signalr-service'
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum'
import type { GameUserDto } from '~/interfaces/api/contracts/model/game/dto/game-user-dto'
import type { GetLobbyUsersWithBetsResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-lobby-users-with-bets/get-lobby-users-with-bets-response-dto'
import { RouteName } from '~/interfaces'

const route = useRoute()
const router = useRouter()
const gamesStore = useGamesStore()
const authStore = useAuthStore()
const game = ref<GetLobbyUsersWithBetsResponseDto | null>(null)
const players = ref<GameUserDto[]>([])

function suitName(suit: number | string) {
    switch (suit) {
        case SuitType.Diamonds:
        case 'Diamonds':
            return 'Бубны'
        case SuitType.Hearts:
        case 'Hearts':
            return 'Черви'
        case SuitType.Spades:
        case 'Spades':
            return 'Пики'
        case SuitType.Clubs:
        case 'Clubs':
            return 'Трефы'
        default:
            return suit
    }
}

async function loadLobby() {
    const gameId = route.params.id as string

    const checkResponse = await gamesStore.checkPlayerConnectedToGame({
      Data: {
        GameId: gameId,
        UserId: authStore.user?.Id ?? '',
      },
    });

    if (!checkResponse?.Data?.IsConnected) {
        console.log('User is not connected to the game. Redirecting to join page...')
        router.push({ name: RouteName.JoinGame, params: { id: gameId } })
        return
    }

    const usersResponse = await gamesStore.getLobbyUsersWithBets({ Data: { Id: gameId } })
    game.value = usersResponse?.Data || null
    players.value = usersResponse?.Data?.Players || []

    // Убедитесь, что соединение с SignalR активно
    if (signalRService.getConnectionState('commonHub') !== 'Connected') {
        await signalRService.connectToHub('commonHub');
    }

    // Подписка на событие StartGame
    signalRService.onStartGame('commonHub', () => {
        console.log('Game started, redirecting to race page...')
        router.push({ name: RouteName.Race, params: { id: gameId } })
    })
}

onMounted(async () => {
    try {
        await loadLobby();
    } catch (err) {
        console.error('Error during lobby initialization:', err);
    }
})
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
