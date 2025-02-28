# "Скачки"
## 📜 Правила игры

### 🔥 Описание игры
"Скачки" — это карточная азартная игра для нескольких игроков, в которой случайность и стратегия определяют победителя. Игроки делают ставки на одну из четырех "лошадей" (мастей карт), а затем следят за гонкой, где карты из колоды определяют движение лошадей по полю.

---

### 🎯 Цель игры
Задача игрока — сделать ставку на масть (лошадь), которая первой пересечет финишную черту.

---

### 🃏 Игровая колода
- Используется стандартная **колода из 52 карт** без джокеров.
- Четыре туза становятся лошадьми (**♠️ Пики, ♥️ Черви, ♣️ Крести, ♦️ Бубны**).
- Остальные карты перемешиваются и составляют игровую колоду.

---

### 👥 Игроки
- В одной игре может участвовать от **2 до 10 игроков**.
- Каждый игрок делает ставку на одну из лошадей.
- Ставки делаются до начала гонки.

---

### 📌 Подготовка к игре
1. Ведущий вытаскивает из колоды 4 туза и выкладывает их вертикально (лошади) в следующем порядке:
    - ♠️ Пики
    - ♥️ Черви
    - ♣️ Крести
    - ♦️ Бубны

2. Из оставшихся карт выкладываются **6 карт рубашкой вверх** горизонтально — это **препятствия на дистанции**.

3. Остальная часть колоды перемешивается.

---

### 💰 Ставки
- Перед стартом игроки делают ставки на одну из лошадей.
- Минимальная ставка — 10.
- Максимальная ставка зависит от баланса игрока.

---

### 🏇 Гонка
1. Ведущий начинает гонку, открывая по одной карте из колоды.
2. Лошадь с соответствующей мастью (туз) перемещается на одну позицию вперед.
3. Когда последняя лошадь пересекает препятствие (перевернутую карту), эта карта открывается:
   - Та лошадь, с которой совпала масть совпадает делает шаг назад.

4. Гонка продолжается, пока одна из лошадей не пересечет финишную линию (6 позиция).

---

### 🏆 Завершение игры
- Лошадь, которая первой пересекла финишную черту, становится победителем.
- Игроки, сделавшие ставку на победителя, получают выигрыш в зависимости от коэффициента.
- Остальные ставки проигрываются.

---

### 💰 Выигрыш
Формула расчета выигрыша:
``` 
Выигрыш = Ставка * 4
```
Если только один игрок сделал ставку на победителя, он получает весь банк игры.

---

### 🔍 Пример игры
| Игрок      | Ставка   | Лошадь   | Итог    |
|------------|----------|----------|---------|
| Иван       | 100      | ♥️ Черви | Проигрыш |
| Олег       | 200      | ♠️ Пики  | Выигрыш (800) |
| Анна       | 150      | ♦️ Бубны | Проигрыш |

---

### 🛑 Особые ситуации
- Если колода закончилась, гонка завершается без победителя.
- Если две лошади одновременно пересекают финиш, побеждает та, которая двигалась последней.

### 🔑 Конец игры
- После окончания игры всем игрокам начисляются выигрыши или списываются ставки.

## Схема базы данных
``` mermaid
erDiagram
    EventLog {
        int Id PK
        int EventType
        nvarchar(100) Message
        datetime TimeStamp
        int UserId FK
        nvarchar(100) Exception
    }
    Users {
        int UserId PK
        int CreateUserId
        datetime DateChange
        int ChangeUserId
        bit IsRemoved
        datetime DateCreate
        nvarchar(100) Username
        nvarchar(100) Password
        nvarchar(100) FirstName
        nvarchar(100) LastName
        nvarchar(100) Email
        nvarchar(20) Phone
    }
    
    Accounts {
        int AccountId PK
        int UserId FK
        int Balance
    }
    Games {
        int GameId PK
        int Status
        nvarchar(100) Name
        datetime DateStart
        datetime DateEnd
        datetime DateCreate
        int CreateUserId FK
        datetime DateChange
        int ChangeUserId FK
    }
    GameResults {
        int GameResultId PK
        int GameId FK
        int UserId FK
        nvarchar(1) BetSuit
        int Position
    }
    GamePlayers {
        int GamePlayerId PK
        int GameId FK
        int UserId FK
        nvarchar(1) BetSuit
        decimal BetAmount
    }
    
    GameDeckCards {
        int GameDeckCardId PK
        int GameId FK
        nvarchar(1) CardSuit
        nvarchar(2) CardRank
        int CardOrder
        int Zone
    }
    
    GameHorsePositions {
        int GameHorsePositionId PK
        int GameId FK
        nvarchar(1) HorseSuit
        int Position
    }
    
    GameEvents {
        int GameEventId PK
        int GameId FK
        int EventType
        nvarchar(1) CardSuit
        nvarchar(2) CardRank
        int DeckCardOrder
        nvarchar(1) HorseSuit
        int OldPosition
        int NewPosition
        datetime DateCreate
        int CreateUserId FK
    }
    
    Users ||--o{ Accounts : owns
    Users ||--o{ EventLog : logs
    Users ||--o{ GamePlayers : plays
    Games ||--o{ GamePlayers : has
    Games ||--o{ GameDeckCards : has
    Games ||--o{ GameHorsePositions : has
    Games ||--o{ GameEvents : has
    Games ||--o{ GameResults : produces
    GamePlayers ||--o{ GameResults : bets
```
