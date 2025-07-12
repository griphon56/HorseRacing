<template>
  <div class="games-page">
    <n-h1>Игры</n-h1>
    <n-button type="primary" @click="showModal = true" class="mb-4">Создать игру</n-button>
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
import { NH1, NButton, NModal, NForm, NFormItem, NInput, NInputNumber, NSelect, NSpace, type FormInst, type FormRules } from 'naive-ui';
import { useGamesStore } from '~/stores/games-store';
import { useAuthStore } from '~/stores/auth-store';

const gamesStore = useGamesStore();
const authStore = useAuthStore();
const showModal = ref(false);
const loading = ref(false);
const formRef = ref<FormInst | null>(null);

const form = ref({
  name: '',
  betAmount: 1,
  betSuit: 1,
});

const rules: FormRules = {
  name: { required: true, message: 'Введите название', trigger: ['input'] },
  betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
  betSuit: { required: true, type: 'number', message: 'Выберите масть', trigger: ['change'] },
};

const suitOptions = [
    { label: 'Бубны', value: 1 },
    { label: 'Черви', value: 2 },
    { label: 'Пики', value: 3 },
    { label: 'Трефы', value: 4 },
];

async function onCreateGame() {
  if (!formRef.value) return;
  loading.value = true;
  try {
    await formRef.value.validate();
    await gamesStore.createGame({
      Data: {
        UserId: authStore.user?.Id || '',
        Name: form.value.name,
        BetAmount: form.value.betAmount,
        BetSuit: form.value.betSuit,
      }
    });
    showModal.value = false;
    // Очистить форму
    form.value = { name: '', betAmount: 1, betSuit: 1 };
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
.game-list-item {
  cursor: pointer;
  padding: 10px;
  border-bottom: 1px solid #eee;
  transition: background 0.2s;
}
.game-list-item:hover {
  background: #f5f5f5;
}
.game-details {
  margin-top: 20px;
}
</style>
