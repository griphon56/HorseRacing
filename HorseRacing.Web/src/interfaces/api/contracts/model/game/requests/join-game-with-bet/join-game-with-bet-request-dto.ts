export interface JoinGameWithBetRequestDto {
    /** Идентификатор игры */
    GameId: string;
    /** Идентификатор пользователя */
    UserId: string;
    /** Ставка пользователя */
    BetAmount: number;
    /** Масть лошади, которую выбрал пользователь */
    BetSuit: number;
}
