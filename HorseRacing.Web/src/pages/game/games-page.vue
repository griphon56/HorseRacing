<template>
   <div class="games-page">
    <n-h1>Игры</n-h1>
    <n-button type="primary" @click="goToCreateRoom" class="mb-4">Создать игру</n-button>
    <GameList :games="games" @selectGame="onSelectGame" />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { NH1, NButton } from 'naive-ui';

import { useAuthStore } from '~/stores/auth-store';
import { useGamesStore } from '~/stores/games-store';
import GameList from '~/core/games/game-list.vue';
import { RouteName } from '~/interfaces';

import type { GameDto } from '~/interfaces/api/contracts/model/game/dto/game-dto';

const router = useRouter();
const gamesStore = useGamesStore();
const authStore = useAuthStore();
const games = ref<GameDto[]>([]);

async function loadGames() {
  const response = await gamesStore.getWaitingGames();
  games.value = response?.Data?.Games || [];
}

async function onSelectGame(game: GameDto) {
  try {
    const checkResponse = await gamesStore.checkPlayerConnectedToGame({
         Data: {
            GameId: game.GameId,
            UserId: authStore.user?.Id ?? ''
        }});

    if (checkResponse?.Data?.IsConnected) {
      router.push({ name: RouteName.Lobby, params: { id: game.GameId } });
    } else {
      router.push({ name: RouteName.JoinGame, params: { id: game.GameId } });
    }
  } catch {
    router.push({ name: RouteName.JoinGame, params: { id: game.GameId } });
  }
}

function goToCreateRoom() {
  router.push({ name: RouteName.CreateRoom });
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
