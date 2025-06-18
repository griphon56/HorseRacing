<template>
  <div class="games-page">
    <n-h1>Игры</n-h1>
    <n-button type="primary" @click="showModal = true" class="mb-4">Создать игру</n-button>
    <GameList @selectGame="onSelectGame" />
    <GameDetails v-if="selectedGame" :game="selectedGame" class="mt-4" />
    <n-modal v-model:show="showModal" preset="dialog" title="Создание игры">
      <n-form ref="formRef" :model="form" :rules="rules" label-placement="top">
        <n-form-item label="Название игры" path="name">
          <n-input v-model:value="form.name" placeholder="Введите название" />
        </n-form-item>
        <n-form-item label="Ставка" path="betAmount">
          <n-input-number v-model:value="form.betAmount" :min="1" placeholder="Введите ставку" />
        </n-form-item>
        <n-form-item label="Масть" path="betSuit">
          <n-select v-model:value="form.betSuit" :options="suitOptions" placeholder="Выберите масть" />
        </n-form-item>
      </n-form>
      <template #action>
        <n-space>
          <n-button @click="showModal = false">Отмена</n-button>
          <n-button type="primary" :loading="loading" @click="onCreateGame">Создать</n-button>
        </n-space>
      </template>
    </n-modal>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { NH1, NButton, NModal, NForm, NFormItem, NInput, NInputNumber, NSelect, NSpace, type FormInst } from 'naive-ui';
import GameList from '~/core/games/GameList.vue';
import GameDetails from '~/core/games/GameDetails.vue';
import { useGamesStore } from '~/stores/games-store';
import { useAuthStore } from '~/stores/auth-store';

const gamesStore = useGamesStore();
const authStore = useAuthStore();
const selectedGame = ref(null);
const showModal = ref(false);
const loading = ref(false);
const formRef = ref<FormInst | null>(null);

const form = ref({
  name: '',
  betAmount: 1,
  betSuit: '',
});

const rules = {
  name: { required: true, message: 'Введите название', trigger: ['input'] },
  betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
  betSuit: { required: true, message: 'Выберите масть', trigger: ['change'] },
};

const suitOptions = [
  { label: 'Черви', value: 'hearts' },
  { label: 'Бубны', value: 'diamonds' },
  { label: 'Трефы', value: 'clubs' },
  { label: 'Пики', value: 'spades' },
];

function onSelectGame(game: any) {
  selectedGame.value = game;
}

async function onCreateGame() {
  if (!formRef.value) return;
  loading.value = true;
  try {
    await formRef.value.validate();
    await gamesStore.createGame({
      userId: authStore.user?.id || '',
      name: form.value.name,
      betAmount: form.value.betAmount,
      betSuit: form.value.betSuit,
    });
    showModal.value = false;
    // Очистить форму
    form.value = { name: '', betAmount: 1, betSuit: '' };
    await gamesStore.getWaitingGames();
  } finally {
    loading.value = false;
  }
}
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
</style>
