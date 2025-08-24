import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';

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
