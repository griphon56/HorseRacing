import * as PIXI from 'pixi.js';
import type { GameConfig } from '~/core/games/game-config';
import { ZoneType } from '~/interfaces/api/contracts/model/game/enums/zone-type-enum';
import { RankType } from '~/interfaces/api/contracts/model/game/enums/rank-type-enum';

export type AtlasService = {
    getCardTextureFor: (suit: number, rankIndex: number) => PIXI.Texture;
    getBackTexture: () => PIXI.Texture;
};

export class PixiRendererService {
    // external
    private container: HTMLElement;
    private config: GameConfig;
    private atlas: AtlasService;

    // PIXI
    public app!: PIXI.Application;
    private trackContainer!: PIXI.Container;

    // caches / state
    private deckSprites: PIXI.Sprite[] = [];
    private discardSprites: PIXI.Sprite[] = [];
    private currentCardSprite: PIXI.Sprite | null = null;

    private barrierSprites: PIXI.Sprite[] = [];
    private horseSprites: Record<number, PIXI.Sprite> = {};
    private horsePositions: Record<number, number> = {};
    private positionYs: number[] = [];

    private events: any[] = [];
    private stopProcessing = false;
    private processing = false;

    private resizeHandler = () => this.onResize();

    constructor(container: HTMLElement, config: GameConfig, atlasService: AtlasService) {
        this.container = container;
        this.config = config;
        this.atlas = atlasService;
    }

    // ---------------- lifecycle ----------------
    async init() {
        // DPR handling: limit to config.MAX_DEVICE_PIXEL_RATIO to save memory
        const DPR = Math.min(window.devicePixelRatio || 1, this.config.MAX_DEVICE_PIXEL_RATIO);
        const DESIGN_W = Math.round(this.config.DESIGN_WIDTH);
        const DESIGN_H = Math.round(this.config.DESIGN_HEIGHT);

        // Create app in physical pixels (DESIGN * DPR)
        this.app = new PIXI.Application();
        await this.app.init({
            width: Math.round(DESIGN_W * DPR),
            height: Math.round(DESIGN_H * DPR),
            antialias: true,
            backgroundColor: 0x1d3557,
            resolution: DPR,
        });

        // Insert canvas into container
        // app.view is the DOM canvas element in Pixi v8; as fallback use (app as any).canvas
        const canvas = (this.app.canvas) as HTMLCanvasElement;
        if (!canvas) throw new Error('Pixi canvas element not found');

        // Set fixed CSS size to logical DESIGN size (so canvas visually is 360x800)
        canvas.style.width = `${DESIGN_W}px`;
        canvas.style.height = `${DESIGN_H}px`;
        canvas.style.display = 'block';
        this.container.appendChild(canvas);

        // Make stage draw in logical coordinates by scaling it down by DPR
        // (we drew app at physical resolution DESIGN*DPR, but we want logical coords = DESIGN)
        this.app.stage.scale.set(1 / DPR, 1 / DPR);

        // Create track container (all game visuals go here)
        this.trackContainer = new PIXI.Container();
        this.trackContainer.x = this.config.TRACK_SIDE_MARGIN_X;
        this.trackContainer.y = this.config.TRACK_SIDE_MARGIN_Y;
        this.app.stage.addChild(this.trackContainer);

        // Listen resize for container (we do not resize canvas; we reposition/relayout)
        window.addEventListener('resize', this.resizeHandler);
    }

    destroy() {
        try {
            this.stopEvents();
            window.removeEventListener('resize', this.resizeHandler);
            this.clearScene();
            // destroy app but avoid destroying shared textures
            this.app.destroy(true);
        } catch (e) {
            // ignore
        }
    }

    // ---------------- public draw orchestration ----------------
    drawInitial(gameData: any) {
        this.clearScene();
        this.recomputePositionYs();
        this.drawBarriers(gameData.InitialDeck || []);
        this.placeBarriersOnPositions();
        this.drawDeck(gameData.InitialDeck || []);
        this.drawHorses(gameData.HorseBets || []);
        this.placeHorsesAtPositions();
    }

    startEvents(events: any[]) {
        this.events = events ?? [];
        this.stopProcessing = false;
        void this.processEventsSequentially();
    }

    stopEvents() {
        this.stopProcessing = true;
    }

    // ---------------- geometry / layout ----------------
    private computeTrackMetrics() {
        // Верхняя граница трека = смещение "финишной линии"
        const trackTop = this.config.FINISH_OFFSET;

        // Нижняя граница трека = высота экрана минус смещение контейнера и отступ снизу
        const trackBottom = this.config.DESIGN_HEIGHT - this.trackContainer.y - this.config.BOTTOM_PADDING;

        // Высота трека = разница между низом и верхом (но не меньше 200px, чтобы поле не было слишком маленьким)
        const trackHeight = Math.max(200, trackBottom - trackTop);

        // Ширина трека = ширина экрана минус отступы (левый + правый запас 60px)
        const trackWidth = Math.max(400, this.config.DESIGN_WIDTH - this.trackContainer.x - this.config.TRACK_SIDE_MARGIN_X - 60);

        return { trackTop, trackBottom, trackHeight, trackWidth };
    }

    private recomputePositionYs() {
        const { trackTop, trackBottom } = this.computeTrackMetrics();
        const intervals = Math.max(1, this.config.POSITIONS_COUNT - 1);
        const step = ((trackBottom - trackTop) / intervals) * this.config.STEP_SCALE;
        this.positionYs = [];

        for (let p = 0; p < this.config.POSITIONS_COUNT; p++) {
            const lineY = trackBottom - p * step;
            const spriteTopY = lineY - this.config.CARD_HEIGHT;
            this.positionYs.push(spriteTopY);
        }
    }

    // ---------------- scene build / initial draw ----------------
    private safeRemoveAndDestroy(obj?: any | null) {
        if (!obj) return;
        try {
            if (obj.parent) obj.parent.removeChild(obj);
        } catch {}
        try {
            (obj as any).destroy?.({ children: true, texture: false });
        } catch {
            try {
                (obj as any).destroy?.();
            } catch {}
        }
    }

    private clearScene() {
        if (!this.trackContainer) return;
        this.trackContainer.removeChildren();
        this.deckSprites.forEach(s => this.safeRemoveAndDestroy(s));
        this.deckSprites = [];
        this.discardSprites.forEach(s => this.safeRemoveAndDestroy(s));
        this.discardSprites = [];
        this.safeRemoveAndDestroy(this.currentCardSprite);
        this.currentCardSprite = null;

        this.barrierSprites.forEach(s => this.safeRemoveAndDestroy(s));
        this.barrierSprites.length = 0;

        for (const k in this.horseSprites) {
            this.safeRemoveAndDestroy(this.horseSprites[+k]);
            delete this.horseSprites[+k];
        }
        for (const k in this.horsePositions) delete this.horsePositions[+k];
        this.positionYs = [];
    }

    //#region Отрисовка барьеров
    private drawBarriers(initialDeck: any[]) {
        const barriers = (initialDeck || [])
            .filter((c: any) => c.Zone === ZoneType.Table)
            .slice(0, this.config.BARRIER_COUNT);

        for (let i = 0; i < barriers.length; i++) {
            const card = barriers[i];
            let tex: PIXI.Texture;
            try {
                tex = this.atlas.getCardTextureFor(card.CardSuit, card.CardRank);
            } catch (e) {
                console.warn('barrier texture not found', e);
                continue;
            }
            const barrierSprite = new PIXI.Sprite(tex);
            barrierSprite.width = this.config.CARD_WIDTH;
            barrierSprite.height = this.config.CARD_HEIGHT;
            this.trackContainer.addChild(barrierSprite);
            this.barrierSprites.push(barrierSprite);
        }
    }

    private placeBarriersOnPositions() {
        this.recomputePositionYs();
        const leftX = 10;
        for (let i = 0; i < this.barrierSprites.length; i++) {
            const posIndex = i + 1;
            const topY =
                this.positionYs[posIndex] ?? this.positionYs[this.positionYs.length - 1] ?? 0;
            this.barrierSprites[i].x = leftX;
            this.barrierSprites[i].y = topY;
        }
    }
    //#endregion

    //#region Отрисовка колоды
    private drawDeck(initialDeck: any[]) {
        const deckCards = (initialDeck || []).filter((c: any) => c.Zone === ZoneType.Deck);
        this.drawDeckFromData(deckCards);
    }

    private drawDeckFromData(deckCards: any[]) {
        const { trackWidth } = this.computeTrackMetrics();
        const deckX = trackWidth - (this.config.CARD_WIDTH * 3) - this.config.DECK_SHIFT_LEFT; // правая стопка
        const deckY = 6;

        // Очистим старые backs
        this.deckSprites.forEach(s => this.safeRemoveAndDestroy(s));
        this.deckSprites = [];

        // Нарисовать стопку рубашкой вверх (слегка смещённые по y для наложения)
        for (let i = 0; i < deckCards.length; i++) {
            const card = deckCards[i];
            const backTex = this.atlas.getBackTexture();
            const back = new PIXI.Sprite(backTex);
            back.width = this.config.CARD_WIDTH;
            back.height = this.config.CARD_HEIGHT;
            back.x = deckX;
            back.y = deckY + i * 0.35;
            (back as any).__card = card;
            this.trackContainer.addChild(back);
            this.deckSprites.push(back);
        }

        // Рисуем рамку отбоя (placeholder) справа от стопки
        const discardX = deckX + this.config.CARD_WIDTH + 12;
        // Удаляем старые placeholder-ы
        const old = this.trackContainer.children.filter((c: any) => c && c.__deckPlaceholder);
        old.forEach((ph: any) => {
            try { this.trackContainer.removeChild(ph); (ph as any).destroy?.(); } catch {}
        });

        const discardOutline = new PIXI.Graphics();
        (discardOutline as any).__deckPlaceholder = 'discard';
        discardOutline.lineStyle(2, 0x444444, 0.6);
        discardOutline.drawRoundedRect(discardX, deckY, this.config.CARD_WIDTH, this.config.CARD_HEIGHT, 8);
        this.trackContainer.addChild(discardOutline);

        // Если есть уже текущая карта (currentCardSprite) — убедимся что она на позиции отбоя
        if (this.currentCardSprite) {
            // позиция в локальных координатах trackContainer
            const localX = discardX;
            const localY = deckY;
            try { this.currentCardSprite.parent?.removeChild(this.currentCardSprite); } catch {}
            this.currentCardSprite.x = localX;
            this.currentCardSprite.y = localY;
            this.trackContainer.addChild(this.currentCardSprite);
        }
    }

    // Возвращает глобальную позицию (в window coords) для отбоя (куда должны прибывать карты)
    private getDiscardGlobalPos(): { x: number; y: number } {
        const { trackWidth } = this.computeTrackMetrics();
        const deckX = trackWidth - (this.config.CARD_WIDTH * 3) - this.config.DECK_SHIFT_LEFT;
        const deckY = 6;
        const discardX = deckX + this.config.CARD_WIDTH + 8;
        const discardY = deckY;
        return { x: this.trackContainer.x + discardX, y: this.trackContainer.y + discardY };
    }

    // Обновлённый обработчик "взять карту из колоды" — карта летит в отбой, там переворачивается и остаётся.
    private async onGetCardFromDeck(ev: any) {
        if (this.deckSprites.length === 0) return;

        // берем визуально верхнюю карту
        const top = this.deckSprites.pop()!;
        const cardData = ev && ev.CardSuit !== undefined ? ev : (top as any).__card;

        // стартовая глобальная позиция (из визуального back-спрайта)
        const startGlobal = top.getGlobalPosition();

        // создаём "летящую" sprite на сцене (используем текстуру рубашки, как в стопке)
        const fly = new PIXI.Sprite(top.texture);
        (fly as any).__card = cardData;
        fly.x = startGlobal.x;
        fly.y = startGlobal.y;
        this.app.stage.addChild(fly);

        // удаляем визуальный back (он уже "взял" карту)
        this.safeRemoveAndDestroy(top);

        // --- целевая позиция отбоя (глобальные координаты) ---
        let discardGlobal;
        try {
            discardGlobal = this.getDiscardGlobalPos();
        } catch (err) {
            console.warn('getDiscardGlobalPos failed, fallback to (0,0)', err);
            discardGlobal = { x: 0, y: 0 };
        }

        // защита: если не объект, делаем fallback
        if (!discardGlobal || typeof discardGlobal.x !== 'number' || typeof discardGlobal.y !== 'number') {
            console.warn('discardGlobal is invalid, using fallback', discardGlobal);
            discardGlobal = { x: 0, y: 0 };
        }

        // 2) анимируем перемещение в отбой
        await this.animateMoveToAsync(fly, discardGlobal.x, discardGlobal.y);

        // 3) в позиции отбоя переворачиваем в лицо (flip)
        await this.flipSpriteToFaceAsync(fly, cardData);

        // 4) переводим fly в локальные координаты trackContainer и добавляем в него как элемент отбоя
        const local = this.trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), this.app.stage);
        fly.x = local.x;
        fly.y = local.y;
        try { fly.parent?.removeChild(fly); } catch {}
        this.trackContainer.addChild(fly);

        // 5) поместим в discardSprites / currentCardSprite
        this.discardSprites.push(fly);
        this.currentCardSprite = fly;
    }
    //#endregion

    //#region Отрисовка лошадей
    /** Метод возвращает параметры для горизонтального расположения лошадей */
    private computeHorizontalLayout(count: number) {
        // границы по горизонтали внутри трассы: левый запас и правый запас (где колода)
        const leftPadding = this.config.HORSE_LEFT_PADDING ?? 12;
        const rightPadding =
            this.config.CARD_WIDTH + (this.config.HORSE_RIGHT_EXTRA ?? 24);

        const { trackWidth } = this.computeTrackMetrics();
        const usableWidth = Math.max(1, trackWidth - leftPadding - rightPadding);

        // если count === 0, spacing всё равно >0 чтобы не делить на ноль
        const spacing = usableWidth / (Math.max(1, count) + 1);

        // функция получения X для индекса i (0-based)
        const xForIndex = (i: number) => Math.round(leftPadding + spacing * (i + 1) - this.config.CARD_WIDTH / 2);

        return { leftPadding, rightPadding, usableWidth, spacing, xForIndex };
    }

    // create sprites for horses (only create; do not duplicate positioning logic)
    private drawHorses(horseBets: any[]) {
        this.recomputePositionYs();

        const bets = horseBets ?? [];
        const count = Math.max(1, bets.length);
        const layout = this.computeHorizontalLayout(count);

        for (let i = 0; i < bets.length; i++) {
            const bet = bets[i];
            const suit = bet.BetSuit ?? i + 1;

            // если спрайт уже есть — пропускаем создание (будем переиспользовать)
            if (this.horseSprites[suit]) {
                continue;
            }

            // получить текстуру "Туз" для масти (лошадь = туз)
            const tex = this.atlas.getCardTextureFor(suit, RankType.Ace as unknown as number);

            const horseSprite = new PIXI.Sprite(tex);
            horseSprite.width = this.config.CARD_WIDTH;
            horseSprite.height = this.config.CARD_HEIGHT;
            horseSprite.anchor.set(0, 0);

            // начальная X (расчёт одинаковый как для draw, так и для place)
            const x = layout.xForIndex(i);
            horseSprite.x = x;

            // стартовая Y — position 0 (нижняя позиция). positionYs уже пересчитаны
            horseSprite.y = this.positionYs[0] ?? 0;

            this.trackContainer.addChild(horseSprite);
            this.horseSprites[suit] = horseSprite;
            this.horsePositions[suit] = 0;
        }

        // Удаляем спрайты для лошадей, которые уже не присутствуют в bets
        const validSuits = new Set(bets.map(b => b.BetSuit ?? 0));
        for (const key of Object.keys(this.horseSprites).map(k => +k)) {
            if (!validSuits.has(key)) {
                this.safeRemoveAndDestroy(this.horseSprites[key]);
                delete this.horseSprites[key];
                delete this.horsePositions[key];
            }
        }
    }

    // позиционирует уже существующие спрайты по текущим позициям (вызывается onResize / after move / etc.)
    private placeHorsesAtPositions() {
        this.recomputePositionYs();

        const suits = Object.keys(this.horseSprites).map(k => +k);
        if (suits.length === 0) return;

        const count = suits.length;
        const layout = this.computeHorizontalLayout(count);

        // сортируем suits чтобы порядок стран не менялся при переборе
        suits.sort((a, b) => a - b);

        for (let i = 0; i < suits.length; i++) {
            const suit = suits[i];
            const spr = this.horseSprites[suit];
            if (!spr) continue;

            spr.width = this.config.CARD_WIDTH;
            spr.height = this.config.CARD_HEIGHT;
            spr.anchor.set(0, 0);

            // X — определяется индексом i (последовательное размещение)
            const targetX = layout.xForIndex(i);
            spr.x = targetX;

            // Y — по позиции horsePositions[suit], привязка к positionYs
            const pos = Math.max(0, Math.min(this.config.POSITIONS_COUNT - 1, this.horsePositions[suit] ?? 0));
            const targetTopY = this.positionYs[pos] ?? this.positionYs[0] ?? 0;
            spr.y = targetTopY;
        }
    }
    //#endregion

    // ---------------- Events & animations ----------------
    private async processEventsSequentially() {
        if (this.processing) return;
        this.processing = true;
        this.stopProcessing = false;
        for (let i = 0; i < (this.events || []).length; i++) {
            if (this.stopProcessing) break;
            const ev = this.events[i];
            try {
                await this.handleEvent(ev);
            } catch (err) {
                console.warn('handleEvent error', err);
            }
            await this.sleep(this.config.EVENT_DELAY);
        }
        this.processing = false;
    }

    private async handleEvent(ev: any) {
        switch (ev.EventType) {
            case 5: // GetCardFromDeck
                await this.onGetCardFromDeck(ev);
                break;
            case 3: // ObstacleRevealed
                await this.onObstacleRevealed(ev);
                break;
            case 6: // UpdateHorsePosition
                await this.onUpdateHorsePosition(ev);
                break;
            default:
                break;
        }
    }

    private async onObstacleRevealed(ev: any) {
        const card = { CardSuit: ev.CardSuit, CardRank: ev.CardRank, CardOrder: ev.CardOrder };
        const tex = this.atlas.getCardTextureFor(card.CardSuit, card.CardRank);
        const s = new PIXI.Sprite(tex);
        s.width = this.config.CARD_WIDTH;
        s.height = this.config.CARD_HEIGHT;
        this.trackContainer.addChild(s);
        let idx = 0;
        if (typeof ev.CardOrder === 'number' && ev.CardOrder >= 1)
            idx = Math.min(this.config.BARRIER_COUNT - 1, ev.CardOrder - 1);
        const posIndex = idx + 1;
        this.recomputePositionYs();
        s.x = 12;
        s.y = this.positionYs[posIndex] ?? 0;
        this.barrierSprites[idx] = s;
    }

    private async onUpdateHorsePosition(ev: any) {
        const suit = ev.HorseSuit;
        const pos = Math.max(0, Math.min(this.config.POSITIONS_COUNT - 1, ev.Position ?? 0));
        await this.animateHorseToPosition(suit, pos);
    }

    private flipSpriteToFaceAsync(sprite: PIXI.Sprite, cardData: any): Promise<void> {
        return new Promise(resolve => {
            let s = Math.abs(sprite.scale.x) || 1;
            let phase: 'in' | 'out' = 'in';
            const cb = () => {
                if (phase === 'in') {
                    s -= this.config.FLIP_STEP;
                    sprite.scale.x = s;
                    if (s <= 0) {
                        sprite.texture = this.atlas.getCardTextureFor(
                            cardData.CardSuit,
                            cardData.CardRank
                        );
                        (sprite as any).__card = cardData;
                        phase = 'out';
                    }
                } else {
                    s += this.config.FLIP_STEP;
                    sprite.scale.x = s;
                    if (s >= 1) {
                        this.app.ticker.remove(cb);
                        resolve();
                    }
                }
            };
            this.app.ticker.add(cb);
        });
    }

    private animateMoveToAsync(sprite: PIXI.Sprite, tx: number, ty: number): Promise<void> {
        return new Promise(resolve => {
            const cb = () => {
                const dx = tx - sprite.x;
                const dy = ty - sprite.y;
                sprite.x += dx * this.config.MOVE_LERP;
                sprite.y += dy * this.config.MOVE_LERP;
                if (Math.abs(dx) < this.config.MOVE_EPS && Math.abs(dy) < this.config.MOVE_EPS) {
                    sprite.x = tx;
                    sprite.y = ty;
                    this.app.ticker.remove(cb);
                    resolve();
                }
            };
            this.app.ticker.add(cb);
        });
    }

    private animateHorseToPosition(suit: number, position: number): Promise<void> {
        return new Promise(resolve => {
            this.recomputePositionYs();
            const horse = this.horseSprites[suit];
            if (!horse) {
                resolve();
                return;
            }
            const cur = this.horsePositions[suit] ?? 0;
            if (cur === position) {
                resolve();
                return;
            }
            const targetTopY =
                this.positionYs[Math.min(Math.max(0, position), this.positionYs.length - 1)];
            if (Math.abs(horse.y - targetTopY) < this.config.MOVE_EPS) {
                this.horsePositions[suit] = position;
                horse.y = targetTopY;
                resolve();
                return;
            }
            const cb = () => {
                const dy = targetTopY - horse.y;
                horse.y += dy * this.config.MOVE_LERP;
                if (Math.abs(dy) < this.config.MOVE_EPS) {
                    horse.y = targetTopY;
                    this.app.ticker.remove(cb);
                    this.horsePositions[suit] = position;
                    resolve();
                }
            };
            this.app.ticker.add(cb);
        });
    }

    private onResize() {
        try {
            this.app.renderer.resize(this.container.clientWidth, this.container.clientHeight);
        } catch {}
        this.recomputePositionYs();
        this.placeBarriersOnPositions();
        this.placeHorsesAtPositions();

        // rebuild deck visuals preserving remaining card data
        const remaining = this.deckSprites.map(s => (s as any).__card).filter(Boolean);
        this.deckSprites.forEach(s => this.safeRemoveAndDestroy(s));
        this.deckSprites = [];
        this.drawDeckFromData(remaining);
    }

    // small util
    private sleep(ms: number) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}
