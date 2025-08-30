# "Скачки"
## 📜 Правила игры

### 🔥 Описание игры
"Скачки" — это карточная игра для нескольких игроков, в которой случайность и стратегия определяют победителя. Игроки делают ставки на одну из четырех "лошадей" (мастей карт), а затем следят за гонкой, где карты из колоды определяют движение лошадей по полю.

---

### 🎯 Цель игры
Задача игрока — сделать ставку "лошадь", которая первой пересечет финишную черту.

---

### 🃏 Игровая колода
- Используется стандартная **колода из 52 карт** без джокеров.
- Четыре туза - лошади забега (**♠️ Пики, ♥️ Черви, ♣️ Крести, ♦️ Бубны**).
- Пять карт из колоды - **препятствия на дистанции**. Они выкладываются рубашками вверх в вертикальную линию.
- Остальные карты составляют игровую колоду.

---

### 👥 Игроки
- В одной игре может участвовать **4 игрока**.
- Перед стартом игроки делают ставки на одну из лошадей.

---

### 💰 Ставки
- Тип ставок - Win (ставка на победителя)
- Минимальная ставка — 10 коинов.
- Максимальная ставка зависит от баланса игрока. 

---

### 📌 Режим игры
1. Классический
	- В момент создания игры, создатель игры устанавливает фиксированную ставку.
	- Собирается общий банк, победитель получает весь выигрыш.

### 🏇 Гонка
1. В вертикальную линию выстрамваются барьеры.
2. Лошади выстраиваются в горизонтальную линию внизу экрана.
3. Из колоды вытягивается по одной карте и переворачивается лицом вверх.
4. Лошадь с соответствующей мастью (туз) перемещается на одну позицию вперед.
5. Когда последняя лошадь пересекает барьер, эта карта открывается. Лошадь, с которой совпала масть, делает шаг назад.
6. Гонка продолжается, до тех пока все лошади не пересечет финишную линию (5 позиций).

---

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
        guid UserId PK
		datetime DateCreate
        guid CreateUserId
        datetime DateChange
        guid ChangeUserId
        bit IsRemoved
        nvarchar(100) Username
        nvarchar(100) Password
        nvarchar(100) FirstName
        nvarchar(100) LastName
        nvarchar(100) Email
        nvarchar(20) Phone
    }
    
    Accounts {
        guid AccountId PK
        guid UserId FK
        int Balance
    }
    Games {
        guid GameId PK
        int Status
        nvarchar(100) Name
        datetime DateStart
        datetime DateEnd
        datetime DateCreate
        guid CreateUserId FK
        datetime DateChange
        guid ChangeUserId FK
    }
    GameResults {
        guid GameResultId PK
        guid GameId FK
        guid UserId FK
        int BetSuit
        int Position
    }
    GamePlayers {
        guid GamePlayerId PK
        guid GameId FK
        guid UserId FK
        int BetSuit
        decimal BetAmount
    }
    
    GameDeckCards {
        guid GameDeckCardId PK
        guid GameId FK
        int CardSuit
        int CardRank
        int CardOrder
        int Zone
    }
    
    GameHorsePositions {
        guid GameHorsePositionId PK
        guid GameId FK
        int HorseSuit
        int Position
    }
    
    GameEvents {
        guid GameEventId PK
        guid GameId FK
        int EventType
        int CardSuit
        int CardRank
        int DeckCardOrder
        int HorseSuit
        int Position
        datetime EventDate
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
