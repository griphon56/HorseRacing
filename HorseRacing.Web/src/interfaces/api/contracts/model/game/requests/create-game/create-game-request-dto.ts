export interface CreateGameRequestDto {
    /** Идентификатор пользователя создавшего игру */
    UserId: string;
    /** Название игры */
    Name: string;
    /** Ставка пользователя создавшего игру */
    BetAmount: number;
    /** Масть лошади, которую выбрал пользователь при создании игры */
    BetSuit: number;
}
