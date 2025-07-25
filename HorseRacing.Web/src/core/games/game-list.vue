<template>
    <n-list>
        <n-list-item
            v-for="game in games"
            :key="game.GameId"
            @click="$emit('selectGame', game)"
            class="game-list-item"
        >
            <div>
                <strong>{{ game.Name }}</strong>
                <span v-if="game.Status !== undefined"> — Статус: {{ game.Status }}</span>
                <span v-if="game.BetAmount !== undefined"> — Ставка: {{ game.BetAmount }}</span>
            </div>
        </n-list-item>
    </n-list>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useGamesStore } from '~/stores/games-store'
import { NList, NListItem } from 'naive-ui'
import { signalRService } from '~/core/signalr/signalr-service'

const gamesStore = useGamesStore()
const games = ref<{ GameId: string; Name: string; Status?: string; BetAmount?: number }[]>([])

const updateGameList = async () => {
    const response = await gamesStore.getWaitingGames()
    // Ожидается, что response.Data.Games — массив игр
    games.value = response?.Data?.Games || []
    console.log('Game list updated:', games.value)
}

onMounted(async () => {
    const hubName = 'commonHub'
    try {
        await signalRService.connectToHub(hubName)
        await signalRService.subscribeToLobby(hubName)
        signalRService.onUpdateListLobby(hubName, updateGameList)
    } catch (err) {
        console.error('Error setting up SignalR for game list updates:', err)
    }

    await updateGameList()
})

onUnmounted(async () => {
    const hubName = 'commonHub'
    try {
        await signalRService.unsubscribeFromLobby(hubName)
        signalRService.disconnect(hubName)
    } catch (err) {
        console.error('Error cleaning up SignalR for game list updates:', err)
    }
})
</script>

<style scoped>
.game-list-item {
    cursor: pointer;
    padding: 10px;
    border-bottom: 1px solid #eee;
    transition: background 0.2s;
}
.game-list-item:hover {
    background: #f5f5f5;
}
</style>
