import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { GameDeckCardDto } from '../../dto/game-deck-card-dto';
import type { HorseBetDto } from '../../dto/horse-bet-dto';
import type { GameEventDto } from '../../dto/game-event-dto';

export interface PlayGameResponseDto extends BaseDto {
    /** Код игры */
    GameId: string;
    /** Назване игры */
    Name: string;
    /** Колода до начала игры */
    InitialDeck: GameDeckCardDto[];
    /** Информация о лошади и игроке который на неё поставил */
    HorseBets: HorseBetDto[];
    /** Все события по шагам */
    Events: GameEventDto[];
}
