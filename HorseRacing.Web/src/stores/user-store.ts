import { defineStore } from 'pinia';
import type { GetUserRequest } from '~/interfaces/api/contracts/model/user/requests/get-user/get-user-request';
import { makeApiWrapper } from '~/utils/api-wrapper';
import { useAuthStore } from './auth-store';
import type { GetUserResponse } from '~/interfaces/api/contracts/model/user/responses/get-user/get-user-response';
import type { UpdateUserRequest } from '~/interfaces/api/contracts/model/user/requests/update-user/update-user-request';
import type { GetAccountBalanceRequest } from '~/interfaces/api/contracts/model/user/requests/get-account-balance/get-account-balance-request';
import type { GetAccountBalanceResponse } from '~/interfaces/api/contracts/model/user/responses/get-account-balance/get-account-balance-response';

export const useUserStore = defineStore('user', () => {
    const api = makeApiWrapper({ baseUrl: '/api/v1/User' });

    async function getUser(request: GetUserRequest) {
        const { tokens } = useAuthStore();
        const headers: Record<string, string> = tokens?.AccessToken
            ? { Authorization: `Bearer ${tokens.AccessToken}` }
            : {};
        const response = await api.postJson('get-user', { body: request, headers });
        return (await response.json()) as GetUserResponse;
    }
    async function updateUser(request: UpdateUserRequest) {
        const { tokens } = useAuthStore();
        const headers: Record<string, string> = tokens?.AccessToken
            ? { Authorization: `Bearer ${tokens.AccessToken}` }
            : {};
        const response = await api.postJson('update-user', { body: request, headers });
        return await response.json();
    }
    async function getAccountBalance(request: GetAccountBalanceRequest) {
        const { tokens } = useAuthStore();
        const headers: Record<string, string> = tokens?.AccessToken
            ? { Authorization: `Bearer ${tokens.AccessToken}` }
            : {};
        const response = await api.postJson('get-account-balance', { body: request, headers });
        return (await response.json()) as GetAccountBalanceResponse;
    }

    return {
        getUser,
        updateUser,
        getAccountBalance,
    };
});
