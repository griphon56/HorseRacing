<template>
   <div class="games-page">
    <n-h1>Игры</n-h1>
    <n-button type="primary" @click="goToCreateRoom" class="mb-4">Создать игру</n-button>
    <GameList :games="games" @selectGame="onSelectGame" />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { NH1, NButton } from 'naive-ui';
import { useGamesStore } from '~/stores/games-store';
import GameList from '~/core/games/game-list.vue';
import { useRouter } from 'vue-router';
import { RouteName } from '~/interfaces';

const router = useRouter();
const gamesStore = useGamesStore();
const games = ref<any[]>([]);

async function loadGames() {
  const response = await gamesStore.getWaitingGames();
  games.value = response?.Data?.Games || [];
}

function onSelectGame(game: any) {
  // Перенаправление на страницу join-game с передачей id игры
  router.push({ name: RouteName.JoinGame, params: { id: game.GameId } });
}

function goToCreateRoom() {
  router.push({ name: RouteName.Games }); // Имя маршрута для страницы создания игры
}

loadGames();
</script>

<style scoped>
.games-page {
  padding: 20px;
}
.mt-4 {
  margin-top: 20px;
}
.mb-4 {
  margin-bottom: 20px;
}
.game-list-item {
  cursor: pointer;
  padding: 10px;
  border-bottom: 1px solid #eee;
  transition: background 0.2s;
}
.game-list-item:hover {
  background: #f5f5f5;
}
.game-details {
  margin-top: 20px;
}
</style>
