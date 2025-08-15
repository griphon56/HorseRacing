import * as PIXI from 'pixi.js';
import { defaultGameConfig } from '~/core/games/game-config';
const texturesCache = new Map<string, PIXI.Texture>();

// Загружает spritesheet JSON через PIXI.Assets и сохраняет все текстуры в texturesCache.
export async function loadCardAtlasAndCache(): Promise<void> {
    (await PIXI.Assets.load(defaultGameConfig.CARD_ATLAS_JSON_PATH)) as PIXI.Spritesheet;

    // Assets.get может вернуть либо объект spritesheet с .textures, либо сам набор текстур
    const res: any = PIXI.Assets.get(defaultGameConfig.CARD_ATLAS_JSON_PATH);
    const texturesObj: Record<string, PIXI.Texture> | undefined = res?.textures ?? res;

    if (!texturesObj || typeof texturesObj !== 'object') {
        throw new Error('Atlas load failed: textures not found');
    }

    // заполняем кеш
    for (const name of Object.keys(texturesObj)) {
        const t = texturesObj[name] as PIXI.Texture;
        if (t) texturesCache.set(name, t);
    }
}

export function getCardTextureByKey(key: string): PIXI.Texture {
  const t = texturesCache.get(key);
  if (!t) throw new Error(`Texture not found ${key}`);
  return t;
}

/**
 * Получить текстуру карты по масти и рангу.
 * В json ключи называются как `${rankIndex}_${suit}.png` (пример: "0_1.png").
 */
export function getCardTextureFor(suit: number, rankIndex: number): PIXI.Texture {
  const key = `${rankIndex}_${suit}.png`; // адаптируй под имена в json
  return getCardTextureByKey(key);
}

/** Текстура рубашки */
export function getBackTexture(): PIXI.Texture {
  return getCardTextureByKey('card_back.png');
}
