<template>
    <div class="container">
        <div class="row">
            <div class="col-12 mb-3">
                <label for="username" class="form-label">Логин:</label>
                <input id="username" class="form-control" v-model="model.username" type="text" />
            </div>
            <div class="col-12 mb-3">
                <label for="password" class="form-label">Пароль:</label>
                <input
                    id="password"
                    class="form-control"
                    v-model="model.password"
                    type="password"
                />
            </div>
            <div class="col-12">
                <a href="#" class="btn btn-info text-white" @click="handleLogin()">Войти</a>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from './auth-store'

const model = ref({
    username: '',
    password: '',
})

const router = useRouter()
const authStore = useAuthStore()

function afterAuthRedirect() {
    const { afterAuthRedirectPath = '/' } = authStore
    authStore.afterAuthRedirectPath = undefined
    router.replace(afterAuthRedirectPath)
}

const handleLogin = async () => {
    await authStore.authByForm(model.value.username, model.value.password)
    afterAuthRedirect()
}
</script>
