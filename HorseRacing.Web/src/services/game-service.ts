export async function loadMockGame(path = '/mock/game.json') {
  const r = await fetch(path);
  const parsed = await r.json();
  return parsed.Data ?? parsed;
}
