import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { useLocalStorage, useSessionStorage, StorageSerializers } from '@vueuse/core';
import { decodeJwt } from 'jose';
import { useRouter } from 'vue-router';
import type { TokensDto } from '~/interfaces/api/contracts/model/auth/dto/tokens-dto';
import type { UserProfileDto } from '~/interfaces/api/contracts/model/user/dto/user-profile-dto';
import { LoginRequest } from '~/interfaces/api/contracts/model/auth/requests/login/login-request';
import { makeApiWrapper } from '~/utils/api-wrapper';
import type { LoginRequestDto } from '~/interfaces/api/contracts/model/auth/requests/login/login-request-dto';
import type { AuthenticationResponse } from '~/interfaces/api/contracts/model/auth/responses/authentication/authentication-response';
import type { RegistrationRequestDto } from '~/interfaces/api/contracts/model/auth/requests/registration/registration-request-dto';
import type { RegistrationRequest } from '~/interfaces/api/contracts/model/auth/requests/registration/registration-request';

const api = makeApiWrapper({ baseUrl: 'api/v1/Authentication' });

export const useAuthStore = defineStore('auth', () => {
    // 1. Токены в localStorage
    const tokens = useLocalStorage<TokensDto | null>('auth-tokens', null, {
        serializer: StorageSerializers.object,
    });

    // 2. Профиль пользователя в localStorage
    const user = useLocalStorage<UserProfileDto | null>('auth-user', null, {
        serializer: StorageSerializers.object,
    });

    // 3. Куда редиректить после логина
    const afterAuthRedirectPath = useSessionStorage<string | undefined>(
        'after-auth-redirect-path',
        undefined,
        { serializer: StorageSerializers.string }
    );

    const router = useRouter();

    // Вспомогательная функция для проверки валидности access-токена
    function isTokenValid(token: string): boolean {
        try {
            const { exp } = decodeJwt(token);
            return !!exp && exp * 1000 > Date.now();
        } catch {
            return false;
        }
    }

    // 4. Запрос на логин к API
    async function fetchAuthByForm(login: string, pwd: string): Promise<AuthenticationResponse> {
        const dto: LoginRequestDto = { UserName: login, Password: pwd };
        const req = new LoginRequest(dto);
        const resp = await api.postJson('login', { body: req });
        return (await resp.json()) as AuthenticationResponse;
    }

    // 5. Логин по форме: сохраняем токен и профиль, возвращаем ответ
    async function authByForm(login: string, pwd: string): Promise<AuthenticationResponse> {
        const result = await fetchAuthByForm(login, pwd);

        // Сохраняем токены
        tokens.value = {
            AccessToken: result.Data.Token,
            // RefreshToken: result.Data.RefreshToken
        };

        // Сохраняем профиль пользователя
        user.value = {
            Id: result.Data.Id,
            UserName: result.Data.UserName,
            FirstName: result.Data.FirstName,
            LastName: result.Data.LastName,
            Email: result.Data.Email,
        };

        return result;
    }

    // 6. Попытаться получить токены (обновить если устарели)
    async function getTokens(): Promise<TokensDto | null> {
        if (tokens.value && !isTokenValid(tokens.value.AccessToken)) {
            // здесь можно вставить логику refresh-токена
            tokens.value = null;
            user.value = null;
        }
        return tokens.value;
    }

    // 7. Сбросить всё (выход)
    function logout() {
        tokens.value = null;
        user.value = null;
        router.push({ name: 'Auth' });
    }

    // 8. Отформатированное имя пользователя, например "Иванов И."
    const userFormattedName = computed(() => {
        if (!user.value) return '';
        const { LastName, FirstName, UserName } = user.value;

        const lastName = LastName?.trim() || '';
        const firstName = FirstName?.trim() || '';
        const username = UserName?.trim() || '';

        const fullName = [lastName, firstName].filter(Boolean).join(' ');
        return fullName ? `${fullName} (${username})` : username;
    });

    // 9. Регистрация пользователя
    async function register(request: RegistrationRequest): Promise<void> {
        const result = await api.postJson('register', { body: request });

        const response = (await result.json()) as AuthenticationResponse;

        // Сохраняем токены
        tokens.value = {
            AccessToken: response.Data.Token,
            // RefreshToken: result.Data.RefreshToken
        };

        // Сохраняем профиль пользователя
        user.value = {
            Id: response.Data.Id,
            UserName: response.Data.UserName,
            FirstName: response.Data.FirstName,
            LastName: response.Data.LastName,
            Email: response.Data.Email,
        };
    }

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
        logout,
        register,
    };
});
