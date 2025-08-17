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
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response';

const pixiContainer = ref<HTMLDivElement | null>(null);
const phoneFrame = ref<HTMLDivElement | null>(null);
let renderer: PixiRendererService | null = null;

onMounted(async () => {
    try {
        await loadCardAtlasAndCache();
    } catch (e) {
        console.warn(e);
    }

    let gameData: any = { InitialDeck: [], HorseBets: [], Events: [] };
    try {
        const parsed = (await loadMockGame('/mock/game.json')) as PlayGameResponse;
        gameData = parsed.Data ?? parsed;
    } catch (e) {
        console.warn(e);
    }

    if (!pixiContainer.value) return;

    const atlasProvider = { getCardTextureFor, getBackTexture };

    renderer = new PixiRendererService(pixiContainer.value, defaultGameConfig, atlasProvider);
    await renderer.init();
    renderer.drawInitial(gameData);
    renderer.startEvents(gameData.Events ?? []);
});

onBeforeUnmount(() => {
    try {
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
