export interface GameConfig {
  // визуальные
  CARD_WIDTH: number;
  CARD_HEIGHT: number;
  HORSE_SCALE: number;
  DECK_SHIFT_LEFT: number; // сдвиг колоды влево для отрисовки карт

  // трек / логика
  POSITIONS_COUNT: number;
  BARRIER_COUNT: number;
  STEP_SCALE: number;

  // тайминги и анимации (ms)
  EVENT_DELAY: number;
  FLIP_STEP: number;
  MOVE_LERP: number;
  MOVE_EPS: number;

  // пути
  CARD_ATLAS_JSON_PATH: string;

  // цвета / отступы
  TRACK_SIDE_MARGIN: number;
  FINISH_OFFSET: number;
  BOTTOM_PADDING: number;
}

export const defaultGameConfig: GameConfig = {
  CARD_WIDTH: 64,
  CARD_HEIGHT: 64,
  HORSE_SCALE: 1,
  DECK_SHIFT_LEFT : 20,
  POSITIONS_COUNT: 8,
  BARRIER_COUNT: 6,
  STEP_SCALE: 0.7,
  EVENT_DELAY: 220,
  FLIP_STEP: 0.18,
  MOVE_LERP: 0.22,
  MOVE_EPS: 0.6,
  CARD_ATLAS_JSON_PATH: '/assets/spritesheet.json',
  TRACK_SIDE_MARGIN: 20,
  FINISH_OFFSET: 64 + 8,
  BOTTOM_PADDING: 12,
};
