<template>
  <div class="race-container">
    <div ref="pixiContainer" class="pixi-stage"></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref } from 'vue';
import * as PIXI from 'pixi.js';

// ---------------- CONFIG ----------------
const CARD_WIDTH = 64;       // уменьшенный размер карты/лошади
const CARD_HEIGHT = 88;
const BARRIER_GAP = 4;      // расстояние между картами-барьерами
const DECK_SHIFT_LEFT = 22; // сдвиг стопки внутрь трассы
const POSITIONS_COUNT = 8;  // позиции 0..7 (0 start, 7 finish)
const BARRIER_COUNT = 6;    // 1..6 — барьеры
const FINISH_OFFSET = CARD_HEIGHT + 8; // сверху отступ = 1 карта
const BOTTOM_PADDING = 12;  // снизу отступ чтобы лошади полностью видны
const TRACK_SIDE_MARGIN = 20; // отступ слева от края для трека
const EVENT_DELAY = 200;    // пауза между событиями
const MOVE_EPS = 0.6;       // порог завершения анимации
const STEP_SCALE = 0.7; // <1 — уменьшаем расстояние между позициями; >1 — увеличиваем
const EXTRA_BOTTOM_SPACE = 100; // <-- увеличь, если нужно больше воздуха внизу
const RIGHT_PANEL_WIDTH = 100;
// ----------------------------------------

const pixiContainer = ref<HTMLDivElement | null>(null);
let app: PIXI.Application;
let trackContainer: PIXI.Container;

// Sprites and state
let deckSprites: PIXI.Sprite[] = [];
let discardSprites: PIXI.Sprite[] = [];
let currentCardSprite: PIXI.Sprite | null = null;

const barrierSprites: PIXI.Sprite[] = [];
const horseSprites: Record<number, PIXI.Sprite> = {};
const horsePositions: Record<number, number> = {}; // logical positions 0..7

let positionYs: number[] = []; // Y (top) for sprite at each logical position (local to trackContainer)

// Game data & events
let gameData: any = null;
let events: any[] = [];

// small util
const sleep = (ms: number) => new Promise(r => setTimeout(r, ms));

// ---------------- PIXI init & lifecycle ----------------
onMounted(async () => {
  app = new PIXI.Application();
  await app.init({
    width: window.innerWidth,
    height: window.innerHeight,
    backgroundColor: 0x1d3557,
    antialias: true,
  });

  if (!pixiContainer.value) throw new Error('pixiContainer ref is null');
  pixiContainer.value.appendChild(app.canvas);

  trackContainer = new PIXI.Container();
  // offset trackContainer slightly from left/top
  trackContainer.x = TRACK_SIDE_MARGIN;
  trackContainer.y = 20;
  app.stage.addChild(trackContainer);

  // load mock (or replace with SignalR)
  try {
    const res = await fetch('/mock/game.json');
    const parsed = await res.json();
    gameData = parsed.Data;
  } catch (e) {
    console.warn('failed to load mock game.json', e);
    gameData = { InitialDeck: [], HorseBets: [], Events: [] };
  }

  buildInitialScene(gameData);
  events = gameData.Events ?? [];

  // start sequential processing of events
  processEventsSequentially();

  window.addEventListener('resize', onResize);
});

onBeforeUnmount(() => {
  window.removeEventListener('resize', onResize);
  try { app.destroy(); } catch {}
});

// ---------------- Layout & geometry ----------------
function computeTrackMetrics() {
  const trackTop = FINISH_OFFSET;
  const trackBottom = app.screen.height - trackContainer.y - BOTTOM_PADDING - EXTRA_BOTTOM_SPACE;
  const trackHeight = Math.max(180, trackBottom - trackTop);
  const trackWidth = Math.max(300, app.screen.width - trackContainer.x - TRACK_SIDE_MARGIN - (RIGHT_PANEL_WIDTH + 24));
  return { trackTop, trackBottom, trackHeight, trackWidth };
}

function recomputePositionYs() {
  // positions 0..7, position 0 is bottom (start), 7 is near top (finish)
  const { trackTop, trackBottom } = computeTrackMetrics();
  const intervals = POSITIONS_COUNT - 1; // 7 intervals
  const step = ((trackBottom - trackTop) / intervals) * STEP_SCALE;
  positionYs = [];
  for (let p = 0; p < POSITIONS_COUNT; p++) {
    const lineY = trackBottom - p * step; // reference line for position p
    const spriteTopY = lineY - CARD_HEIGHT; // top coordinate of sprite so its top aligns with lineY - CARD_HEIGHT
    positionYs.push(spriteTopY);
  }
}

// ---------------- Build initial scene ----------------
function buildInitialScene(data: any) {
  // clear previous content
  trackContainer.removeChildren();
  deckSprites = [];
  discardSprites = [];
  currentCardSprite = null;
  barrierSprites.length = 0;
  Object.keys(horseSprites).forEach(k => {
    try { horseSprites[+k].destroy({ children: true, texture: false }); } catch {}
    delete horseSprites[+k];
  });
  for (const k in horsePositions) delete horsePositions[+k];
  positionYs = [];

  recomputePositionYs();
  drawPositionLines(); // dashed lines and labels
  drawBarriersFromDeck(data.InitialDeck || []);
  drawDeckFromData(data.InitialDeck || []);
  drawHorses(data.HorseBets || []);
  placeBarriersOnPositions();
  placeHorsesAtPositions();
}

// draw dashed lines and labels for positions 0..7
function drawPositionLines() {
  // remove old lines (keep other children like sprites)
  // find any Graphics with name 'posLines' and remove
  for (let i = trackContainer.children.length - 1; i >= 0; i--) {
    const ch = trackContainer.children[i];
    if ((ch as any).__posLines) {
      trackContainer.removeChild(ch);
      try { ch.destroy({ children: true, texture: false }); } catch {}
    }
  }

  recomputePositionYs();
  const g = new PIXI.Graphics();
  (g as any).__posLines = true;
  const { trackWidth } = computeTrackMetrics();
  const xStart = 0;
  const xEnd = trackWidth;
  const labelStyle = new PIXI.TextStyle({ fill: 0xffffff, fontSize: 12 });

  for (let p = 0; p < positionYs.length; p++) {
    const topY = positionYs[p];
    const lineY = topY + CARD_HEIGHT; // reference baseline
    drawDashedLine(g, xStart, lineY, xEnd, lineY, 8, 6);
    const label = new PIXI.Text(String(p), labelStyle);
    label.x = -28;
    label.y = lineY - label.height / 2;
    (label as any).__posLines = true;
    trackContainer.addChild(label);
  }
  trackContainer.addChild(g);
}

// dashed line drawer
function drawDashedLine(g: PIXI.Graphics, x1: number, y1: number, x2: number, y2: number, dash = 6, gap = 4) {
  g.lineStyle(2, 0xffffff, 0.12);
  const dx = x2 - x1;
  const dy = y2 - y1;
  const dist = Math.hypot(dx, dy);
  const stepTotal = dash + gap;
  const segments = Math.floor(dist / stepTotal);
  for (let i = 0; i < segments; i++) {
    const start = i * stepTotal;
    const sx = x1 + (dx * start) / dist;
    const sy = y1 + (dy * start) / dist;
    const ex = x1 + (dx * (start + dash)) / dist;
    const ey = y1 + (dy * (start + dash)) / dist;
    g.moveTo(sx, sy);
    g.lineTo(ex, ey);
  }
}

// ---------------- Barriers (draw from initial deck Zone==1) ----------------
function drawBarriersFromDeck(deck: any[]) {
  const barriers = deck.filter((c: any) => c.Zone === 1).slice(0, BARRIER_COUNT);
  for (let i = 0; i < barriers.length; i++) {
    const card = barriers[i];
    const s = createCardFront(card);
    s.name = `barrier-${i+1}`;
    trackContainer.addChild(s);
    barrierSprites.push(s);
  }
}

function placeBarriersOnPositions() {
  recomputePositionYs();
  // barriers map to positions 1..6
  for (let i = 0; i < barrierSprites.length; i++) {
    const posIndex = 1 + i; // position 1..6
    const topY = positionYs[posIndex];
    const x = 12; // left padding inside trackContainer
    barrierSprites[i].x = x;
    barrierSprites[i].y = topY;
  }
}

// ---------------- Deck, active, discard inside trackContainer ----------------
function drawDeckFromData(deck: any[]) {
  // compute deck stack X (right side inside track)
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  const deckCards = (deck || []).filter((c: any) => c.Zone === 0);
  deckSprites = [];
  for (let i = 0; i < deckCards.length; i++) {
    const card = deckCards[i];
    const back = createCardBack();
    back.x = deckX;
    back.y = deckY + i * 0.35;
    (back as any).__card = card;
    trackContainer.addChild(back);
    deckSprites.push(back);
  }

  // discard placeholder (to the right of deck)
  const discardX = deckX + CARD_WIDTH + 12;
  const discardG = new PIXI.Graphics();
  discardG.lineStyle(2, 0x444444, 0.6);
  discardG.drawRoundedRect(discardX, deckY, CARD_WIDTH, CARD_HEIGHT, 8);
  trackContainer.addChild(discardG);

  // active slot (below deck)
  const activeX = deckX;
  const activeY = deckY + CARD_HEIGHT + 14;
  const activeG = new PIXI.Graphics();
  activeG.lineStyle(2, 0x888888, 0.6);
  activeG.drawRoundedRect(activeX, activeY, CARD_WIDTH, CARD_HEIGHT, 8);
  trackContainer.addChild(activeG);
}

// get global coords for active/discard (for move animations)
function getActiveGlobalPosArray(): [number, number] {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  const ax = trackContainer.x + deckX;
  const ay = trackContainer.y + deckY + CARD_HEIGHT + 14;
  return [ax, ay];
}
function getDiscardGlobalPos(): { x: number; y: number } {
  const { trackWidth } = computeTrackMetrics();
  const deckX = trackWidth - CARD_WIDTH - DECK_SHIFT_LEFT;
  const deckY = 6;
  const dx = trackContainer.x + deckX + CARD_WIDTH + 12;
  const dy = trackContainer.y + deckY;
  return { x: dx, y: dy };
}

// ---------------- Horses (same size as card), in a row ----------------
function drawHorses(horseBets: any[]) {
  recomputePositionYs();
  const bets = horseBets ?? [];
  const count = Math.max(1, bets.length);
  const { trackWidth } = computeTrackMetrics();
  const spacing = Math.max(1, trackWidth / (count + 1));

  for (let i = 0; i < bets.length; i++) {
    const bet = bets[i];
    const suit = bet.BetSuit ?? (i + 1);
    const horse = createHorseSprite(suit);
    horse.anchor.set(0, 0);
    const x = Math.round(spacing * (i + 1) - CARD_WIDTH / 2);
    horse.x = x;
    horse.y = positionYs[0]; // position 0 by default
    trackContainer.addChild(horse);
    horseSprites[suit] = horse;
    horsePositions[suit] = 0;
  }
}

function placeHorsesAtPositions() {
  recomputePositionYs();
  const bets = Object.keys(horseSprites).map(k => +k);
  const { trackWidth } = computeTrackMetrics();
  const count = Math.max(1, bets.length);
  const spacing = Math.max(1, trackWidth / (count + 1));
  for (let i = 0; i < bets.length; i++) {
    const suit = bets[i];
    const spr = horseSprites[suit];
    const x = Math.round(spacing * (i + 1) - CARD_WIDTH / 2);
    spr.x = x;
    const pos = horsePositions[suit] ?? 0;
    spr.y = positionYs[Math.min(Math.max(0, pos), POSITIONS_COUNT - 1)];
  }
}

// ---------------- Events: sequential processing ----------------
let stopProcessingFlag = false;
async function processEventsSequentially() {
  stopProcessingFlag = false;
  for (let i = 0; i < events.length; i++) {
    if (stopProcessingFlag) break;
    const ev = events[i];
    try {
      await handleEvent(ev);
    } catch (e) {
      console.warn('handleEvent error', e);
    }
    await sleep(EVENT_DELAY);
  }
}

// Map server EventType numbers to handlers (we only use the types you provided)
async function handleEvent(ev: any) {
  switch (ev.EventType) {
    case 5: // GetCardFromDeck
      await handleGetCardFromDeck(ev);
      break;
    case 3: // ObstacleCardRevealed
      await handleObstacleRevealed(ev);
      break;
    case 6: // UpdateHorsePosition
      await handleUpdateHorsePosition(ev);
      break;
    default:
      // other event types are ignored for now
      break;
  }
}

// ---------------- Card flow: deck -> active -> discard ----------------
async function handleGetCardFromDeck(ev: any) {
  if (deckSprites.length === 0) return;
  const top = deckSprites.pop()!;
  const cardData = (ev && (ev.CardSuit !== undefined)) ? ev : (top as any).__card;
  const gp = top.getGlobalPosition();
  const fly = new PIXI.Sprite(top.texture);
  (fly as any).__card = cardData;
  fly.x = gp.x;
  fly.y = gp.y;
  app.stage.addChild(fly);
  try { top.parent?.removeChild(top); top.destroy({ children: true, texture: false }); } catch {}

  if (currentCardSprite) {
    const discardPos = getDiscardGlobalPos();
    await flipAndMoveToDiscardAsync(currentCardSprite, discardPos.x, discardPos.y);
    discardSprites.push(currentCardSprite);
    currentCardSprite = null;
  }

  const [ax, ay] = getActiveGlobalPosArray();
  await animateMoveToAsync(fly, ax, ay);
  await flipSpriteToFaceAsync(fly, cardData);
  // attach to trackContainer local coords (convert global to local)
  const local = trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x;
  fly.y = local.y;
  trackContainer.addChild(fly);
  currentCardSprite = fly;
}

async function handleObstacleRevealed(ev: any) {
  // create card and place on the barrier position indicated (or next free)
  const card = { CardSuit: ev.CardSuit, CardRank: ev.CardRank, CardOrder: ev.CardOrder };
  const s = createCardFront(card);
  trackContainer.addChild(s);
  // find index: prefer CardOrder -> index (cardOrder 1 => barrier index 0 -> position1)
  let idx = 0;
  if (typeof ev.CardOrder === 'number' && ev.CardOrder >= 1) idx = Math.min(BARRIER_COUNT - 1, ev.CardOrder - 1);
  // place at position idx+1
  recomputePositionYs();
  const posIndex = idx + 1;
  s.x = 12;
  s.y = positionYs[posIndex];
  barrierSprites[idx] = s;
}

async function handleUpdateHorsePosition(ev: any) {
  const suit = ev.HorseSuit;
  const pos = Math.max(0, Math.min(POSITIONS_COUNT - 1, ev.Position ?? 0));
  await animateHorseToPosition(suit, pos);
}

// flip existing card to back and move to discard area
async function flipAndMoveToDiscardAsync(sprite: PIXI.Sprite, tx: number, ty: number) {
  await flipSpriteToBackAsync(sprite);
  const gp = sprite.getGlobalPosition();
  const fly = new PIXI.Sprite(sprite.texture);
  (fly as any).__card = (sprite as any).__card;
  fly.x = gp.x; fly.y = gp.y;
  app.stage.addChild(fly);
  try { sprite.parent?.removeChild(sprite); sprite.destroy({ children: true, texture: false }); } catch {}
  await animateMoveToAsync(fly, tx, ty);
  const local = trackContainer.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x; fly.y = local.y;
  trackContainer.addChild(fly);
}

// ---------------- flip & move helpers ----------------
function flipSpriteToFaceAsync(sprite: PIXI.Sprite, cardData: any): Promise<void> {
  return new Promise(resolve => {
    let s = Math.abs(sprite.scale.x) || 1;
    let phase: 'in'|'out' = 'in';
    const cb = () => {
      if (phase === 'in') {
        s -= 0.18; sprite.scale.x = s;
        if (s <= 0) {
          // set face texture - reuse createCardFront to produce texture, then apply texture
          const face = createCardFront(cardData);
          sprite.texture = face.texture;
          (sprite as any).__card = cardData;
          face.destroy({ children: true, texture: false });
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
          const back = createCardBack();
          sprite.texture = back.texture;
          back.destroy({ children: true, texture: false });
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

// ---------------- Horse movement (local to trackContainer) ----------------
function animateHorseToPosition(suit: number, position: number): Promise<void> {
  return new Promise(resolve => {
    recomputePositionYs();
    const horse = horseSprites[suit];
    if (!horse) { resolve(); return; }
    const cur = horsePositions[suit] ?? 0;
    if (cur === position) { resolve(); return; }

    const targetTopY = positionYs[Math.min(Math.max(0, position), POSITIONS_COUNT - 1)];
    // avoid micro moves
    if (Math.abs(horse.y - targetTopY) < MOVE_EPS) {
      horsePositions[suit] = position;
      horse.y = targetTopY;
      resolve(); return;
    }

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

// ---------------- Sprite creators (single unified implementations) ----------------
function createCardBack(): PIXI.Sprite {
  const g = new PIXI.Graphics();
  g.beginFill(0x333399);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  g.endFill();
  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false });
  return new PIXI.Sprite(tex);
}

function createCardFront(card: any): PIXI.Sprite {
  const g = new PIXI.Graphics();
  g.beginFill(0xffffff);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  g.endFill();

  const suitColors: Record<number, number> = { 1: 0xffff00, 2: 0xff0000, 3: 0x0000ff, 4: 0x00cc00 };
  const color = suitColors[(card?.CardSuit as number) ?? 1] || 0x000000;
  g.beginFill(color);
  g.drawCircle(CARD_WIDTH / 2, CARD_HEIGHT / 2, Math.min(CARD_WIDTH, CARD_HEIGHT) * 0.18);
  g.endFill();

  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false });
  const s = new PIXI.Sprite(tex);
  (s as any).__card = card;
  return s;
}

function createHorseSprite(suit: number): PIXI.Sprite {
  const colors: Record<number, number> = { 1: 0xffff00, 2: 0xff0000, 3: 0x0000ff, 4: 0x00cc00 };
  const g = new PIXI.Graphics();
  g.beginFill(colors[suit] ?? 0xffffff);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  g.endFill();
  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false });
  return new PIXI.Sprite(tex);
}

// ---------------- Resize ----------------
function onResize() {
  try { app.renderer.resize(window.innerWidth, window.innerHeight); } catch {}
  recomputePositionYs();
  drawPositionLines();
  placeBarriersOnPositions();
  placeHorsesAtPositions();

  // reposition deck and placeholders
  // (clear deck visuals and redraw keeping remaining deckSprites textures)
  // First convert existing deckSprites to textures + card metadata, then destroy, then re-create at new positions
  const remainingCards = deckSprites.map(s => (s as any).__card).filter(Boolean);
  // destroy old deck visuals
  deckSprites.forEach(s => { try { s.parent?.removeChild(s); s.destroy({ children: true, texture: false }); } catch {} });
  deckSprites = [];
  // redraw deck
  drawDeckFromData(remainingCards);
}

</script>

<style scoped>
.race-container {
  height: calc(100vh - 40px); /* уменьшили высоту игрового поля */
  overflow: hidden;
  position: relative;
}
.pixi-stage { width: 100%; height: 100%; display: block; }

/* .race-container {
  width: 100%;
  height: 100vh;
  overflow: hidden;
  background: #1d3557;
}
.pixi-stage {
  width: 100%;
  height: 100%;
} */
</style>
