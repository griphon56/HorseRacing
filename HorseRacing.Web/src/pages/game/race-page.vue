<template>
  <div class="race-page">
    <n-h1>Гонка</n-h1>
  </div>
</template>

<script setup lang="ts">
/**
 * Race page — переработанная верстка под набросок
 * - слева: барьеры вертикально
 * - центр: дорожка, лошади снизу-верх (horsesLayer)
 * - справа: колода и отбой
 *
 * Предполагается, что initialDeck, horseBets, events и т.д.
 * наполняются извне (signalR, play result). Здесь — отображение и плейбек.
 */

import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum'
import { ZoneType } from '~/interfaces/api/contracts/model/game/enums/zone-type-enum'
import type { GameDeckCardDto } from '~/interfaces/api/contracts/model/game/dto/game-deck-card-dto'
import type { HorseBetDto } from '~/interfaces/api/contracts/model/game/dto/horse-bet-dto'
import type { GameEventDto } from '~/interfaces/api/contracts/model/game/dto/game-event-dto'
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response'
import { GameEventType } from '~/interfaces/api/contracts/model/game/enums/game-event-type-enum'
import { signalRService } from '~/core/signalr/signalr-service'
import { useRoute } from 'vue-router'

/* ---------- state (как раньше) ---------- */
const route = useRoute()
const gameIdParam = String(route.params.id ?? '')

const initialDeck = ref<GameDeckCardDto[]>([])
const horseBets = ref<HorseBetDto[]>([])
const events = ref<GameEventDto[]>([])
const discardCard = ref<GameDeckCardDto | null>(null)
const horsePositions = ref<Record<number, number>>({})


onMounted(async () => {
    try{
        const result = signalRService.onGameSimulationResult();
        await signalRService.registerReadyToStart(gameIdParam);

        const playResult = await result
        console.log('Simulation result received', playResult)
    }
    catch (error) {
        console.error('Ошибка при ожидании результата симуляции:', error);
    }

})

onBeforeUnmount(() => {
    signalRService.offGameSimulationResult();
});
</script>
