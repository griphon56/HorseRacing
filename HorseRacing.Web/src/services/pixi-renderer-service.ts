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
    this.app = new PIXI.Application();
    await this.app.init({
      resizeTo: this.container,
      backgroundColor: 0x1d3557,
      antialias: true,
    });
    // append canvas to DOM
    this.container.appendChild(this.app.canvas);

    // create track container
    this.trackContainer = new PIXI.Container();
    this.trackContainer.x = this.config.TRACK_SIDE_MARGIN;
    this.trackContainer.y = 20;
    this.app.stage.addChild(this.trackContainer);

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
    this.drawPositionLines();
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
    const trackTop = this.config.FINISH_OFFSET;
    const trackBottom = this.app.screen.height - this.trackContainer.y - this.config.BOTTOM_PADDING;
    const trackHeight = Math.max(200, trackBottom - trackTop);
    const trackWidth = Math.max(
      300,
      this.app.screen.width - this.trackContainer.x - this.config.TRACK_SIDE_MARGIN - 120
    );
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
  // ---------------- drawing helpers ----------------
  private drawDashedLine(
    g: PIXI.Graphics,
    x1: number,
    y1: number,
    x2: number,
    y2: number,
    dash = 8,
    gap = 6
  ) {
    const dx = x2 - x1,
      dy = y2 - y1;
    const dist = Math.hypot(dx, dy);
    if (dist === 0) return;
    const step = dash + gap;
    const segments = Math.floor(dist / step);
    for (let i = 0; i < segments; i++) {
      const start = i * step;
      const sx = x1 + (dx * start) / dist;
      const sy = y1 + (dy * start) / dist;
      const ex = x1 + (dx * (start + dash)) / dist;
      const ey = y1 + (dy * (start + dash)) / dist;
      g.moveTo(sx, sy);
      g.lineTo(ex, ey);
    }
  }

  // ---------------- scene build / initial draw ----------------
  private safeRemoveAndDestroy(obj?: any | null) {
    if (!obj) return;
    try { if (obj.parent) obj.parent.removeChild(obj); } catch {}
    try { (obj as any).destroy?.({ children: true, texture: false }); } catch {
      try { (obj as any).destroy?.(); } catch {}
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

  private drawPositionLines() {
    // remove previous pos lines
    for (let i = this.trackContainer.children.length - 1; i >= 0; i--) {
      const ch = this.trackContainer.children[i] as any;
      if (ch && ch.__posLine) {
        this.trackContainer.removeChild(ch);
        this.safeRemoveAndDestroy(ch);
      }
    }

    this.recomputePositionYs();
    const g = new PIXI.Graphics();
    (g as any).__posLine = true;
    const { trackWidth } = this.computeTrackMetrics();
    const xStart = 0, xEnd = trackWidth;
    g.lineStyle(2, 0xffffff, 0.12);
    for (let p = 0; p < this.positionYs.length; p++) {
      const topY = this.positionYs[p];
      const lineY = topY + this.config.CARD_HEIGHT;
      this.drawDashedLine(g, xStart, lineY, xEnd, lineY, 8, 6);
      const label = new PIXI.Text(String(p), { fill: 0xffffff, fontSize: 12 });
      label.x = -28;
      label.y = lineY - label.height / 2;
      (label as any).__posLine = true;
      this.trackContainer.addChild(label);
    }
    this.trackContainer.addChild(g);
  }

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
    const leftX = 12;
    for (let i = 0; i < this.barrierSprites.length; i++) {
      const posIndex = i + 1;
      const topY = this.positionYs[posIndex] ?? this.positionYs[this.positionYs.length - 1] ?? 0;
      this.barrierSprites[i].x = leftX;
      this.barrierSprites[i].y = topY;
    }
  }

  private drawDeck(initialDeck: any[]) {
    const { trackWidth } = this.computeTrackMetrics();
    const deckX = trackWidth - this.config.CARD_WIDTH - this.config.DECK_SHIFT_LEFT;
    const deckY = 6;
    const deck = (initialDeck || []).filter((c: any) => c.Zone === ZoneType.Deck);
    this.deckSprites = [];
    for (let i = 0; i < deck.length; i++) {
      const card = deck[i];
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

    const discardX = deckX + this.config.CARD_WIDTH + 12;
    const discardG = new PIXI.Graphics();
    discardG.lineStyle(2, 0x444444, 0.6);
    discardG.drawRoundedRect(discardX, deckY, this.config.CARD_WIDTH, this.config.CARD_HEIGHT, 8);
    this.trackContainer.addChild(discardG);

    const activeG = new PIXI.Graphics();
    activeG.lineStyle(2, 0x888888, 0.6);
    activeG.drawRoundedRect(deckX, deckY + this.config.CARD_HEIGHT + 14, this.config.CARD_WIDTH, this.config.CARD_HEIGHT, 8);
    this.trackContainer.addChild(activeG);
  }

  private drawHorses(horseBets: any[]) {
    this.recomputePositionYs();
    const bets = horseBets ?? [];
    const count = Math.max(1, bets.length);
    const { trackWidth } = this.computeTrackMetrics();
    const spacing = Math.max(1, trackWidth / (count + 1));

    for (let i = 0; i < bets.length; i++) {
      const bet = bets[i];
      const suit = bet.BetSuit ?? i + 1;
      let tex: PIXI.Texture;
      try {
        tex = this.atlas.getCardTextureFor(suit, (RankType.Ace as unknown) as number);
      } catch (e) {
        console.warn('horse texture not found', e);
        continue;
      }

      const horseSprite = new PIXI.Sprite(tex);
      horseSprite.width = this.config.CARD_WIDTH;
      horseSprite.height = this.config.CARD_HEIGHT;
      horseSprite.anchor.set(0, 0);

      const x = Math.round(spacing * (i + 1) - this.config.CARD_WIDTH / 2);
      horseSprite.x = x;
      horseSprite.y = this.positionYs[0] ?? 0;
      this.trackContainer.addChild(horseSprite);

      this.horseSprites[suit] = horseSprite;
      this.horsePositions[suit] = 0;
    }
  }

  private placeHorsesAtPositions() {
    this.recomputePositionYs();
    const suits = Object.keys(this.horseSprites).map(k => +k);
    if (suits.length === 0) return;
    const { trackWidth } = this.computeTrackMetrics();
    const leftPadding = 8;
    const rightPadding = this.config.CARD_WIDTH + this.config.DECK_SHIFT_LEFT + 24;
    const usableWidth = Math.max(1, trackWidth - leftPadding - rightPadding);
    const count = suits.length;
    const spacing = usableWidth / (count + 1);

    for (let i = 0; i < suits.length; i++) {
      const suit = suits[i];
      const spr = this.horseSprites[suit];
      if (!spr) continue;
      spr.width = this.config.CARD_WIDTH;
      spr.height = this.config.CARD_HEIGHT;
      spr.anchor.set(0, 0);
      const centerX = leftPadding + spacing * (i + 1);
      const targetX = Math.round(centerX - this.config.CARD_WIDTH / 2);
      spr.x = targetX;
      const pos = Math.max(0, Math.min(this.config.POSITIONS_COUNT - 1, this.horsePositions[suit] ?? 0));
      const targetTopY = this.positionYs[pos] ?? this.positionYs[0] ?? 0;
      spr.y = targetTopY;
    }
  }

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

  private async onGetCardFromDeck(ev: any) {
    if (this.deckSprites.length === 0) return;
    const top = this.deckSprites.pop()!;
    const cardData = ev && ev.CardSuit !== undefined ? ev : (top as any).__card;
    const gp = top.getGlobalPosition();
    const fly = new PIXI.Sprite(top.texture);
    (fly as any).__card = cardData;
    fly.x = gp.x; fly.y = gp.y;
    this.app.stage.addChild(fly);
    this.safeRemoveAndDestroy(top);

    if (this.currentCardSprite) {
      const discardGlobal = this.getDiscardGlobalPos();
      await this.flipAndMoveToDiscardAsync(this.currentCardSprite, discardGlobal.x, discardGlobal.y);
      this.discardSprites.push(this.currentCardSprite);
      this.currentCardSprite = null;
    }

    const [ax, ay] = this.getActiveGlobalPosArray();
    await this.animateMoveToAsync(fly, ax, ay);
    await this.flipSpriteToFaceAsync(fly, cardData);

    const local = this.trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), this.app.stage);
    fly.x = local.x; fly.y = local.y;
    this.trackContainer.addChild(fly);
    this.currentCardSprite = fly;
  }

  private async onObstacleRevealed(ev: any) {
    const card = { CardSuit: ev.CardSuit, CardRank: ev.CardRank, CardOrder: ev.CardOrder };
    const tex = this.atlas.getCardTextureFor(card.CardSuit, card.CardRank);
    const s = new PIXI.Sprite(tex);
    s.width = this.config.CARD_WIDTH; s.height = this.config.CARD_HEIGHT;
    this.trackContainer.addChild(s);
    let idx = 0;
    if (typeof ev.CardOrder === 'number' && ev.CardOrder >= 1) idx = Math.min(this.config.BARRIER_COUNT - 1, ev.CardOrder - 1);
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

  private async flipAndMoveToDiscardAsync(sprite: PIXI.Sprite, tx: number, ty: number) {
    await this.flipSpriteToBackAsync(sprite);
    const gp = sprite.getGlobalPosition();
    const fly = new PIXI.Sprite(sprite.texture);
    (fly as any).__card = (sprite as any).__card;
    fly.x = gp.x; fly.y = gp.y;
    this.app.stage.addChild(fly);
    this.safeRemoveAndDestroy(sprite);
    await this.animateMoveToAsync(fly, tx, ty);
    const local = this.trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), this.app.stage);
    fly.x = local.x; fly.y = local.y;
    this.trackContainer.addChild(fly);
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
            sprite.texture = this.atlas.getCardTextureFor(cardData.CardSuit, cardData.CardRank);
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

  private flipSpriteToBackAsync(sprite: PIXI.Sprite): Promise<void> {
    return new Promise(resolve => {
      let s = Math.abs(sprite.scale.x) || 1;
      let phase: 'in' | 'out' = 'in';
      const cb = () => {
        if (phase === 'in') {
          s -= this.config.FLIP_STEP;
          sprite.scale.x = s;
          if (s <= 0) {
            sprite.texture = this.atlas.getBackTexture();
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
          sprite.x = tx; sprite.y = ty;
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
      if (!horse) { resolve(); return; }
      const cur = this.horsePositions[suit] ?? 0;
      if (cur === position) { resolve(); return; }
      const targetTopY = this.positionYs[Math.min(Math.max(0, position), this.positionYs.length - 1)];
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

  // ---------------- helpers ----------------
  private getActiveGlobalPosArray(): [number, number] {
    const { trackWidth } = this.computeTrackMetrics();
    const deckX = trackWidth - this.config.CARD_WIDTH - this.config.DECK_SHIFT_LEFT;
    const deckY = 6;
    return [this.trackContainer.x + deckX, this.trackContainer.y + deckY + this.config.CARD_HEIGHT + 14];
  }

  private getDiscardGlobalPos(): { x: number; y: number } {
    const { trackWidth } = this.computeTrackMetrics();
    const deckX = trackWidth - this.config.CARD_WIDTH - this.config.DECK_SHIFT_LEFT;
    const deckY = 6;
    return { x: this.trackContainer.x + deckX + this.config.CARD_WIDTH + 12, y: this.trackContainer.y + deckY };
  }

  private onResize() {
    try {
      this.app.renderer.resize(this.container.clientWidth, this.container.clientHeight);
    } catch {}
    this.recomputePositionYs();
    this.drawPositionLines();
    this.placeBarriersOnPositions();
    this.placeHorsesAtPositions();

    // rebuild deck visuals preserving remaining card data
    const remaining = this.deckSprites.map(s => (s as any).__card).filter(Boolean);
    this.deckSprites.forEach(s => this.safeRemoveAndDestroy(s));
    this.deckSprites = [];
    this.drawDeckFromData(remaining);
  }

  private drawDeckFromData(deckCards: any[]) {
    const { trackWidth } = this.computeTrackMetrics();
    const deckX = trackWidth - this.config.CARD_WIDTH - this.config.DECK_SHIFT_LEFT;
    const deckY = 6;
    for (let i = 0; i < deckCards.length; i++) {
      const card = deckCards[i];
      const tex = this.atlas.getBackTexture();
      const back = new PIXI.Sprite(tex);
      back.width = this.config.CARD_WIDTH;
      back.height = this.config.CARD_HEIGHT;
      back.x = deckX;
      back.y = deckY + i * 0.35;
      (back as any).__card = card;
      this.trackContainer.addChild(back);
      this.deckSprites.push(back);
    }
  }

  // small util
  private sleep(ms: number) { return new Promise(resolve => setTimeout(resolve, ms)); }
}
