import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useLocalStorage, useSessionStorage, StorageSerializers } from '@vueuse/core'
import { decodeJwt } from 'jose'
import { useRouter } from 'vue-router'
import type { LoginDto } from '~/interfaces/api/contracts/model/auth/dto/login-dto'
import type { TokensDto } from '~/interfaces/api/contracts/model/auth/dto/tokens-dto'
import type { UserProfileDto } from '~/interfaces/api/contracts/model/user/dto/user-profile-dto'
import type { AuthenticationResponse } from '~/interfaces/api/contracts/model/responses/authentication-response'
import { LoginRequest } from '~/interfaces/api/contracts/model/auth/requests/login-request'
import { makeApiWrapper } from '~/utils/api-wrapper'

const api = makeApiWrapper({ baseUrl: 'api/v1/Authentication' })

export const useAuthStore = defineStore('auth', () => {
  // 1. Токены в localStorage
  const tokens = useLocalStorage<TokensDto | null>('auth-tokens', null, {
    serializer: StorageSerializers.object
  })

  // 2. Профиль пользователя в localStorage
  const user = useLocalStorage<UserProfileDto | null>('auth-user', null, {
    serializer: StorageSerializers.object
  })

  // 3. Куда редиректить после логина
  const afterAuthRedirectPath = useSessionStorage<string | undefined>(
    'after-auth-redirect-path',
    undefined,
    { serializer: StorageSerializers.string }
  )

  const router = useRouter()

  // Вспомогательная функция для проверки валидности access-токена
  function isTokenValid(token: string): boolean {
    try {
      const { exp } = decodeJwt(token)
      return !!exp && exp * 1000 > Date.now()
    } catch {
      return false
    }
  }

  // 4. Запрос на логин к API
  async function fetchAuthByForm(login: string, pwd: string): Promise<AuthenticationResponse> {
    const dto: LoginDto = { UserName: login, Password: pwd }
    const req = new LoginRequest(dto)
    const resp = await api.postJson('login', { body: req })
    return (await resp.json()) as AuthenticationResponse
  }

  // 5. Логин по форме: сохраняем токен и профиль, возвращаем ответ
  async function authByForm(login: string, pwd: string): Promise<AuthenticationResponse> {
    const result = await fetchAuthByForm(login, pwd)

    // Сохраняем токены
    tokens.value = {
      AccessToken: result.Data.Token,
     // RefreshToken: result.Data.RefreshToken
    }

    // Сохраняем профиль пользователя
    user.value = {
      Id: result.Data.Id,
      Username: result.Data.Username,
      FirstName: result.Data.FirstName,
      LastName: result.Data.LastName,
      Email: result.Data.Email
    }

    return result
  }

  // 6. Попытаться получить токены (обновить если устарели)
  async function getTokens(): Promise<TokensDto | null> {
    if (tokens.value && !isTokenValid(tokens.value.AccessToken)) {
      // здесь можно вставить логику refresh-токена
      tokens.value = null
      user.value = null
    }
    return tokens.value
  }

  // 7. Сбросить всё (выход)
  function logout() {
    tokens.value = null
    user.value = null
    router.push({ name: 'Auth' })
  }

  // 8. Отформатированное имя пользователя, например "Иванов И."
  const userFormattedName = computed(() => {
    if (!user.value) return ''
    const { LastName, FirstName } = user.value
    const initials = FirstName ? FirstName[0] + '.' : ''
    return `${LastName} ${initials}`
  })

  return {
    // state
    tokens,
    user,
    afterAuthRedirectPath,
    // getters
    userFormattedName,
    // actions
    authByForm,
    getTokens,
    logout
  }
})
