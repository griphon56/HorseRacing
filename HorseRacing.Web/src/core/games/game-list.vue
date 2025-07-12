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
import { ref, onMounted } from 'vue';
import { useGamesStore } from '~/stores/games-store';
import { NList, NListItem } from 'naive-ui';

const gamesStore = useGamesStore();
const games = ref<any[]>([]);

onMounted(async () => {
  const response = await gamesStore.getWaitingGames();
  // Ожидается, что response.Data.Games — массив игр
  games.value = response?.Data?.Games || [];
});
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
