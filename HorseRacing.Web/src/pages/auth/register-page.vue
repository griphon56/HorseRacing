<template>
    <div class="register-page">
        <n-card title="Регистрация" class="register-form">
            <n-form :model="form" :rules="rules" ref="formRef" label-width="120px">
                <n-form-item label="Логин" path="UserName">
                    <n-input v-model:value="form.UserName" placeholder="Введите логин" />
                </n-form-item>
                <n-form-item label="Пароль" path="Password">
                    <n-input
                        type="password"
                        v-model:value="form.Password"
                        placeholder="Введите пароль"
                    />
                </n-form-item>
                <n-form-item label="Фамилия" path="LastName">
                    <n-input v-model:value="form.LastName" placeholder="Введите фамилию" />
                </n-form-item>
                <n-form-item label="Имя" path="FirstName">
                    <n-input v-model:value="form.FirstName" placeholder="Введите имя" />
                </n-form-item>
                <n-form-item label="Email" path="Email">
                    <n-input v-model:value="form.Email" placeholder="Введите email" />
                </n-form-item>
                <n-form-item label="Телефон" path="Phone">
                    <n-input v-model:value="form.Phone" placeholder="Введите телефон" />
                </n-form-item>
                <n-form-item>
                    <n-button type="primary" :loading="loading" @click="onRegister"
                        >Зарегистрироваться</n-button
                    >
                </n-form-item>
            </n-form>
        </n-card>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { NForm, NFormItem, NInput, NButton, NCard } from 'naive-ui';
import { useAuthStore } from '~/stores/auth-store';
import type { RegistrationRequestDto } from '~/interfaces/api/contracts/model/auth/requests/registration/registration-request-dto';
import { RouteName } from '~/interfaces';
import { RegistrationRequest } from '~/interfaces/api/contracts/model/auth/requests/registration/registration-request';

const router = useRouter();
const authStore = useAuthStore();
const loading = ref(false);
const formRef = ref();

const form = ref<RegistrationRequestDto>({
    UserName: '',
    Password: '',
    LastName: '',
    FirstName: '',
    Email: '',
    Phone: '',
});

const rules = {
    UserName: { required: true, message: 'Введите логин', trigger: 'blur' },
    Password: { required: true, message: 'Введите пароль', trigger: 'blur' },
    LastName: { required: true, message: 'Введите фамилию', trigger: 'blur' },
    FirstName: { required: true, message: 'Введите имя', trigger: 'blur' },
    Email: { required: true, message: 'Введите email', trigger: 'blur' },
};

const onRegister = async () => {
    loading.value = true;
    try {
        await formRef.value?.validate();
        await authStore.register(new RegistrationRequest(form.value));
        router.push({ name: RouteName.Games });
    } catch (err) {
        console.error('Ошибка регистрации:', err);
    } finally {
        loading.value = false;
    }
};
</script>

<style scoped>
.register-page {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background-color: #f5f5f5;
}

.register-form {
    width: 400px;
    padding: 20px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    border-radius: 8px;
}
</style>
