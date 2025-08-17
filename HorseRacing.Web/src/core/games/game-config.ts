export interface GameConfig {
    // Экранные размеры
    MAX_DEVICE_PIXEL_RATIO: number; // максимальное значение devicePixelRatio, при котором будет использоваться высокое качество графики
    DESIGN_WIDTH: number; // ширина экрана в пикселях
    DESIGN_HEIGHT: number; // высота экрана в пикселях

    // визуальные
    CARD_WIDTH: number;
    CARD_HEIGHT: number;
    HORSE_RIGHT_EXTRA: number; // дополнительный правый запас (в пикселях) поверх ширины карты и сдвига колоды (CARD_WIDTH + DECK_SHIFT_LEFT), который резервируется справа, чтобы лошади не заходили на область колоды/сброса.
    HORSE_LEFT_PADDING: number; // отступ слева для лошади, чтобы она не была прижата к краю экрана
    DECK_ACTIVE_GAP: number; // отступ между колодой и активной картой
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
    TRACK_SIDE_MARGIN_X: number;
    TRACK_SIDE_MARGIN_Y: number;
    FINISH_OFFSET: number;
    BOTTOM_PADDING: number;
}

export const defaultGameConfig: GameConfig = {
    CARD_WIDTH: 64,
    CARD_HEIGHT: 64,
    HORSE_RIGHT_EXTRA: 24,
    HORSE_LEFT_PADDING: 65,
    DECK_ACTIVE_GAP: 12,
    DECK_SHIFT_LEFT: 20,
    POSITIONS_COUNT: 8,
    BARRIER_COUNT: 6,
    STEP_SCALE: 0.7,
    EVENT_DELAY: 220,
    FLIP_STEP: 0.18,
    MOVE_LERP: 0.22,
    MOVE_EPS: 0.6,
    CARD_ATLAS_JSON_PATH: '/assets/spritesheet.json',
    TRACK_SIDE_MARGIN_X: 5,
    TRACK_SIDE_MARGIN_Y: 5,
    FINISH_OFFSET: 64,
    BOTTOM_PADDING: 12,
    MAX_DEVICE_PIXEL_RATIO: 2,
    DESIGN_WIDTH: 360,
    DESIGN_HEIGHT: 800
};
