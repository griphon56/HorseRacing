<template>
  <div class="race-container">
    <div ref="pixiContainer" class="pixi-stage"></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref } from 'vue';
import * as PIXI from 'pixi.js';
import { useRoute } from 'vue-router';
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response';
import { SuitType } from '~/interfaces/api/contracts/model/game/enums/suit-type-enum';

// ----------------------------
// CONFIG
const CARD_WIDTH = 72;
const CARD_HEIGHT = 96;
const TRACK_LENGTH = 8; // логических шагов
const EVENT_DELAY_BETWEEN = 350; // ms задержка между событиями (доп. пауза)
const RIGHT_PANEL_WIDTH = 200;
const MARGIN = 20;
const TOP_EXTRA_PADDING = 60; // верхний отступ чтобы видно было прохождение последнего барьера
const BOTTOM_PADDING = 12; // отступ снизу чтобы лошади были полностью видны
// ----------------------------

// PIXI / сцена
const pixiContainer = ref<HTMLDivElement | null>(null);
let app: PIXI.Application;
let leftPanel: PIXI.Container;
let trackContainer: PIXI.Container;
let rightPanel: PIXI.Container;

const barrierSprites: PIXI.Sprite[] = [];
const deckSprites: PIXI.Sprite[] = [];
const discardSprites: PIXI.Sprite[] = [];
let currentCardSprite: PIXI.Sprite | null = null;

const horseSprites: Record<number, PIXI.Sprite> = {};

// координаты барьеров в системе trackContainer
let barrierYsInTrack: number[] = [];

// Игровые данные / события
const route = useRoute();
let gameData: any = null;
let events: any[] = [];
let processing = false;
let stopProcessing = false;

const sleep = (ms: number) => new Promise(r => setTimeout(r, ms));

// ----------------------------
// Инициализация
onMounted(async () => {
  try {
    app = new PIXI.Application();
    await app.init({
      width: window.innerWidth,
      height: window.innerHeight,
      backgroundColor: 0x1d3557,
      antialias: true,
    });
    if (!pixiContainer.value) throw new Error('pixiContainer ref is null');
    pixiContainer.value.appendChild(app.canvas);

    // панели
    leftPanel = new PIXI.Container();
    leftPanel.x = MARGIN;
    leftPanel.y = MARGIN;
    app.stage.addChild(leftPanel);

    trackContainer = new PIXI.Container();
    trackContainer.x = 140;
    trackContainer.y = 80; // верхняя граница трассы
    app.stage.addChild(trackContainer);

    rightPanel = new PIXI.Container();
    rightPanel.x = app.screen.width - RIGHT_PANEL_WIDTH - MARGIN;
    rightPanel.y = MARGIN;
    app.stage.addChild(rightPanel);

    // load mock (замени на SignalR при необходимости)
    const res = await fetch('/mock/game.json');
    const parsed = (await res.json()) as PlayGameResponse;
    gameData = parsed.Data;

    drawInitial(gameData);

    events = gameData.Events ?? [];
    processEventsSequentially();
  } catch (e) {
    console.error('init error', e);
  }
});

onBeforeUnmount(() => {
  stopProcessing = true;
  try { app?.destroy?.(); } catch {}
});

// ----------------------------
// Нарисовать начальное состояние
function drawInitial(data: any) {
  // очистка
  leftPanel.removeChildren();
  trackContainer.removeChildren();
  rightPanel.removeChildren();
  barrierSprites.length = 0;
  deckSprites.length = 0;
  discardSprites.length = [];
  currentCardSprite = null;
  barrierYsInTrack = [];

  // Left: barriers (Zone=1)
  const barriers = (data.InitialDeck || []).filter((c: any) => c.Zone === 1);
  barriers.forEach((card: any, i: number) => {
    const s = createCardFront(card);
    s.x = 0;
    s.y = i * (CARD_HEIGHT + 8);
    leftPanel.addChild(s);
    barrierSprites.push(s);
  });

  // Right: deck (Zone=0)
  const deck = (data.InitialDeck || []).filter((c: any) => c.Zone === 0);
  deck.forEach((card: any, i: number) => {
    const back = createCardBack();
    back.x = 0;
    back.y = i * 0.35;
    (back as any).__card = card;
    rightPanel.addChild(back);
    deckSprites.push(back);
  });

  // right placeholders
  drawRightPlaceholders();

  // compute barrier Y positions in trackContainer coordinates
  // we want barrierYsInTrack[] such that index corresponds to visual vertical Y inside trackContainer
  barrierYsInTrack = barrierSprites.map(b => {
    const g = b.getGlobalPosition();
    // convert global to trackContainer local
    const local = trackContainer.toLocal(new PIXI.Point(g.x, g.y), app.stage);
    return local.y;
  });

  // Horses: one horizontal row, bottom of track area; horse size = card size
  const bets = data.HorseBets || [];

  // track local area размеры (внутри trackContainer)
    const trackLocalTop = 10;
    const trackLocalBottom = app.screen.height - trackContainer.y - MARGIN - TOP_EXTRA_PADDING;

    // Количество барьеров
    const nBarriers = barrierSprites.length;

    // Вычисляем Y-координаты барьеров в координатах trackContainer,
    // причём позиция 1 будет ближайшей к стартовой линии (внизу),
    // а более высокие индексы — выше по трассе.
    barrierYsInTrack = [];
    if (nBarriers > 0) {
    // делим пространство между top и bottom на (nBarriers + 1) промежутков,
    // чтобы барьеры равномерно располагались по трассе
    const step = (trackLocalBottom - trackLocalTop) / (nBarriers + 1);
    for (let i = 0; i < nBarriers; i++) {
        // индекс 0 -> ближайший к низу: trackLocalBottom - step*(i+1)
        const y = trackLocalBottom - step * (i + 1);
        // хотим, чтобы лошадь (высотой CARD_HEIGHT) выравнивалась так, чтобы её верх совпадал с барьером
        // (или измените смещение, если нужно центрировать/снизу выровнять)
        const alignTopY = y - (CARD_HEIGHT - 0); // подправьте 0 для смещения сверху/снизу
        barrierYsInTrack.push(alignTopY);
    }
}

// стартовая Y для лошадей — снизу так, чтобы лошадь полностью видна
const relativeStartY = trackLocalBottom - CARD_HEIGHT * 2;

// разместим лошадей на стартовой линии (внизу) в одну линию
const spacing = Math.max(1, (rightPanel.x - trackContainer.x - MARGIN) / (bets.length + 1));
for (let i = 0; i < bets.length; i++) {
  const bet = bets[i];
  const horse = createHorseSprite(bet.BetSuit);
  horse.x = Math.round(spacing * (i + 1) - CARD_WIDTH / 2);
  horse.y = relativeStartY;
  trackContainer.addChild(horse);
  horseSprites[bet.BetSuit] = horse;
}
}

// placeholder visuals on right panel
function drawRightPlaceholders() {
  const discardPlaceholder = new PIXI.Graphics();
  discardPlaceholder.beginFill(0x222222, 0.0);
  discardPlaceholder.lineStyle(2, 0x444444, 0.6);
  discardPlaceholder.drawRoundedRect(CARD_WIDTH + 18, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  discardPlaceholder.endFill();
  rightPanel.addChild(discardPlaceholder);

  const activePlaceholder = new PIXI.Graphics();
  activePlaceholder.beginFill(0x222222, 0.0);
  activePlaceholder.lineStyle(2, 0x888888, 0.6);
  activePlaceholder.drawRoundedRect(0, CARD_HEIGHT + 18, CARD_WIDTH, CARD_HEIGHT, 8);
  activePlaceholder.endFill();
  rightPanel.addChild(activePlaceholder);
}

// ----------------------------
// Обработка событий последовательно
async function processEventsSequentially() {
  if (processing) return;
  processing = true;
  for (let i = 0; i < events.length; i++) {
    if (stopProcessing) break;
    const ev = events[i];
    try {
      await handleEventAsync(ev);
      await sleep(EVENT_DELAY_BETWEEN);
    } catch (err) {
      console.warn('event handler error', err);
    }
  }
  processing = false;
}

function handleEventAsync(event: any): Promise<void> {
  switch (event.EventType) {
    case 5:
      return animateDrawToActiveAsync(event);
    case 6:
      return animateMoveHorseAsync(event.HorseSuit, event.Position);
    case 7:
      return Promise.resolve();
    default:
      return Promise.resolve();
  }
}

// ----------------------------
// Карточные анимации
async function animateDrawToActiveAsync(cardEvent: any): Promise<void> {
  await sleep(200);

  if (deckSprites.length === 0) return;

  const top = deckSprites.pop()!;
  const cardData = (cardEvent && (cardEvent.CardSuit !== undefined)) ? cardEvent : (top as any).__card;
  const globalDeckPos = top.getGlobalPosition();
  const fly = new PIXI.Sprite(top.texture);
  (fly as any).__card = cardData;
  fly.x = globalDeckPos.x; fly.y = globalDeckPos.y;
  app.stage.addChild(fly);
  try { top.parent?.removeChild(top); top.destroy({ children: true, texture: false, baseTexture: false }); } catch {}

  if (currentCardSprite) {
    const discardGlobal = getDiscardGlobalPos();
    await flipAndMoveToDiscardAsync(currentCardSprite, discardGlobal.x, discardGlobal.y);
    discardSprites.push(currentCardSprite);
    currentCardSprite = null;
  }

  const [activeX, activeY] = getActiveGlobalPosArray();
  await animateMoveToAsync(fly, activeX, activeY);
  await flipSpriteToFaceAsync(fly, cardData);

  const local = rightPanel.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x; fly.y = local.y;
  rightPanel.addChild(fly);
  currentCardSprite = fly;
  return;
}

function getActiveGlobalPosArray(): [number, number] {
  const localX = rightPanel.x + 0;
  const localY = rightPanel.y + CARD_HEIGHT + 18;
  return [localX, localY];
}

function getDiscardGlobalPos(): { x: number; y: number } {
  const localX = rightPanel.x + CARD_WIDTH + 18;
  const localY = rightPanel.y + 0;
  return { x: localX, y: localY };
}

async function flipAndMoveToDiscardAsync(sprite: PIXI.Sprite, targetX: number, targetY: number): Promise<void> {
  await flipSpriteToBackAsync(sprite);
  const gPos = sprite.getGlobalPosition();
  const fly = new PIXI.Sprite(sprite.texture);
  (fly as any).__card = (sprite as any).__card;
  fly.x = gPos.x; fly.y = gPos.y;
  app.stage.addChild(fly);
  try { sprite.parent?.removeChild(sprite); sprite.destroy({ children: true, texture: false, baseTexture: false }); } catch {}
  await animateMoveToAsync(fly, targetX, targetY);
  const local = rightPanel.toLocal(new PIXI.Point(fly.x, fly.y), app.stage);
  fly.x = local.x; fly.y = local.y;
  rightPanel.addChild(fly);
  return;
}

function flipSpriteToFaceAsync(sprite: PIXI.Sprite, cardData: any): Promise<void> {
  return new Promise(resolve => {
    let s = Math.abs(sprite.scale.x) || 1;
    let phase: 'in'|'out' = 'in';
    const cb = () => {
      if (phase === 'in') {
        s -= 0.18; sprite.scale.x = s;
        if (s <= 0) { sprite.texture = createCardTexture(cardData); (sprite as any).__card = cardData; phase = 'out'; }
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
        if (s <= 0) { sprite.texture = createCardBack().texture; phase = 'out'; }
      } else {
        s += 0.18; sprite.scale.x = s;
        if (s >= 1) { app.ticker.remove(cb); resolve(); }
      }
    };
    app.ticker.add(cb);
  });
}

function animateMoveToAsync(sprite: PIXI.Sprite, targetX: number, targetY: number): Promise<void> {
  return new Promise(resolve => {
    const cb = () => {
      const dx = targetX - sprite.x;
      const dy = targetY - sprite.y;
      sprite.x += dx * 0.22; sprite.y += dy * 0.22;
      if (Math.abs(dx) < 0.6 && Math.abs(dy) < 0.6) {
        sprite.x = targetX; sprite.y = targetY; app.ticker.remove(cb); resolve();
      }
    };
    app.ticker.add(cb);
  });
}

// ----------------------------
// Лошади: размер = размер карты, в ряд, движение снизу вверх
function animateMoveHorseAsync(horseSuit: number, position: number): Promise<void> {
  return new Promise(resolve => {
    const horse = horseSprites[horseSuit];
    if (!horse) { resolve(); return; }

    // если есть barrierYsInTrack и позиция попадает в диапазон — выравниваем по барьеру
    const nBarriers = barrierYsInTrack.length;
    let targetY: number | null = null;
    if (nBarriers > 0 && position >= 1 && position <= nBarriers) {
      // используем Y барьера в координатах trackContainer
      targetY = barrierYsInTrack[position - 1];
      // корректируем чтобы лошадь полностью совпадала с барьером по верхнему краю
      // если нужно, можно применить смещение (offset) тут
    } else {
      // fallback: равномерный шаг по высоте трассы
      const trackLocalTop = 10;
      const trackLocalBottom = app.screen.height - trackContainer.y - MARGIN - TOP_EXTRA_PADDING;
      const totalSteps = Math.max(1, TRACK_LENGTH);
      const stepPx = (trackLocalBottom - trackLocalTop) / totalSteps;
      const relativeStartY = trackLocalBottom - CARD_HEIGHT * 2;
      targetY = relativeStartY - position * stepPx;
    }

    // animate local move to targetY
    const cb = () => {
      const dy = (targetY! as number) - horse.y;
      horse.y += dy * 0.22;
      if (Math.abs(dy) < 0.6) {
        horse.y = targetY!;
        app.ticker.remove(cb);
        resolve();
      }
    };
    app.ticker.add(cb);
  });
}

// ----------------------------
// Создание спрайтов
function createCardBack(): PIXI.Sprite {
  const g = new PIXI.Graphics();
  g.beginFill(0x333399);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  g.endFill();
  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false, baseTexture: false });
  return new PIXI.Sprite(tex);
}

function createCardFront(card: any): PIXI.Sprite {
  const tex = createCardTexture(card);
  const s = new PIXI.Sprite(tex);
  (s as any).__card = card;
  return s;
}

function createCardTexture(card: any): PIXI.Texture {
  const g = new PIXI.Graphics();
  g.beginFill(0xffffff);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8);
  g.endFill();

  const suitColors: Record<number, number> = {
    [SuitType.Diamonds]: 0xffff00,
    [SuitType.Hearts]: 0xff0000,
    [SuitType.Spades]: 0x0000ff,
    [SuitType.Clubs]: 0x00ff00,
  };

  const color = suitColors[(card?.CardSuit as number) ?? 0] || 0x000000;
  g.beginFill(color);
  g.drawCircle(CARD_WIDTH / 2, CARD_HEIGHT / 2, Math.min(CARD_WIDTH, CARD_HEIGHT) * 0.18);
  g.endFill();

  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false, baseTexture: false });
  return tex;
}

function createHorseSprite(suit: number): PIXI.Sprite {
  const g = new PIXI.Graphics();
  const colors: Record<number, number> = {
    [SuitType.Diamonds]: 0xffff00,
    [SuitType.Hearts]: 0xff0000,
    [SuitType.Spades]: 0x0000ff,
    [SuitType.Clubs]: 0x00ff00,
  };
  g.beginFill(colors[suit] ?? 0xffffff);
  g.drawRoundedRect(0, 0, CARD_WIDTH, CARD_HEIGHT, 8); // horse == card size
  g.endFill();
  const tex = app.renderer.generateTexture(g);
  g.destroy({ children: true, texture: false, baseTexture: false });
  const s = new PIXI.Sprite(tex);
  s.anchor.set(0, 0);
  return s;
}
</script>

<style scoped>
.race-container {
  width: 100%;
  height: 100%;
  overflow: hidden;
  background: #1d3557;
}
.pixi-stage {
  width: 100%;
  height: 100%;
}
</style>
