<template>
  <div>
    <button @click="goToGames">go to games</button>

    {{ counter }}
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { counter } from '~/stores/counter'

const router = useRouter()

const goToGames = () => {
  router.push({ name: 'aaa' })
}
</script>
