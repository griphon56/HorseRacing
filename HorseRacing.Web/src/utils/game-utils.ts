import { GameModeType } from '~/interfaces/api/contracts/model/game/enums/game-mode-type-enum';
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';

export const GameModeOptions = [{ label: 'Классический', value: GameModeType.Classic }];

/**
 * Преобразует режим игры (число enum или строку) в читаемое название.
 */
export function gameModeName(mode: number | string | null | undefined): string {
    if (mode === null || mode === undefined) return '—';
    switch (mode) {
        case GameModeType.Classic:
            return 'Классический';
        default:
            return String(mode);
    }
}

export const SuitOptions = [
    { label: 'Бубны', value: SuitType.Diamonds },
    { label: 'Черви', value: SuitType.Hearts },
    { label: 'Пики', value: SuitType.Spades },
    { label: 'Трефы', value: SuitType.Clubs },
];

/**
 * Преобразует масть (число enum или строку) в читаемое название.
 */
export function suitName(suit: number | string | null | undefined): string {
    if (suit === null || suit === undefined) return '—';
    switch (suit) {
        case SuitType.Diamonds:
            return 'Бубны';
        case SuitType.Hearts:
            return 'Черви';
        case SuitType.Spades:
            return 'Пики';
        case SuitType.Clubs:
            return 'Трефы';
        default:
            return String(suit);
    }
}
