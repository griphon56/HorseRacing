// gameMapper.ts

// Универсальный нормалайзер ID
export function normalizeGameId(rawId: any): string {
    if (!rawId) return '';
    if (typeof rawId === 'string') return rawId;
    if (typeof rawId === 'number') return String(rawId);

    if (typeof rawId === 'object') {
        if ('Value' in rawId && rawId.Value != null) return String(rawId.Value);
        if ('value' in rawId && rawId.value != null) return String(rawId.value);
        if ('Id' in rawId && rawId.Id != null) return String(rawId.Id);
        if ('gameId' in rawId && typeof rawId.gameId === 'string') return rawId.gameId;
    }
    return String(rawId);
}

// Маппинг карты
export function mapCard(raw: any) {
    if (!raw) return null;
    return {
        CardSuit: raw.CardSuit ?? raw.cardSuit ?? null,
        CardRank: raw.CardRank ?? raw.cardRank ?? null,
        CardOrder: raw.CardOrder ?? raw.cardOrder ?? null,
        Zone: raw.Zone ?? raw.zone ?? null,
    };
}

// Маппинг ставки
export function mapHorseBet(raw: any) {
    if (!raw) return null;
    return {
        BetSuit: raw.BetSuit ?? raw.betSuit ?? raw.CardSuit ?? null,
        BetAmount: raw.BetAmount ?? raw.betAmount ?? null,
        UserId: raw.UserId ?? raw.userId ?? null,
        FullName: raw.FullName ?? raw.fullName ?? null,
    };
}

// Маппинг события
export function mapEvent(raw: any) {
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
export function normalizeGamePayload(raw: any) {
    if (!raw) return { GameId: '', Name: '', InitialDeck: [], HorseBets: [], Events: [] };

    const payload = raw.Data ?? raw.data ?? raw;

    const gameId = normalizeGameId(payload.GameId ?? payload.gameId ?? payload.id ?? payload.Id);
    const name = payload.Name ?? payload.name ?? '';

    const initialDeckRaw = payload.InitialDeck ?? payload.initialDeck ?? [];
    const initialDeck = Array.isArray(initialDeckRaw)
        ? initialDeckRaw.map(mapCard).filter(Boolean)
        : [];

    const horseBetsRaw = payload.HorseBets ?? payload.horseBets ?? [];
    const horseBets = Array.isArray(horseBetsRaw)
        ? horseBetsRaw.map(mapHorseBet).filter(Boolean)
        : [];

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

export const GameMapper = {
    normalizeGameId,
    mapCard,
    mapHorseBet,
    mapEvent,
    normalizeGamePayload,
};
