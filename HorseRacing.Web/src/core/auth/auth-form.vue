<template>
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-4">
          <div class="card p-4">
            <h3 class="card-title mb-4 text-center">Вход</h3>
            <form @submit.prevent="onSubmit">
              <div class="mb-3">
                <label for="username" class="form-label">Логин</label>
                <input
                  id="username"
                  v-model="username"
                  type="text"
                  class="form-control"
                  required
                />
              </div>
              <div class="mb-3">
                <label for="password" class="form-label">Пароль</label>
                <input
                  id="password"
                  v-model="password"
                  type="password"
                  class="form-control"
                  required
                />
              </div>
              <button
                type="submit"
                class="btn btn-primary w-100"
                :disabled="loading"
              >
                {{ loading ? 'Входим...' : 'Войти' }}
              </button>
              <p v-if="error" class="text-danger mt-2">{{ error }}</p>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>

  <script setup lang="ts">
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '~/stores/auth-store'

  const router = useRouter()
  const authStore = useAuthStore()

  const username = ref('')
  const password = ref('')
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function onSubmit() {
    loading.value = true
    error.value = null

    try {
      await authStore.authByForm(username.value, password.value)

      // после успешного логина
      const redirect = authStore.afterAuthRedirectPath || '/'
      authStore.afterAuthRedirectPath = undefined
      await router.replace(redirect)
    } catch (e: any) {
      error.value = e?.message || 'Ошибка входа'
    } finally {
      loading.value = false
    }
  }
  </script>

  <style scoped>
  .card {
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  }
  </style>
