export interface CreateGameRequest {
  userId: string;
  name: string;
  betAmount: number;
  betSuit: string;
}

export interface CreateGameResponse {
  id: string;
  name: string;
  betAmount: number;
  betSuit: string;
}

export interface GetGameRequest {
  id: string;
}

export interface GetGameResponse {
  id: string;
  name: string;
  betAmount: number;
  betSuit: string;
}

export interface GetWaitingGamesResponse {
  games: Array<{
    id: string;
    name: string;
    betAmount: number;
    betSuit: string;
  }>;
}

export interface JoinGameWithBetRequest {
  gameId: string;
  userId: string;
  betAmount: number;
  betSuit: string;
}

export interface GetAvailableSuitRequest {
  id: string;
}

export interface GetAvailableSuitResponse {
  suits: string[];
}

export interface StartGameRequest {
  id: string;
}
