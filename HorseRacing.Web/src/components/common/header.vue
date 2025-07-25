<template>
  <div class="header">
    <div class="header-content">
      <span class="player-name">{{ playerName }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth-store';

const authStore = useAuthStore();
const playerName = authStore.userFormattedName || 'Гость';
</script>

<style scoped>
.header {
  background-color: #f5f5f5;
  padding: 10px 20px;
  border-bottom: 1px solid #ddd;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.player-name {
  font-weight: bold;
  font-size: 16px;
}
</style>
