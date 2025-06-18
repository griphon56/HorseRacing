<template>
  <div class="main-layout">
    <!-- Основной контент -->
    <div class="main-content">
      <RouterView />
    </div>

    <!-- Нижнее меню -->
   <main-menu />
  </div>
</template>

<script setup lang="ts">
import { RouterView } from 'vue-router'
import MainMenu from './main-menu.vue'
</script>

<style scoped>
.main-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.main-content {
  flex: 1;
  overflow-y: auto;
  padding-bottom: 56px; /* чтобы контент не перекрывался меню */
}

</style>
