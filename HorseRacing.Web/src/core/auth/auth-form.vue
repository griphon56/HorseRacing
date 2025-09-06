<template>
    <n-card title="Авторизация" class="max-w-md mx-auto mt-10">
        <n-form ref="formRef" :model="form" :rules="rules" label-placement="top">
            <n-form-item label="Логин" path="username">
                <n-input v-model:value="form.username" placeholder="Логин" />
            </n-form-item>

            <n-form-item label="Пароль" path="password">
                <n-input v-model:value="form.password" type="password" placeholder="Пароль" />
            </n-form-item>

            <n-space justify="end" class="mt-4">
                <n-button type="primary" @click="onSubmit" :loading="loading"> Войти </n-button>
                <n-button type="default" @click="goToRegister"> Регистрация </n-button>
            </n-space>

            <p v-if="error" class="text-error mt-2">{{ error }}</p>
        </n-form>
    </n-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '~/stores/auth-store';
import type { FormInst } from 'naive-ui';
import { RouteName } from '~/interfaces';

const router = useRouter();
const authStore = useAuthStore();

const formRef = ref<FormInst | null>(null);
const loading = ref(false);
const error = ref<string | null>(null);

const form = ref({
    username: '',
    password: '',
});

const rules = {
    username: { required: true, message: 'Введите логин', trigger: ['input'] },
    password: { required: true, message: 'Введите пароль', trigger: ['input'] },
};

async function onSubmit() {
    if (!formRef.value) return;
    loading.value = true;
    error.value = null;

    try {
        await formRef.value.validate();
        await authStore.authByForm(form.value.username, form.value.password);

        // редирект после успешного логина
        const redirect = authStore.afterAuthRedirectPath || '/';
        authStore.afterAuthRedirectPath = undefined;
        await router.replace(redirect);
    } catch (err: any) {
        if (Array.isArray(err)) {
            error.value = 'Проверьте правильность введённых данных';
        } else {
            error.value = err.message || 'Ошибка входа';
        }
    } finally {
        loading.value = false;
    }
}

function goToRegister() {
    router.push({ name: RouteName.Register });
}
</script>

<style scoped>
.max-w-md {
    max-width: 400px;
}
.text-error {
    color: var(--n-error-color);
}
</style>
