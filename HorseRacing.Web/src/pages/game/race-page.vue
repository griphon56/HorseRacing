<template>
    <div class="viewport">
        <div class="phone-frame" ref="phoneFrame">
            <div ref="pixiContainer" class="pixi-stage"></div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { PixiRendererService } from '~/services/pixi-renderer-service';
import { defaultGameConfig } from '~/core/games/game-config';
import { loadCardAtlasAndCache, getCardTextureFor, getBackTexture } from '~/services/atlas-service';
import { loadMockGame } from '~/services/game-service';
import { signalRService } from '~/core/signalr/signalr-service';
import { useRoute } from 'vue-router';

const pixiContainer = ref<HTMLDivElement | null>(null);
const phoneFrame = ref<HTMLDivElement | null>(null);
let renderer: PixiRendererService | null = null;

const route = useRoute();

function normalizeGameId(rawId: any): string {
    if (!rawId) return '';
    if (typeof rawId === 'string') return rawId;
    if (typeof rawId === 'number') return String(rawId);
    // common shapes
    if (typeof rawId === 'object') {
        if ('Value' in rawId && rawId.Value != null) return String(rawId.Value);
        if ('value' in rawId && rawId.value != null) return String(rawId.value);
        if ('Id' in rawId && rawId.Id != null) return String(rawId.Id);
        if ('gameId' in rawId && typeof rawId.gameId === 'string') return rawId.gameId;
    }
    return String(rawId);
}

function mapCard(raw: any) {
    if (!raw) return null;
    return {
        CardSuit: raw.CardSuit ?? raw.cardSuit ?? null,
        CardRank: raw.CardRank ?? raw.cardRank ?? null,
        CardOrder: raw.CardOrder ?? raw.cardOrder ?? null,
        Zone: raw.Zone ?? raw.zone ?? null,
    };
}

function mapHorseBet(raw: any) {
    if (!raw) return null;
    return {
        BetSuit: raw.BetSuit ?? raw.betSuit ?? raw.CardSuit ?? null,
        BetAmount: raw.BetAmount ?? raw.betAmount ?? null,
        UserId: raw.UserId ?? raw.userId ?? null,
        FullName: raw.FullName ?? raw.fullName ?? null,
    };
}

function mapEvent(raw: any) {
    if (!raw) return null;
    return {
        Step: raw.Step ?? raw.step ?? null,
        EventType: raw.EventType ?? raw.eventType ?? null,
        CardSuit: raw.CardSuit ?? raw.cardSuit ?? null,
        CardRank: raw.CardRank ?? raw.cardRank ?? null,
        CardOrder: raw.CardOrder ?? raw.cardOrder ?? null,
        HorseSuit: raw.HorseSuit ?? raw.horseSuit ?? null,
        Position: raw.Position ?? raw.position ?? null,
        EventDate: raw.EventDate ?? raw.eventDate ?? null,
    };
}

/**
 * Возвращает объект { GameId, Name, InitialDeck[], HorseBets[], Events[] }
 */
function normalizeGamePayload(raw: any) {
    if (!raw) return { GameId: '', Name: '', InitialDeck: [], HorseBets: [], Events: [] };

    // raw может быть already Data or nested data: { Data: { ... } } или { data: { ... } }
    const payload = raw.Data ?? raw.data ?? raw;

    const gameId = normalizeGameId(
        payload.GameId ?? payload.gameId ?? payload.gameId ?? payload.id ?? payload.Id
    );
    const name = payload.Name ?? payload.name ?? '';

    // InitialDeck mapping (convert casing and field names)
    const initialDeckRaw = payload.InitialDeck ?? payload.initialDeck ?? payload.initialDeck ?? [];
    const initialDeck = Array.isArray(initialDeckRaw)
        ? initialDeckRaw.map(mapCard).filter(Boolean)
        : [];

    // HorseBets mapping
    const horseBetsRaw =
        payload.HorseBets ?? payload.horseBets ?? payload.HorseBets ?? payload.horseBets ?? [];
    const horseBets = Array.isArray(horseBetsRaw)
        ? horseBetsRaw.map(mapHorseBet).filter(Boolean)
        : [];

    // Events mapping
    const eventsRaw = payload.Events ?? payload.events ?? [];
    const events = Array.isArray(eventsRaw) ? eventsRaw.map(mapEvent).filter(Boolean) : [];

    return {
        GameId: gameId,
        Name: name,
        InitialDeck: initialDeck,
        HorseBets: horseBets,
        Events: events,
    };
}

onMounted(async () => {
    try {
        await loadCardAtlasAndCache();
    } catch (e) {
        console.warn(e);
    }

    let gameData: any = {
        InitialDeck: [],
        HorseBets: [],
        Events: [],
    };
    try {
        const resultGameSimulation = signalRService.onGameSimulationResult();
        await signalRService.registerReadyToStart(String(route.params.id));
        const playResult = await resultGameSimulation;
        console.log('Simulation result received', playResult);

        const raw = playResult?.Data ?? playResult;
        const normalized = normalizeGamePayload(raw);
        gameData = {
            InitialDeck: normalized.InitialDeck,
            HorseBets: normalized.HorseBets,
            Events: normalized.Events,
        };

        // const parsed = (await loadMockGame('/mock/game.json')) as PlayGameResponse;
        // gameData = parsed.Data ?? parsed;
    } catch (e) {
        console.warn(e);
    }

    if (!pixiContainer.value) return;

    const atlasProvider = { getCardTextureFor, getBackTexture };

    renderer = new PixiRendererService(
        pixiContainer.value,
        defaultGameConfig,
        atlasProvider,
        String(route.params.id)
    );
    await renderer.init();
    renderer.drawInitial(gameData);
    renderer.startEvents(gameData.Events ?? []);
});

onBeforeUnmount(() => {
    try {
        signalRService.offGameSimulationResult();
        renderer?.stopEvents();
        renderer?.destroy();
        renderer = null;
    } catch {}
});
</script>

<style scoped>
.viewport {
    width: 100vw;
    height: calc(100vh - 200px);
    display: flex;
    align-items: center; /* центр по вертикали */
    justify-content: center; /* центр по горизонтали */
    background: linear-gradient(180deg, #0b1b2b, #07121a); /* фон вокруг */
}

/* "телефон" фиксированного размера — дизайн-размер */
.phone-frame {
    width: 360px; /* DESIGN_WIDTH */
    height: 800px; /* DESIGN_HEIGHT */
    border-radius: 20px;
    overflow: hidden;
    box-shadow: 0 12px 40px rgba(0, 0, 0, 0.6);
    background: #07121a;
    display: flex;
    align-items: stretch;
    justify-content: stretch;
}

.pixi-stage {
    width: 100%;
    height: 100%;
    position: relative;
}
</style>
