using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Contracts.Models.Game.Dtos;
using HorseRacing.Contracts.Models.Game.Responses.CreateGame;
using HorseRacing.Contracts.Models.Game.Responses.GetAvailableSuit;
using HorseRacing.Contracts.Models.Game.Responses.GetGame;
using HorseRacing.Contracts.Models.Game.Responses.GetGameResult;
using HorseRacing.Contracts.Models.Game.Responses.GetLobbyUsersWithBets;
using HorseRacing.Contracts.Models.Game.Responses.GetWaitingGames;
using HorseRacing.Domain.GameAggregate;
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
        }
    }
}