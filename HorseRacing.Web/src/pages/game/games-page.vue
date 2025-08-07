<template>
   <div class="games-page">
    <n-h1>Игры</n-h1>
    <n-button type="primary" @click="goToCreateRoom" class="mb-4">Создать игру</n-button>
    <n-list>
      <n-list-item
          v-for="game in games"
          :key="game.GameId"
          @click="onSelectGame(game)"
          class="game-list-item"
      >
          <div>
              <strong>{{ game.Name }}</strong>
              <span v-if="game.Status !== undefined"> — Статус: {{ game.Status }}</span>
          </div>
      </n-list-item>
    </n-list>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { useRouter } from 'vue-router';
import { NH1, NButton, NList, NListItem } from 'naive-ui';

import { useAuthStore } from '~/stores/auth-store';
import { useGamesStore } from '~/stores/games-store';
import { RouteName } from '~/interfaces';
import { signalRService } from '~/core/signalr/signalr-service';

import type { GameDto } from '~/interfaces/api/contracts/model/game/dto/game-dto';

const router = useRouter();
const gamesStore = useGamesStore();
const authStore = useAuthStore();
const games = ref<GameDto[]>([]);

const updateGameList = async () => {
    const response = await gamesStore.getWaitingGames();
    games.value = response?.Data?.Games || [];
    console.log('Game list updated:', games.value);
};

const onSelectGame = async (game: GameDto) => {
  try {
    const checkResponse = await gamesStore.checkPlayerConnectedToGame({
      Data: {
        GameId: game.GameId,
        UserId: authStore.user?.Id ?? '',
      },
    });

    if (checkResponse?.Data?.IsConnected) {
      console.log('Player is already connected to the game. Redirecting to lobby...');
      router.push({ name: RouteName.Lobby, params: { id: game.GameId } });
    } else {
      console.log('Player joined the game. Redirecting to lobby...');
      router.push({ name: RouteName.JoinGame, params: { id: game.GameId } });
    }
  } catch (err) {
    console.error('Error joining game:', err);
  }
};

const goToCreateRoom = () => {
  router.push({ name: RouteName.CreateRoom });
};

onMounted(async () => {
    await signalRService.subscribeGameListUpdates();
    await signalRService.onGameListUpdated(updateGameList);
    await updateGameList();
});

onBeforeUnmount(async () => {
  await signalRService.offGameListUpdated();
});
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
</style>
