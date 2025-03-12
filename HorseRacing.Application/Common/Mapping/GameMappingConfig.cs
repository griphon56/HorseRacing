using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Contracts.Models.Game.Dtos;
using HorseRacing.Contracts.Models.Game.Responses;
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
        }
    }
}
