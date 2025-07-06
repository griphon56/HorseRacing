export interface GameResultDto {
    /** Место, позиция */
    Position: number;
    /** Масть */
    BetSuit: string;
    /** Код игры */
    GameId: string;
    /** Код пользователя */
    UserId: string;
    /** Полное имя пользователя */
    FullName: string;
    /** Победитель */
    IsWinner: boolean;
}
