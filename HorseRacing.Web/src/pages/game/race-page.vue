<template>
  <div class="race-page">
    <n-h1>Гонка</n-h1>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { NH1 } from 'naive-ui';
import { useRoute, useRouter } from 'vue-router';
import * as signalR from '@microsoft/signalr';


</script>

<style scoped>
.race-page {
  padding: 20px;
  max-width: 600px;
  margin: 0 auto;
}
</style>
