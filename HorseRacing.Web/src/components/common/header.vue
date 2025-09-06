<template>
    <div class="header">
        <div class="header-content">
            <span class="player-name">{{ playerName }}</span>
            <span class="balance">
                {{ balance }}
            </span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '~/stores/auth-store';
import { useUserStore } from '~/stores/user-store';

const authStore = useAuthStore();
const userStore = useUserStore();
const playerName = authStore.userFormattedName || 'Гость';
const balance = ref(0);

onMounted(async () => {
    const result =
        (await userStore.getAccountBalance({ Data: { UserId: authStore.user?.Id ?? '' } })) ?? 0;
    balance.value = result.Data?.Balance ?? 0;
});
</script>

<style scoped>
.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px;
    background-color: #f5f5f5;
}

.header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.player-name {
    font-weight: bold;
    font-size: 16px;
}

.balance {
    display: flex;
    align-items: center;
    gap: 5px;
    font-weight: bold;
}
</style>
