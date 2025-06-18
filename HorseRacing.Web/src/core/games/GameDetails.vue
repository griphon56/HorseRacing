<template>
  <n-card v-if="game" title="Детали игры" class="game-details">
    <div><strong>Название:</strong> {{ game.name }}</div>
    <div><strong>Ставка:</strong> {{ game.betAmount }}</div>
    <div><strong>Масть:</strong> {{ game.betSuit }}</div>
    <n-space class="mt-4">
      <n-button type="primary" @click="joinGame">Присоединиться</n-button>
    </n-space>
  </n-card>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';
import { useGamesStore } from '~/stores/games-store';
import { NCard, NButton, NSpace } from 'naive-ui';

const props = defineProps({
  game: Object
});

const gamesStore = useGamesStore();

function joinGame() {
  if (props.game) {
    gamesStore.joinGameWithBet({
      gameId: props.game.id,
      userId: '', // подставьте актуальный userId
      betAmount: props.game.betAmount,
      betSuit: props.game.betSuit
    });
  }
}
</script>

<style scoped>
.game-details {
  margin-top: 20px;
}
</style>
