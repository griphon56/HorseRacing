import { defineStore } from 'pinia';
import { ref } from 'vue';
import type {
  CreateGameRequest,
  GetGameRequest,
  JoinGameWithBetRequest,
  GetAvailableSuitRequest,
} from '~/interfaces/api/contracts/games';
import { apiWrapper } from '~/utils/api-wrapper';

// Определяем интерфейс Game для типизации массива games
export interface Game {
  id: string;
  name: string;
  betAmount: number;
  betSuit: string;
}

export const useGamesStore = defineStore('games', () => {
  const games = ref<Game[]>([]);

  async function createGame(request: CreateGameRequest) {
    const response = await apiWrapper.postJson('/create-game', request);
    games.value.push(response.data);
  }

  async function getGameById(request: GetGameRequest) {
    const response = await apiWrapper.postJson('/get-game', request);
    return response.data;
  }

  async function getWaitingGames() {
    const response = await apiWrapper.postJson('/get-waiting-games');
    games.value = response.data.games;
  }

  async function joinGameWithBet(request: JoinGameWithBetRequest) {
    await apiWrapper.postJson('/join-game-with-bet', request);
    await getWaitingGames();
  }

  async function getAvailableSuit(request: GetAvailableSuitRequest) {
    const response = await apiWrapper.postJson('/get-available-suit', request);
    return response.data.suits;
  }

  async function startGame(request: { id: string }) {
    await apiWrapper.postJson('/start-game', request);
    await getWaitingGames();
  }

  return {
    games,
    createGame,
    getGameById,
    getWaitingGames,
    joinGameWithBet,
    getAvailableSuit,
    startGame,
  };
});
