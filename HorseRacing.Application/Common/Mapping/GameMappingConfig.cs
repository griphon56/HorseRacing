using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Contracts.Models.Game.Dtos;
using HorseRacing.Contracts.Models.Game.Responses.CheckPlayerConnectedToGame;
using HorseRacing.Contracts.Models.Game.Responses.CreateGame;
using HorseRacing.Contracts.Models.Game.Responses.GetAvailableSuit;
using HorseRacing.Contracts.Models.Game.Responses.GetGame;
using HorseRacing.Contracts.Models.Game.Responses.GetGameResult;
using HorseRacing.Contracts.Models.Game.Responses.GetLobbyUsersWithBets;
using HorseRacing.Contracts.Models.Game.Responses.GetWaitingGames;
using HorseRacing.Contracts.Models.Game.Responses.JoinGameWithBet;
using HorseRacing.Contracts.Models.Game.Responses.PlayGame;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using Mapster;

namespace HorseRacing.Application.Common.Mapping
{
    public class GameMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Game, GetGameResult>()
                .Map(dest => dest.GameId, src => src.Id.Value);

            config.NewConfig<GetGameResult, GetGameResponse>()
                .Map(dest => dest.Data, src => src)
                .Map(dest => dest.Data.GameId, src => src.GameId.Value);

            config.NewConfig<GetWaitingGamesResult, GetWaitingGamesResponseDto>()
                .Map(dest => dest, src => src);

            config.NewConfig<CreateGameResult, CreateGameResponse>()
                .Map(dest => dest.Data, src => src)
                .Map(dest => dest.Data.GameId, src => src.GameId.Value);

            config.NewConfig<Game, GameView>()
                .Map(dest => dest, src => src);

            config.NewConfig<GameView, GameDto>()
                .Map(dest => dest, src => src)
                .Map(dest => dest.GameId, src => src.GameId.Value);

            config.NewConfig<GameAvailableSuitView, GetAvailableSuitResponseDto>()
                .Map(dest => dest, src => src);

            config.NewConfig<GetAvailableSuitResult, GetAvailableSuitResponse>()
                .Map(dest => dest.DataValues, src => src.AvailableSuits);

            config.NewConfig<GameResultView, GetGameResultResponseDto>()
                .Map(dest => dest, src => src);

            config.NewConfig<GetGameResultsResult, GetGameResultResponse>()
                .Map(dest => dest.DataValues, src => src.GameResults);

            config.NewConfig<GameUserView, GameUserDto>()
                .Map(dest => dest, src => src)
                .Map(dest => dest.UserId, src => src.UserId.Value)
                .Map(dest => dest.BetSuit, src => (int)src.BetSuit);

            config.NewConfig<LobbyUsersWithBetsView, GetLobbyUsersWithBetsResponseDto>()
                .Map(dest => dest, src => src)
                .Map(dest => dest.GameId, src => src.GameId.Value)
                .Map(dest => dest.Players, src => src.Players);

            config.NewConfig<GetLobbyUsersWithBetsResult, GetLobbyUsersWithBetsResponse>()
                .Map(dest => dest.Data, src => src.Data);

            config.NewConfig<CheckPlayerConnectedToGameResult, CheckPlayerConnectedToGameResponse>()
                .Map(dest => dest.Data.IsConnected, src => src.IsConnected);

            config.NewConfig<JoinGameWithBetResult, JoinGameWithBetResponse>()
                .Map(dest => dest.Data.IsLastPlayer, src => src.IsLastPlayer);

            config.NewConfig<PlayGameResult, PlayGameResponseDto>()
                .Map(dest => dest.GameId, src => src.GameId.Value)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Events, src => src.Events)
                .Map(dest => dest.HorseBets, src => src.HorseBets)
                .Map(dest => dest.InitialDeck, src => src.InitialDeck);

            config.NewConfig<GameDeckCardView, GameDeckCardDto>()
                .Map(dest => dest.CardOrder, src => src.CardOrder)
                .Map(dest => dest.CardRank, src => (int)src.CardRank)
                .Map(dest => dest.CardSuit, src => (int)src.CardSuit)
                .Map(dest => dest.Zone, src => (int)src.Zone);

            config.NewConfig<GameEventView, GameEventDto>()
                .Map(dest => dest.Step, src => src.Step)
                .Map(dest => dest.CardOrder, src => src.CardOrder)
                .Map(dest => dest.EventType, src => (int)src.EventType)
                .Map(dest => dest.CardRank, src => src.CardRank)
                .Map(dest => dest.HorseSuit, src => src.HorseSuit)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.EventDate, src => src.EventDate);

            config.NewConfig<HorseBetView, HorseBetDto>()
                .Map(dest => dest.UserId, src => src.UserId.Value)
                .Map(dest => dest.FullName, src => src.FullName)
                .Map(dest => dest.BetAmount, src => src.BetAmount)
                .Map(dest => dest.BetSuit, src => (int)src.BetSuit);
        }
    }
}