<template>
  <div class="main-layout">
    <!-- <TheHeader /> -->
    <div class="header">
      <RouterLink :to="{ name: 'account' }">Аккаунт</RouterLink>
      <RouterLink :to="{ name: 'aaa' }">Список игр</RouterLink>
    </div>
    <RouterView />
  </div>
</template>

<script setup lang="ts">
/**
 * Главный layout приложения. Предназначен для использования
 * в router в качестве родительского компонента, в котором
 * будут отображаться страницы.
 */
import { RouterView, RouterLink } from 'vue-router'
//   import { TheHeader } from './header';
</script>

<style scoped>
.main-layout {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.header {
  display: flex;
  flex-direction: row;
  gap: 4px;
}
</style>
