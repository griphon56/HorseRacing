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
import { makeApiWrapper } from '~/utils/api-wrapper'

const model = ref({
    username: '',
    password: '',
})
const router = useRouter()

const api = makeApiWrapper({ baseUrl: '/api/v1/Authentication' })

const handleLogin = async () => {
    const response = await api.postJson('login', {
        body: {
            Data: {
                UserName: model.value.username,
                Password: model.value.password,
            },
        },
    })
    const result = await response.json()

    if (response.ok) {
        localStorage.setItem('token', result.Data.Token)
        router.push('/')
    } else {
        alert(result.Message)
    }
}
</script>
