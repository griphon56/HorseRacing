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
    <GameList :games="games" @selectGame="onSelectGame" />
    <n-modal v-model:show="showJoinModal" preset="dialog" title="Присоединиться к игре">
      <n-form ref="joinFormRef" :model="joinForm" :rules="joinRules" label-placement="top">
        <n-form-item label="Ставка" path="betAmount">
          <n-input-number v-model:value="joinForm.betAmount" :min="1" placeholder="Введите ставку" />
        </n-form-item>
        <n-form-item label="Масть" path="betSuit">
          <n-select v-model:value="joinForm.betSuit" :options="availableSuits" placeholder="Выберите масть" />
        </n-form-item>
      </n-form>
      <template #action>
        <n-space>
          <n-button @click="showJoinModal = false">Отмена</n-button>
          <n-button type="primary" :loading="joinLoading" @click="onJoinGame">Присоединиться</n-button>
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
import GameList from '~/core/games/game-list.vue';

const gamesStore = useGamesStore();
const authStore = useAuthStore();
const showModal = ref(false);
const loading = ref(false);
const formRef = ref<FormInst | null>(null);
const games = ref<any[]>([]);
const selectedGame = ref<any | null>(null);
const showJoinModal = ref(false);
const joinFormRef = ref<FormInst | null>(null);
const joinLoading = ref(false);
const joinForm = ref({ betAmount: 1, betSuit: 1 });
const availableSuits = ref<{ label: string; value: number }[]>([]);

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

const joinRules: FormRules = {
  betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
  betSuit: { required: true, type: 'number', message: 'Выберите масть', trigger: ['change'] },
};


enum SuitType {
  Diamonds = 1,
  Hearts = 2,
  Spades = 3,
  Clubs = 4,
}

const suitOptions = [
  { label: 'Бубны', value: SuitType.Diamonds },
  { label: 'Черви', value: SuitType.Hearts },
  { label: 'Пики', value: SuitType.Spades },
  { label: 'Трефы', value: SuitType.Clubs },
];

async function loadGames() {
  const response = await gamesStore.getWaitingGames();
  games.value = response?.Data?.Games || [];
}

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
        BetSuit: form.value.betSuit as SuitType,
      }
    });
    showModal.value = false;
    form.value = { name: '', betAmount: 1, betSuit: 1 };
    await loadGames();
  } finally {
    loading.value = false;
  }
}

async function onSelectGame(game: any) {
  selectedGame.value = game;
  // Получить доступные масти для выбранной игры
  const response = await gamesStore.getAvailableSuit({ Data :{ Id: game.GameId }});

    const suits = response?.DataValues || [];
    availableSuits.value = suits.map((suit: any) => ({
        label: suit.name ?? suit.Name,
        value: suit.value ?? suit.Value
    }));
    joinForm.value = { betAmount: 1, betSuit: availableSuits.value[0]?.value ?? SuitType.Diamonds };
    showJoinModal.value = true;
}

async function onJoinGame() {
  if (!joinFormRef.value || !selectedGame.value) return;
  joinLoading.value = true;
  try {
    await joinFormRef.value.validate();
    await gamesStore.joinGameWithBet({
      Data: {
        GameId: selectedGame.value.GameId,
        UserId: authStore.user?.Id || '',
        BetAmount: joinForm.value.betAmount,
        BetSuit: joinForm.value.betSuit as SuitType,
      }
    });
    showJoinModal.value = false;
    await loadGames();
  } finally {
    joinLoading.value = false;
  }
}

loadGames();
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
