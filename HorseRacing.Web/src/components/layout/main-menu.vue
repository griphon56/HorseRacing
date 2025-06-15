<template>
  <n-layout-header class="main-menu" bordered>
    <n-space justify="space-between" align="center" class="w-full px-4">
      <div class="logo" @click="goHome">🐎 Horse Racing</div>
      <n-menu
        mode="horizontal"
        :options="menuOptions"
        :value="selectedKey"
        @update:value="handleSelect"
      />
      <div>
        <n-button quaternary size="small" @click="logout">
          Выйти
        </n-button>
      </div>
    </n-space>
  </n-layout-header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { NLayoutHeader, NMenu, NButton, NSpace } from 'naive-ui'
import { useAuthStore } from '~/stores/auth-store'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const selectedKey = computed(() => route.name as string)

const menuOptions = [
  { label: 'Игры', key: 'Games' },
  { label: 'Создать', key: 'Lobby' },
  { label: 'Аккаунт', key: 'Account' },
  { label: 'Профиль', key: 'Profile' }
]

function handleSelect(key: string) {
  router.push({ name: key })
}

function goHome() {
  router.push({ name: 'Games' })
}

function logout() {
  authStore.logout()
  router.push({ name: 'Auth' })
}
</script>

<style scoped>
.main-menu {
  background-color: #fff;
  height: 60px;
  display: flex;
  align-items: center;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.logo {
  font-weight: bold;
  font-size: 18px;
  cursor: pointer;
}
</style>
