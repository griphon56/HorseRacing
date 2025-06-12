<template>
    <div class="main-layout">
      <div class="header">
        <RouterLink :to="{ name: RouteName.Account }">Аккаунт</RouterLink>
        <RouterLink :to="{ name: RouteName.Games }">Список игр</RouterLink>
        <RouterLink :to="{ name: RouteName.Profile }">Профиль</RouterLink>
        <RouterLink :to="{ name: RouteName.Lobby }">Лобби</RouterLink>
      </div>
      <RouterView />
    </div>
  </template>

  <script setup lang="ts">
  import { RouterView, RouterLink } from 'vue-router'
  import { RouteName } from '~/interfaces'

  /**
   * Главный layout приложения. Рендерит шапку с навигацией
   * и <RouterView> для вложенных страниц.
   */
  </script>

  <style scoped>
  .main-layout {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
  }

  .header {
    display: flex;
    gap: 1rem;
    padding: 1rem;
    background-color: #f5f5f5;
  }
  </style>
