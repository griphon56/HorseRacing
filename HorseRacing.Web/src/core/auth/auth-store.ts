import { StorageSerializers, useLocalStorage, useSessionStorage } from '@vueuse/core';
import { decodeJwt } from "jose";
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { AuthMode, config } from '~/config';
import { makeBaseController } from '~/core/api';
import { EntityRequestDto, EntityResponseDto, LoginRequestDto, TokensDto, UserAuthProfileDto, UserProfileDto } from '~/interfaces';

export interface IAuthResult {
    tokens: TokensDto;
    user: UserProfileDto;
}

const { api } = makeBaseController({ controllerRoute: 'api/account' });

async function fetchAuthByForm(login: string, password: string): Promise<IAuthResult> {
    const response = await api.postJson<LoginRequestDto>('login-form', {
        body: { Login: login, Password: password }
    });
    const { Data } = await response.json() as EntityResponseDto<UserAuthProfileDto>;
    const { Tokens, ...UserProfile } = Data;
    return { tokens: Tokens, user: UserProfile };
}

async function fetchRefreshTokens(data: EntityRequestDto<string>): Promise<TokensDto> {
    const response = await api.postJson('refresh-tokens', {
        body: data,
    });
    const { Data: tokens } = await response.json() as EntityResponseDto<TokensDto>;
    return tokens;
}

async function fetchGetUserByToken(accessToken: string): Promise<UserProfileDto> {
    const response = await api.postJson('get-current-user', {
        headers: { 'Authorization': `Bearer ${accessToken}` }
    });
    const { Data } = await response.json() as EntityResponseDto<UserProfileDto>;
    return Data;
}

function shouldRefreshToken(token: string) {
    const tokenData = decodeJwt(token);
    const tokenExpiration = tokenData.exp! * 1000;
    return tokenExpiration - Date.now() < 60 * 1000;
}

const usePrivateAuthStore = defineStore('private-auth', () => {
    const tokens = useLocalStorage<TokensDto | null>('auth-tokens', null, { serializer: StorageSerializers.object });
    return { tokens };
});

export const useAuthStore = defineStore('auth', () => {
    const auth = usePrivateAuthStore();
    const user = ref<UserProfileDto | undefined>();
    const afterAuthRedirectPath = useSessionStorage<string | undefined>('after-auth-redirect-path', undefined, { serializer: StorageSerializers.string });

    async function authByForm(login: string, password: string): Promise<IAuthResult> {
        const result = await fetchAuthByForm(login, password);
        auth.tokens = result.tokens;
        user.value = result.user;
        return result;
    }

    let activeTokenLock: Promise<TokensDto> | undefined;
    async function receiveTokens() {
        if (auth.tokens && shouldRefreshToken(auth.tokens.AccessToken)) {
            auth.tokens = await fetchRefreshTokens({ Data: auth.tokens.RefreshToken })
                .catch(() => null);
        }

        // Выполнится, если непросроченный токен уже был в наличии или если рефреш прошёл успешно
        if (auth.tokens) {
            return auth.tokens;
        }

        // Выполнится если токенов не было изначально
        // или если рефреш прошёл неудачно
        if (config.authMode == AuthMode.OnlyForm) {
            throw new Error('Требуется авторизация');
        }
    }

    async function getTokens(): Promise<TokensDto | null> {
        if (activeTokenLock) {
            return activeTokenLock;
        }
        try {
            activeTokenLock = receiveTokens();
            const tokens = await activeTokenLock;
            return tokens;
        } catch (err) {
            auth.tokens = null;
            user.value = undefined;
            return null;
        } finally {
            activeTokenLock = undefined;
        }
    }

    // async function logOut(): Promise<void> {
    //   if (!auth.tokens) {
    //     console.warn('logOut without tokens');
    //     return;
    //   }
    //   await fetchLogOut(auth.tokens);
    //   auth.tokens = null;
    //   user.value = undefined;
    // }

    async function loadUserOrTryLogIn(): Promise<UserProfileDto | null> {
        const tokens = await getTokens();
        if (tokens == null) {
            return null;
        }
        if (user.value) {
            return user.value;
        }
        try {
            user.value = await fetchGetUserByToken(tokens.AccessToken);
            return user.value;
        } catch {
            console.log('unable to load user data');
            return null;
        }
    }

    /**
     * @example
     * Иванов Иван ... -> Иванов И.
     * Петров -> Петров
     */
    const userFormattedName = computed(() => {
        const FI = user.value?.Name;
        if (!FI) {
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
        // logOut,
        loadUserOrTryLogIn,
    };
});
