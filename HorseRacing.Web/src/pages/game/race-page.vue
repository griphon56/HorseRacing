<template>
  <div class="race-container">
    <div ref="pixiContainer" class="pixi-stage"></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref } from 'vue';
import * as PIXI from 'pixi.js';
import { RankType } from '~/interfaces/api/contracts/model/game/enums/rank-type-enum';
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response';
import { ZoneType } from '~/interfaces/api/contracts/model/game/enums/zone-type-enum';

// --------------- CONFIG ---------------
const CARD_ATLAS_JSON_PATH = '/assets/spritesheet.json';

const CARD_WIDTH = 64;
const CARD_HEIGHT = 64;
const POSITIONS_COUNT = 8;
const BARRIER_COUNT = 6;
const FINISH_OFFSET = CARD_HEIGHT + 8;
const BOTTOM_PADDING = 12;
const TRACK_SIDE_MARGIN = 20;
const DECK_SHIFT_LEFT = 22;
const EVENT_DELAY = 220;
const MOVE_EPS = 0.6;
const STEP_SCALE = 0.7; // <1 — уменьшаем расстояние между позициями; >1 — увеличиваем
// --------------------------------------

// PIXI refs
const pixiContainer = ref<HTMLDivElement | null>(null);
let app: PIXI.Application;
let trackContainer: PIXI.Container;

// caches
const texturesCache = new Map<string, PIXI.Texture>();
let backTexture: PIXI.Texture | null = null;

// sprites/state
let deckSprites: PIXI.Sprite[] = [];
let discardSprites: PIXI.Sprite[] = [];
let currentCardSprite: PIXI.Sprite | null = null;

const barrierSprites: PIXI.Sprite[] = [];
const horseSprites: Record<number, PIXI.Sprite> = {};
const horsePositions: Record<number, number> = {};
let positionYs: number[] = [];

let gameData: any = null;
let events: any[] = [];
let stopProcessing = false;

const sleep = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// ------------------ Helpers ------------------
// Safe remove from parent and destroy WITHOUT destroying shared textures/baseTexture.
// We intentionally DO NOT pass 'baseTexture' because TS typings don't allow it.
function safeRemoveAndDestroy(obj?: any | null) {
  if (!obj) return;
  try {
    if (obj.parent) obj.parent.removeChild(obj);
  } catch {}
  try {
    // don't destroy textures/baseTexture by default (likely shared)
    (obj as any).destroy?.({ children: true, texture: false });
  } catch {
    try { (obj as any).destroy?.(); } catch {}
  }
}

// ---------------- Atlas load & cache ----------------
// Загружает spritesheet JSON через PIXI.Assets и сохраняет все текстуры в texturesCache.
async function loadCardAtlasAndCache(): Promise<void> {
  // загружаем json (PIXI.Assets корректно обработает spritesheet)
  await PIXI.Assets.load(CARD_ATLAS_JSON_PATH) as PIXI.Spritesheet;

  // Assets.get может вернуть либо объект spritesheet с .textures, либо сам набор текстур
  const res: any = PIXI.Assets.get(CARD_ATLAS_JSON_PATH);
  const texturesObj: Record<string, PIXI.Texture> | undefined = res?.textures ?? res;

  if (!texturesObj || typeof texturesObj !== 'object') {
    throw new Error('Atlas load failed: textures not found');
  }

  // заполняем кеш
  for (const name of Object.keys(texturesObj)) {
    const t = texturesObj[name] as PIXI.Texture;
    if (t) texturesCache.set(name, t);
  }

  // try find back texture by common name
  backTexture = texturesCache.get('card_back.png') ?? null;
}

// Получить текстуру карты по масти и рангу.
// Ожидаем, что в json ключи называются как `${rankIndex}_${suit}.png` (пример: "0_1.png").
function getCardTextureFor(suit: number, rankIndex: number): PIXI.Texture {
  const rank = Math.max(0, Math.min(12, rankIndex));
  const key = `${rank}_${suit}.png`;
  const t = texturesCache.get(key);
  if (!t) throw new Error(`Card texture not found: ${key}`);
  return t;
}

// Текстура рубашки
function getBackTexture(): PIXI.Texture {
  if (backTexture) return backTexture;
  const t = texturesCache.get('card_back.png');
  if (!t) throw new Error('Back texture not found in atlas');
  backTexture = t;
  return t;
}

// ---------------- Geometry/Layout ----------------
function computeTrackMetrics() {
  const trackTop = FINISH_OFFSET;
  const trackBottom = app.screen.height - trackContainer.y - BOTTOM_PADDING;
  const trackHeight = Math.max(200, trackBottom - trackTop);
  const trackWidth = Math.max(300, app.screen.width - trackContainer.x - TRACK_SIDE_MARGIN - 120);
  return { trackTop, trackBottom, trackHeight, trackWidth };
}

function recomputePositionYs() {
  const { trackTop, trackBottom } = computeTrackMetrics();
  const intervals = POSITIONS_COUNT - 1;
  const step = ((trackBottom - trackTop) / intervals) * STEP_SCALE;
  positionYs = [];
  for (let p = 0; p < POSITIONS_COUNT; p++) {
    const lineY = trackBottom - p * step;
    const spriteTopY = lineY - CARD_HEIGHT;
    positionYs.push(spriteTopY);
  }
}

// ---------------- Drawing helpers ----------------
function drawDashedLine(g: PIXI.Graphics, x1: number, y1: number, x2: number, y2: number, dash = 8, gap = 6) {
  const dx = x2 - x1, dy = y2 - y1;
  const dist = Math.hypot(dx, dy);
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

// ---------------- Scene build / initial draw ----------------
function clearScene() {
  trackContainer.removeChildren();
  // destroy deck sprites safely
  deckSprites.forEach(s => safeRemoveAndDestroy(s));
  deckSprites = [];
  discardSprites.forEach(s => safeRemoveAndDestroy(s));
  discardSprites = [];
  safeRemoveAndDestroy(currentCardSprite);
  currentCardSprite = null;

  barrierSprites.forEach(s => safeRemoveAndDestroy(s));
  barrierSprites.length = 0;

  for (const k in horseSprites) {
    safeRemoveAndDestroy(horseSprites[+k]);
    delete horseSprites[+k];
  }
  for (const k in horsePositions) delete horsePositions[+k];
  positionYs = [];
}

function drawPositionLines() {
  // remove previous
  for (let i = trackContainer.children.length - 1; i >= 0; i--) {
    const ch = trackContainer.children[i];
    if ((ch as any).__posLine) {
      trackContainer.removeChild(ch);
      safeRemoveAndDestroy(ch);
    }
  }

  recomputePositionYs();
  const g = new PIXI.Graphics();
  (g as any).__posLine = true;
  const { trackWidth } = computeTrackMetrics();
  const xStart = 0, xEnd = trackWidth;
  g.lineStyle(2, 0xffffff, 0.12);
  for (let p = 0; p < positionYs.length; p++) {
    const topY = positionYs[p];
    const lineY = topY + CARD_HEIGHT;
    drawDashedLine(g, xStart, lineY, xEnd, lineY, 8, 6);
    const label = new PIXI.Text(String(p), { fill: 0xffffff, fontSize: 12 });
    label.x = -28;
    label.y = lineY - label.height / 2;
    (label as any).__posLine = true;
    trackContainer.addChild(label);
  }
  trackContainer.addChild(g);
}

function drawBarriers(initialDeck: any[]) {
  const barriers = (initialDeck || []).filter((c:any) => c.Zone === ZoneType.Table).slice(0, BARRIER_COUNT);
  for (let i = 0; i < barriers.length; i++) {
    const card = barriers[i];

    const tex = getCardTextureFor(card.CardSuit, card.CardRank);

    if (!tex) {
      console.warn(`Текстура ${tex} не найдена в атласе`);
      continue;
    }

    const barrierSprite = new PIXI.Sprite(tex);

    barrierSprite.width = CARD_WIDTH;
    barrierSprite.height = CARD_HEIGHT;

    trackContainer.addChild(barrierSprite);
    barrierSprites.push(barrierSprite);
  }
}

function placeBarriersOnPositions() {
  recomputePositionYs();
  const leftX = 12;
  for (let i = 0; i < barrierSprites.length; i++) {
    const posIndex = i + 1;
    const topY = positionYs[posIndex];
    barrierSprites[i].x = leftX;
    barrierSprites[i].y = topY;
  }
}

function drawDeck(initialDeck: any[]) {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  const deck = (initialDeck || []).filter((c:any) => c.Zone === ZoneType.Deck);
  deckSprites = [];
  for (let i = 0; i < deck.length; i++) {
    const card = deck[i];
    const backTex = getBackTexture();
    const back = new PIXI.Sprite(backTex);
    back.width = CARD_WIDTH; back.height = CARD_HEIGHT;
    back.x = deckX; back.y = deckY + i * 0.35;
    (back as any).__card = card;
    trackContainer.addChild(back);
    deckSprites.push(back);
  }
  const discardX = deckX + CARD_WIDTH + 12;
  const discardG = new PIXI.Graphics();
  discardG.lineStyle(2, 0x444444, 0.6);
  discardG.drawRoundedRect(discardX, deckY, CARD_WIDTH, CARD_HEIGHT, 8);
  trackContainer.addChild(discardG);
  const activeG = new PIXI.Graphics();
  activeG.lineStyle(2, 0x888888, 0.6);
  activeG.drawRoundedRect(deckX, deckY + CARD_HEIGHT + 14, CARD_WIDTH, CARD_HEIGHT, 8);
  trackContainer.addChild(activeG);
}

function drawHorses(horseBets: any[]) {
  recomputePositionYs();

  const bets = horseBets ?? [];
  const count = Math.max(1, bets.length);
  const { trackWidth } = computeTrackMetrics();
  const spacing = Math.max(1, trackWidth / (count + 1));

  for (let i = 0; i < bets.length; i++) {
    const bet = bets[i];
    const suit = bet.BetSuit ?? (i + 1);

    const tex = getCardTextureFor(suit, RankType.Ace);

    if (!tex) {
      console.warn(`Текстура ${tex} не найдена в атласе`);
      continue;
    }

    const horseSprite = new PIXI.Sprite(tex);
    horseSprite.width = CARD_WIDTH;
    horseSprite.height = CARD_HEIGHT;
    horseSprite.anchor.set(0, 0);

    // Позиционирование
    const x = Math.round(spacing * (i + 1) - CARD_WIDTH / 2);
    horseSprite.x = x;
    horseSprite.y = positionYs[0]; // стартовая позиция
    trackContainer.addChild(horseSprite);

    horseSprites[suit] = horseSprite;
    horsePositions[suit] = 0;
  }
}

function placeHorsesAtPositions() {
  recomputePositionYs();
  const suits = Object.keys(horseSprites).map(k => +k);
  if (suits.length === 0) return;
  const { trackWidth } = computeTrackMetrics();
  const leftPadding = 8;
  const rightPadding = CARD_WIDTH + DECK_SHIFT_LEFT + 24;
  const usableWidth = Math.max(1, trackWidth - leftPadding - rightPadding);
  const count = suits.length;
  const spacing = usableWidth / (count + 1);
  for (let i = 0; i < suits.length; i++) {
    const suit = suits[i];
    const spr = horseSprites[suit];
    if (!spr) continue;
    spr.width = CARD_WIDTH; spr.height = CARD_HEIGHT;
    spr.anchor.set(0, 0);
    const centerX = leftPadding + spacing * (i + 1);
    const targetX = Math.round(centerX - CARD_WIDTH / 2);
    spr.x = targetX;
    const pos = Math.max(0, Math.min(POSITIONS_COUNT - 1, horsePositions[suit] ?? 0));
    const targetTopY = positionYs[pos] ?? positionYs[0] ?? 0;
    spr.y = targetTopY;
  }
}

function createPlaceholderCard(suit?: number) {
  const g = new PIXI.Graphics();
  g.beginFill(0xffffff);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 6);
  g.endFill();
  if (suit) {
    const cols: Record<number, number> = {1:0xffff00,2:0xff0000,3:0x0000ff,4:0x00cc00};
    g.beginFill(cols[suit] ?? 0x666666);
    g.drawCircle(CARD_WIDTH/2, CARD_HEIGHT/2, Math.min(CARD_WIDTH, CARD_HEIGHT) * 0.18);
    g.endFill();
  }
  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false });
  return new PIXI.Sprite(tex);
}

// ---------------- Events & animations ----------------
async function processEventsSequentially() {
  stopProcessing = false;
  for (let i = 0; i < (events || []).length; i++) {
    if (stopProcessing) break;
    const ev = events[i];
    try { await handleEvent(ev); } catch (err) { console.warn('handleEvent error', err); }
    await sleep(EVENT_DELAY);
  }
}

async function handleEvent(ev: any) {
  switch (ev.EventType) {
    case 5: await onGetCardFromDeck(ev); break;
    case 3: await onObstacleRevealed(ev); break;
    case 6: await onUpdateHorsePosition(ev); break;
    default: break;
  }
}

async function onGetCardFromDeck(ev: any) {
  if (deckSprites.length === 0) return;
  const top = deckSprites.pop()!;
  const cardData = (ev && ev.CardSuit !== undefined) ? ev : (top as any).__card;
  const gp = top.getGlobalPosition();
  const fly = new PIXI.Sprite(top.texture);
  (fly as any).__card = cardData;
  fly.x = gp.x; fly.y = gp.y;
  app.stage.addChild(fly);
  safeRemoveAndDestroy(top);
  if (currentCardSprite) {
    const discardGlobal = getDiscardGlobalPos();
    await flipAndMoveToDiscardAsync(currentCardSprite, discardGlobal.x, discardGlobal.y);
    discardSprites.push(currentCardSprite);
    currentCardSprite = null;
  }
  const [ax, ay] = getActiveGlobalPosArray();
  await animateMoveToAsync(fly, ax, ay);
  await flipSpriteToFaceAsync(fly, cardData);
  const local = trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x; fly.y = local.y;
  trackContainer.addChild(fly);
  currentCardSprite = fly;
}

async function onObstacleRevealed(ev: any) {
  const card = { CardSuit: ev.CardSuit, CardRank: ev.CardRank, CardOrder: ev.CardOrder };
  let s: PIXI.Sprite;
  if (card.CardSuit !== undefined) {
    s = new PIXI.Sprite(getCardTextureFor(card.CardSuit, card.CardRank));
    s.width = CARD_WIDTH; s.height = CARD_HEIGHT;
  } else s = createPlaceholderCard(card.CardSuit);
  trackContainer.addChild(s);
  let idx = 0;
  if (typeof ev.CardOrder === 'number' && ev.CardOrder >= 1) idx = Math.min(BARRIER_COUNT - 1, ev.CardOrder - 1);
  const posIndex = idx + 1;
  recomputePositionYs();
  s.x = 12; s.y = positionYs[posIndex];
  barrierSprites[idx] = s;
}

async function onUpdateHorsePosition(ev: any) {
  const suit = ev.HorseSuit;
  const pos = Math.max(0, Math.min(POSITIONS_COUNT - 1, ev.Position ?? 0));
  await animateHorseToPosition(suit, pos);
}

async function flipAndMoveToDiscardAsync(sprite: PIXI.Sprite, tx: number, ty: number) {
  await flipSpriteToBackAsync(sprite);
  const gp = sprite.getGlobalPosition();
  const fly = new PIXI.Sprite(sprite.texture);
  (fly as any).__card = (sprite as any).__card;
  fly.x = gp.x; fly.y = gp.y;
  app.stage.addChild(fly);
  safeRemoveAndDestroy(sprite);
  await animateMoveToAsync(fly, tx, ty);
  const local = trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x; fly.y = local.y;
  trackContainer.addChild(fly);
}

function flipSpriteToFaceAsync(sprite: PIXI.Sprite, cardData: any): Promise<void> {
  return new Promise(resolve => {
    let s = Math.abs(sprite.scale.x) || 1;
    let phase: 'in'|'out' = 'in';
    const cb = () => {
      if (phase === 'in') {
        s -= 0.18; sprite.scale.x = s;
        if (s <= 0) {
          if (cardData?.CardSuit !== undefined) {;
            sprite.texture = getCardTextureFor(cardData.CardSuit, cardData.CardRank);
          } else {
            const tmp = createPlaceholderCard(cardData?.CardSuit);
            sprite.texture = tmp.texture;
            safeRemoveAndDestroy(tmp);
          }
          (sprite as any).__card = cardData;
          phase = 'out';
        }
      } else {
        s += 0.18; sprite.scale.x = s;
        if (s >= 1) { app.ticker.remove(cb); resolve(); }
      }
    };
    app.ticker.add(cb);
  });
}

function flipSpriteToBackAsync(sprite: PIXI.Sprite): Promise<void> {
  return new Promise(resolve => {
    let s = Math.abs(sprite.scale.x) || 1;
    let phase: 'in'|'out' = 'in';
    const cb = () => {
      if (phase === 'in') {
        s -= 0.18; sprite.scale.x = s;
        if (s <= 0) {
          sprite.texture = getBackTexture();
          phase = 'out';
        }
      } else {
        s += 0.18; sprite.scale.x = s;
        if (s >= 1) { app.ticker.remove(cb); resolve(); }
      }
    };
    app.ticker.add(cb);
  });
}

function animateMoveToAsync(sprite: PIXI.Sprite, tx: number, ty: number): Promise<void> {
  return new Promise(resolve => {
    const cb = () => {
      const dx = tx - sprite.x;
      const dy = ty - sprite.y;
      sprite.x += dx * 0.22;
      sprite.y += dy * 0.22;
      if (Math.abs(dx) < MOVE_EPS && Math.abs(dy) < MOVE_EPS) {
        sprite.x = tx; sprite.y = ty;
        app.ticker.remove(cb);
        resolve();
      }
    };
    app.ticker.add(cb);
  });
}

function animateHorseToPosition(suit: number, position: number): Promise<void> {
  return new Promise(resolve => {
    recomputePositionYs();
    const horse = horseSprites[suit];
    if (!horse) { resolve(); return; }
    const cur = horsePositions[suit] ?? 0;
    if (cur === position) { resolve(); return; }
    const targetTopY = positionYs[Math.min(Math.max(0, position), POSITIONS_COUNT - 1)];
    if (Math.abs(horse.y - targetTopY) < MOVE_EPS) { horsePositions[suit] = position; horse.y = targetTopY; resolve(); return; }
    const cb = () => {
      const dy = targetTopY - horse.y;
      horse.y += dy * 0.22;
      if (Math.abs(dy) < MOVE_EPS) {
        horse.y = targetTopY;
        app.ticker.remove(cb);
        horsePositions[suit] = position;
        resolve();
      }
    };
    app.ticker.add(cb);
  });
}

// ---------------- Helpers ----------------
function getActiveGlobalPosArray(): [number, number] {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  return [trackContainer.x + deckX, trackContainer.y + deckY + CARD_HEIGHT + 14];
}
function getDiscardGlobalPos(): { x:number, y:number } {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  return { x: trackContainer.x + deckX + CARD_WIDTH + 12, y: trackContainer.y + deckY };
}

// ---------------- Init / mount / resize ----------------
onMounted(async () => {
  app = new PIXI.Application();
  await app.init({ resizeTo: pixiContainer.value!, backgroundColor: 0x1d3557, antialias: true });
  pixiContainer.value!.appendChild(app.canvas);

  trackContainer = new PIXI.Container();
  trackContainer.x = TRACK_SIDE_MARGIN;
  trackContainer.y = 20;
  app.stage.addChild(trackContainer);

  try { await loadCardAtlasAndCache(); } catch (e) { console.warn('atlas load failed', e); }

  try {
    const res = await fetch('/mock/game.json');
    const parsed = await res.json() as PlayGameResponse;
    gameData = parsed.Data ?? parsed;
  } catch (e) {
    console.warn('mock not found', e);
    gameData = { InitialDeck: [], HorseBets: [], Events: [] };
  }

  clearScene();
  recomputePositionYs();
  drawPositionLines();
  drawBarriers(gameData.InitialDeck || []);
  placeBarriersOnPositions();
  drawDeck(gameData.InitialDeck || []);
  drawHorses(gameData.HorseBets || []);
  placeHorsesAtPositions();

  events = gameData.Events ?? [];
  processEventsSequentially();

  window.addEventListener('resize', onResize);
});

onBeforeUnmount(() => {
  stopProcessing = true;
  window.removeEventListener('resize', onResize);
  // destroy scene, but avoid destroying shared textures by default
  try {
    clearScene();
    app.destroy(true);
  } catch {}
});

function onResize() {
  if (!pixiContainer.value) return;
  try { app.renderer.resize(pixiContainer.value.clientWidth, pixiContainer.value.clientHeight); } catch {}
  recomputePositionYs();
  drawPositionLines();
  placeBarriersOnPositions();
  placeHorsesAtPositions();
  // rebuild deck visuals preserving remaining card data
  const remaining = deckSprites.map(s => (s as any).__card).filter(Boolean);
  deckSprites.forEach(s => safeRemoveAndDestroy(s));
  deckSprites = [];
  drawDeckFromData(remaining);
}

function drawDeckFromData(deckCards: any[]) {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  for (let i = 0; i < deckCards.length; i++) {
    const card = deckCards[i];
    const tex =  getBackTexture();
    const back = new PIXI.Sprite(tex);
    back.width = CARD_WIDTH; back.height = CARD_HEIGHT;
    back.x = deckX; back.y = deckY + i * 0.35;
    (back as any).__card = card;
    trackContainer.addChild(back);
    deckSprites.push(back);
  }
}
</script>

<style scoped>
.race-container {
    width: 100%;
    height: calc(100vh - 200px);
    overflow: hidden;
    position: relative;
}
.pixi-stage { width: 100%; height: 100%; display: block; }
</style>
