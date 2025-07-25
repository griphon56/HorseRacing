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
        <div v-if="showTimer" class="timer-block">
            <n-h2>До старта игры: {{ timer }} сек.</n-h2>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { NH1, NH2, NTable } from 'naive-ui'
import { useRoute, useRouter } from 'vue-router'
import { useGamesStore } from '~/stores/games-store'
import { signalRService } from '~/core/signalr/signalr-service'
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum'
import type { GameUserDto } from '~/interfaces/api/contracts/model/game/dto/game-user-dto'
import type { GetLobbyUsersWithBetsResponseDto } from '~/interfaces/api/contracts/model/game/responses/get-lobby-users-with-bets/get-lobby-users-with-bets-response-dto'

const route = useRoute()
const router = useRouter()
const gamesStore = useGamesStore()
const game = ref<GetLobbyUsersWithBetsResponseDto | null>(null)
const players = ref<GameUserDto[]>([])
const showTimer = ref(false)
const timer = ref(10) // 10 секунд до старта
let timerInterval: ReturnType<typeof setInterval> | null = null

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
    const usersResponse = await gamesStore.getLobbyUsersWithBets({ Data: { Id: gameId } })
    game.value = usersResponse?.Data || null
    players.value = usersResponse?.Data?.Players || []

    const allSuitsTaken = players.value.length > 0 && players.value.every((p) => p.BetSuit)
    if (allSuitsTaken) {
        startTimer()
    }

    // Подписка на событие StartGame
    signalRService.onStartGame('commonHub', () => {
        console.log('Game started, redirecting to game page...')
        router.push(`/game/${gameId}`)
    })
}

function startTimer() {
    showTimer.value = true
    timer.value = 10
    if (timerInterval) clearInterval(timerInterval)
    timerInterval = setInterval(() => {
        timer.value--
        if (timer.value <= 0) {
            clearInterval(timerInterval!)
            // Здесь можно вызвать старт игры
            showTimer.value = false
        }
    }, 1000)
}

onMounted(async () => {
    const gameId = route.params.id as string
    await signalRService.connectToHub('commonHub')
    await signalRService.joinToGame('commonHub', gameId)
    await loadLobby()
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
