<template>
  <div class="race-page">
    <n-h1>Гонка</n-h1>
    <div v-if="!connected">Подключение к серверу...</div>
    <div v-else-if="waiting">Ожидание других игроков...</div>
    <div v-else>Гонка началась!</div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { NH1 } from 'naive-ui';
import { useRoute, useRouter } from 'vue-router';
import { useAuthStore } from '~/stores/auth-store';
import * as signalR from '@microsoft/signalr';

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();

const connected = ref(false);
const waiting = ref(true);
let connection: signalR.HubConnection | null = null;

async function connectToHub() {
  const token = authStore.tokens?.AccessToken;
  const userId = authStore.user?.Id;
  const gameId = route.params.id as string;
  if (!token || !userId || !gameId) return;

  connection = new signalR.HubConnectionBuilder()
    .withUrl(`/hubs/race?gameId=${gameId}&userId=${userId}`, {
      accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build();

  connection.on('StartGame', () => {
    waiting.value = false;
    // Перенаправить на страницу гонки (если вы хотите отдельную страницу, иначе показать гонку)
    // router.push({ name: 'Race', params: { id: gameId } });
  });

  connection.on('AllPlayersConnected', () => {
    // Можно отправить уведомление или обновить UI
    // Например, показать что все готовы
  });

  connection.onclose(() => {
    connected.value = false;
  });

  try {
    await connection.start();
    connected.value = true;
  } catch (err) {
    connected.value = false;
  }
}

onMounted(connectToHub);
onUnmounted(() => {
  if (connection) {
    connection.stop();
  }
});
</script>

<style scoped>
.race-page {
  padding: 20px;
  max-width: 600px;
  margin: 0 auto;
}
</style>
