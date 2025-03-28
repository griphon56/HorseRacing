import { StorageSerializers, useLocalStorage, useSessionStorage } from '@vueuse/core';
import { decodeJwt } from 'jose';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import type { LoginDto } from '~/interfaces/api/contracts/model/auth/dto/login-dto';
import type { TokensDto } from '~/interfaces/api/contracts/model/auth/dto/tokens-dto';
import { LoginRequest } from '~/interfaces/api/contracts/model/auth/requests/login-request';
import type { AuthenticationResponse } from '~/interfaces/api/contracts/model/responses/authentication-response';
import type { UserProfileDto } from '~/interfaces/api/contracts/model/user/dto/user-profile-dto';
import { makeApiWrapper } from '~/utils/api-wrapper';

const api = makeApiWrapper({ baseUrl: 'api/v1/Authentication' });

async function fetchAuthByForm(login: string, password: string): Promise<AuthenticationResponse> {
    const loginDto: LoginDto = {
        UserName: login,
        Password: password,
    };
    const request = new LoginRequest(loginDto);

    const response = await api.postJson('login', {
        body: request
    });

    const result = await response.json();
    return result as AuthenticationResponse;
}

function isTokenValid(token: string): boolean {
    try {
        const tokenData = decodeJwt(token);
        const tokenExpiration = tokenData.exp! * 1000;
        return tokenExpiration > Date.now();
    } catch {
        return false;
    }
}

const usePrivateAuthStore = defineStore('private-auth', () => {
    const tokens = useLocalStorage<TokensDto | null>('auth-tokens', null, { serializer: StorageSerializers.object });
    return { tokens };
});

export const useAuthStore = defineStore('auth', () => {
    const auth = usePrivateAuthStore();
    const user = ref<UserProfileDto | undefined>();
    const afterAuthRedirectPath = useSessionStorage<string | undefined>(
        'after-auth-redirect-path',
        undefined,
        { serializer: StorageSerializers.string }
    );
    const router = useRouter();

    // Авторизация по форме, сохранение токена и пользователя в стор и перенаправление на страницу игр.
    async function authByForm(login: string, password: string): Promise<AuthenticationResponse> {
        const result = await fetchAuthByForm(login, password);

        // Сохраняем токен в стор
        if (auth.tokens) {
            auth.tokens.AccessToken = result.Data.Token;
        } else {
            auth.tokens = { AccessToken: result.Data.Token };
        }

        // Сохраняем данные пользователя в стор
        if (user.value) {
            user.value.Id = result.Data.Id;
            user.value.Username = result.Data.Username;
            user.value.FirstName = result.Data.FirstName;
            user.value.LastName = result.Data.LastName;
            user.value.Email = result.Data.Email;
        } else {
            user.value = {
                Id: result.Data.Id,
                Username: result.Data.Username,
                FirstName: result.Data.FirstName,
                LastName: result.Data.LastName,
                Email: result.Data.Email
            } as UserProfileDto;
        }

        // Перенаправляем на страницу игр после успешной авторизации
        router.push('/games');
        return result;
    }

    // Если токен присутствует и годен перенаправляем на игры, иначе на страницу авторизации.
    function checkAuthAndRedirect() {
        if (auth.tokens && isTokenValid(auth.tokens.AccessToken)) {
            router.push('/games');
        } else {
            router.push('/auth');
        }
    }

    // Примерная функция для получения токенов (с логикой обновления, если потребуется)
    let activeTokenLock: Promise<TokensDto | null>;
    async function receiveTokens(): Promise<TokensDto | null> {
        if (auth.tokens && !isTokenValid(auth.tokens.AccessToken)) {
            // Здесь можно реализовать логику обновления токена,
            // если refreshToken доступен.
            // Примерно: auth.tokens = await fetchRefreshTokens(...).catch(() => null);
            auth.tokens = null;
        }
        if (auth.tokens) {
            return auth.tokens;
        }
        return null;
    }

    async function getTokens(): Promise<TokensDto | null> {
        try {
            if (activeTokenLock) {
                return activeTokenLock;
            }
            activeTokenLock = receiveTokens();
            const tokens = await activeTokenLock;
            return tokens || null;
        } catch {
            auth.tokens = null;
            user.value = undefined;
            return null;
        }
    }

    /**
     * @example
     * Иванов Иван ... -> Иванов И.
     * Петров -> Петров
     */
    const userFormattedName = computed(() => {
        const FI = user.value?.LastName + ' ' + user.value?.FirstName;
        if (!FI.trim()) {
            return '';
        }
        const [F, I] = FI.split(' ').filter(Boolean);
        if (!I) {
            return F;
        }
        return `${F} ${I[0]}.`;
    });

    return {
        user,
        userFormattedName,
        afterAuthRedirectPath,
        authByForm,
        getTokens,
        checkAuthAndRedirect,
    };
});
