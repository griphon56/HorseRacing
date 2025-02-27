# Скачки
## Правила игры

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
        decimal Balance
        datetime DateCreate
        int CreateUserId FK
        datetime DateChange
        int ChangeUserId FK
    }
    Games {
        int GameId PK
        int Status
        datetime DateStart
        datetime DateEnd
        datetime DateCreate
        int CreateUserId FK
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
