<template>
  <div>
    jhgndifgdifg

    <button @click="counter++">go to account</button>

    <input type="text" v-model="nameInput" />
    <input type="password" v-model="passwordInput" />
    <button @click="login(nameInput, passwordInput)">LOGIN</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { counter } from '~/stores/counter'
import { makeApiWrapper } from '~/utils/api-wrapper'

const router = useRouter()

const goToAccount = () => {
  router.push({ name: 'account' })
}

const nameInput = ref('')
const passwordInput = ref('')

const api = makeApiWrapper({ baseUrl: '/api/v1/Authentication' })

const login = async (name: string, password: string) => {
  const response = await api.postJson('login', {
    body: {
      Data: {
        UserName: name,
        Password: password,
      },
    },
  })
  const result = await response.json()
  console.log(result)
}
</script>
