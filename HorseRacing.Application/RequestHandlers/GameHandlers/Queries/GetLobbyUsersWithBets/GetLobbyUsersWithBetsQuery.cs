﻿using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetLobbyUsersWithBets
{
    /// <summary>
    /// Команда полкучения списка пользователей в лобби с их ставками.
    /// </summary>
    public class GetLobbyUsersWithBetsQuery : IRequest<ErrorOr<GetLobbyUsersWithBetsResult>>
    {
        /// <summary>
        /// Идентификатор игры
        /// </summary>
        public GameId GameId { get; }
        public GetLobbyUsersWithBetsQuery(GameId gameId)
        {
            GameId = gameId;
        }
    }
}
