<template>
    <div class="race-root">
        <!-- Pixi container: занимает всю доступную область на мобильном экране -->
        <div ref="pixiContainer" class="pixi-container" />

        <!-- Лёгкая полупрозрачная оверлей-индикация загрузки/ошибки (минималистично) -->
        <div v-if="state.loading || state.error" class="overlay">
            <div v-if="state.loading" class="loader">Загрузка...</div>
            <div v-else class="err">{{ state.error }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, reactive } from 'vue';
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response';
import { defaultGameConfig } from '~/core/games/game-config';
import { loadCardAtlasAndCache, getCardTextureFor, getBackTexture } from '~/services/atlas-service';
import { loadMockGame } from '~/services/game-service';
import { PixiRendererService } from '~/services/pixi-renderer-service';

const pixiContainer = ref<HTMLDivElement | null>(null);

const state = reactive({
    loading: true,
    error: '' as string | null,
});

let renderer: PixiRendererService | null = null;

const atlasProvider = {
    getCardTextureFor: (suit: number, rankIndex: number) => getCardTextureFor(suit, rankIndex),
    getBackTexture: () => getBackTexture(),
};

onMounted(async () => {
    state.loading = true;
    state.error = null;

    // 1) загрузить атлас карт
    try {
        await loadCardAtlasAndCache();
    } catch (err: any) {
        // показываем ошибку, но пытаемся продолжить — PixiRendererService должен иметь fallback
        state.error = 'Не удалось загрузить атлас карт';
        console.warn('Atlas load error', err);
        // не return — оставляем возможность продолжить (если нужно)
    }

    // 2) загрузить mock-данные игры
    let gameData: any = { InitialDeck: [], HorseBets: [], Events: [] };
    try {
        const parsed = (await loadMockGame('/mock/game.json')) as PlayGameResponse;
        gameData = parsed.Data ?? parsed;
    } catch (err: any) {
        // если mock не доступен — создаём пустую игру, отображаем сообщение
        state.error = state.error
            ? state.error + '; Mock not found'
            : 'Не удалось загрузить mock-данные';
        console.warn('Mock load error', err);
        gameData = { InitialDeck: [], HorseBets: [], Events: [] };
    }

    // 3) инициализировать рендерер
    try {
        if (!pixiContainer.value) throw new Error('pixi container not found');

        renderer = new PixiRendererService(pixiContainer.value, defaultGameConfig, atlasProvider);
        await renderer.init();

        // 4) отрисовать начальное состояние и запустить последовательный прогон событий
        renderer.drawInitial(gameData);
        renderer.startEvents(gameData.Events ?? []);
    } catch (err: any) {
        state.error =
            err && err.message
                ? `Ошибка инициализации рендерера: ${err.message}`
                : 'Ошибка инициализации рендерера';
        console.error('Renderer init/draw error', err);

        try {
            renderer?.destroy();
            renderer = null;
        } catch {}
    } finally {
        state.loading = false;
    }
});

onBeforeUnmount(() => {
    try {
        renderer?.stopEvents();
        renderer?.destroy();
        renderer = null;
    } catch (err) {
        // silently
        console.warn('cleanup error', err);
    }
});
</script>

<style scoped>
/* Вертикальная мобильная верстка: канва должна занимать почти весь экран */
.race-root {
    width: 100%;
    height: calc(100vh - 200px); /* ориентировано на мобильный вертикальный экран */
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    display: flex;
    flex-direction: column;
    background: linear-gradient(180deg, #0e2233 0%, #07121a 100%);
    -webkit-touch-callout: none; /* удобство на iOS */
    -webkit-user-select: none;
    -ms-user-select: none;
    user-select: none;
}

/* Контейнер для Pixi canvas: займёт всё доступное место */
.pixi-container {
    flex: 1 1 auto;
    width: 100%;
    height: 100%;
    position: relative;
    overflow: hidden;
    display: block;
}

/* Минималистичный overlay для загрузки/ошибки */
.overlay {
    position: absolute;
    top: 10px;
    left: 50%;
    transform: translateX(-50%);
    z-index: 100;
    pointer-events: none;
    width: auto;
    max-width: 90%;
    text-align: center;
}

.loader {
    display: inline-block;
    background: rgba(255, 255, 255, 0.06);
    color: #e6f2ff;
    padding: 8px 12px;
    border-radius: 10px;
    font-weight: 600;
    font-size: 14px;
    backdrop-filter: blur(4px);
}

.err {
    display: inline-block;
    background: rgba(255, 60, 60, 0.12);
    color: #ffdede;
    padding: 8px 12px;
    border-radius: 10px;
    font-weight: 600;
    font-size: 13px;
}

/* Убираем скролл и обеспечиваем полную высоту на мобильных браузерах */
html,
body,
#app {
    height: 100%;
    margin: 0;
    padding: 0;
}

/* При необходимости: лёгкая адаптация шрифта на мелких экранах */
@media (max-width: 420px) {
    .loader,
    .err {
        font-size: 13px;
        padding: 6px 10px;
    }
}
</style>
