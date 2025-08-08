<template>
  <div class="race-page">
    <n-h1>Гонка</n-h1>

  </div>
</template>

<script setup lang="ts">
import { onMounted, onBeforeUnmount } from 'vue';
import { NH1 } from 'naive-ui';
import { useRoute, useRouter } from 'vue-router';
import { signalRService } from '~/core/signalr/signalr-service';
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response';

const route = useRoute()

onMounted(async () => {
    const gameId = route.params.id as string

    await signalRService.registerReadyToStart(gameId);

    signalRService.onGameSimulationResult((data: PlayGameResponse) => {
        console.log('Game simulation result received:', data);
        // Handle the game simulation result here, e.g., update the UI or store state
    });
})

onBeforeUnmount(() => {
  signalRService.offGameSimulationResult();
});
</script>

<style scoped>
.race-page {
  padding: 20px;
  max-width: 600px;
  margin: 0 auto;
}
</style>
