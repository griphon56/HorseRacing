<template>
  <n-list>
    <n-list-item v-for="game in games" :key="game.id" @click="$emit('selectGame', game)" class="game-list-item">
      <div>
        <strong>{{ game.name }}</strong> — Ставка: {{ game.betAmount }}
      </div>
    </n-list-item>
  </n-list>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useGamesStore, type Game } from '~/stores/games-store';
import { NList, NListItem } from 'naive-ui';

const gamesStore = useGamesStore();
const games = computed<Game[]>(() => gamesStore.games);
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
