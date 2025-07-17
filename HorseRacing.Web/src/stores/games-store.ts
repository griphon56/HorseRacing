import { defineStore } from 'pinia';
import { ref } from 'vue';
import { makeApiWrapper } from '~/utils/api-wrapper'
import { useAuthStore } from './auth-store'
import type { CreateGameRequest } from '~/interfaces/api/contracts/model/game/requests/create-game/create-game-request';
import type { CreateGameResponse } from '~/interfaces/api/contracts/model/game/responses/create-game/create-game-response';
import type { GetGameRequest } from '~/interfaces/api/contracts/model/game/requests/get-game/get-game-request';
import type { GetGameResponse } from '~/interfaces/api/contracts/model/game/responses/get-game/get-game-response';
import type { GetWaitingGamesResponse } from '~/interfaces/api/contracts/model/game/responses/get-waiting-games/get-waiting-games-response';
import type { JoinGameWithBetRequest } from '~/interfaces/api/contracts/model/game/requests/join-game-with-bet/join-game-with-bet-request';
import type { GetAvailableSuitRequest } from '~/interfaces/api/contracts/model/game/requests/get-available-suit/get-available-suit-request';
import type { GetAvailableSuitResponse } from '~/interfaces/api/contracts/model/game/responses/get-available-suit/get-available-suit-response';
import type { StartGameRequest } from '~/interfaces/api/contracts/model/game/requests/start-game/start-game-request';
import type { GetLobbyUsersWithBetsRequest } from '~/interfaces/api/contracts/model/game/requests/get-lobby-users-with-bets/get-lobby-users-with-bets-request';
import type { GetLobbyUsersWithBetsResponse } from '~/interfaces/api/contracts/model/game/responses/get-lobby-users-with-bets/get-lobby-users-with-bets-response';
import type { CheckPlayerConnectedToGameRequest } from '~/interfaces/api/contracts/model/game/requests/check-player-connected-to-game/check-player-connected-to-game-request';
import type { CheckPlayerConnectedToGameResponse } from '~/interfaces/api/contracts/model/game/responses/check-player-connected-to-game/check-player-connected-to-game-response';

export const useGamesStore = defineStore('games', () => {
  const api = makeApiWrapper({ baseUrl: '/api/v1/Game' });

  async function createGame(request: CreateGameRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('create-game', { body: request, headers });
    return await response.json() as CreateGameResponse;
  }

  async function getGameById(request: GetGameRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('get-game', { body: request, headers });
    return await response.json() as GetGameResponse;
  }

  async function getWaitingGames() {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('get-waiting-games', { headers });
    return await response.json() as GetWaitingGamesResponse;
  }

  async function joinGameWithBet(request: JoinGameWithBetRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('join-game-with-bet', { body: request, headers });
    return await response.json();
  }

  async function getAvailableSuit(request: GetAvailableSuitRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('get-available-suit', { body: request, headers });
    return await response.json() as GetAvailableSuitResponse;
  }

  async function startGame(request: StartGameRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('start-game', { body: request, headers });
    return await response.json();
  }

  async function getLobbyUsersWithBets(request: GetLobbyUsersWithBetsRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('get-lobby-users-with-bets', { body: request, headers });
    return await response.json() as GetLobbyUsersWithBetsResponse;
  }

  async function checkPlayerConnectedToGame(request: CheckPlayerConnectedToGameRequest) {
    const { tokens } = useAuthStore();
    const headers: Record<string, string> = tokens?.AccessToken
      ? { Authorization: `Bearer ${tokens.AccessToken}` }
      : {};
    const response = await api.postJson('check-player-connected-to-game', { body: request, headers });
    return await response.json() as CheckPlayerConnectedToGameResponse;
  }

  return {
    createGame,
    getGameById,
    getWaitingGames,
    joinGameWithBet,
    getAvailableSuit,
    startGame,
    getLobbyUsersWithBets,
    checkPlayerConnectedToGame
  };
});
