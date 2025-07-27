<template>
  <div class="create-room-page">
    <n-h1>Создать игру</n-h1>
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
    <n-space class="mt-4">
      <n-button @click="goBack">Назад</n-button>
      <n-button type="primary" :loading="loading" @click="onCreateGame">Создать</n-button>
    </n-space>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { NForm, NFormItem, NInput, NInputNumber, NSelect, NButton, NH1, NSpace, type FormInst, type FormRules } from 'naive-ui';
import { useGamesStore } from '~/stores/games-store';
import { useAuthStore } from '~/stores/auth-store';
import { RouteName } from '~/interfaces';
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';
import { signalRService } from '~/core/signalr/signalr-service';

const suitOptions = [
  { label: 'Бубны', value: SuitType.Diamonds },
  { label: 'Черви', value: SuitType.Hearts },
  { label: 'Пики', value: SuitType.Spades },
  { label: 'Трефы', value: SuitType.Clubs },
];

const gamesStore = useGamesStore();
const authStore = useAuthStore();
const router = useRouter();

const loading = ref(false);
const formRef = ref<FormInst | null>(null);
const form = ref({
  name: '',
  betAmount: 1,
  betSuit: SuitType.Diamonds,
});

const rules: FormRules = {
  name: { required: true, message: 'Введите название', trigger: ['input'] },
  betAmount: { required: true, type: 'number', message: 'Введите ставку', trigger: ['input'] },
  betSuit: { required: true, type: 'number', message: 'Выберите масть', trigger: ['change'] },
};

function goBack() {
  router.push({ name: RouteName.Games });
}

async function onCreateGame() {
  if (!formRef.value) return;
  loading.value = true;
  try {
    await formRef.value.validate();
    const response = await gamesStore.createGame({
      Data: {
        UserId: authStore.user?.Id || '',
        Name: form.value.name,
        BetAmount: form.value.betAmount,
        BetSuit: form.value.betSuit as SuitType,
      }
    });

    const gameId = response?.Data?.GameId;
    if (gameId) {
      await signalRService.joinToGame(gameId);
      router.push({ name: RouteName.Lobby, params: { id: gameId } });
    }
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.create-room-page {
  padding: 20px;
  max-width: 400px;
  margin: 0 auto;
}
.mt-4 {
  margin-top: 20px;
}
</style>
