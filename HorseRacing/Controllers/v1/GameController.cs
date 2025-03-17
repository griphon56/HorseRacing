using Asp.Versioning;
using HorseRacing.Api.Controllers.Base;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGameWithBet;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.PlaceBet;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame;
using HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetAvailableSuit;
using HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGame;
using HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGameResults;
using HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetWaitingGames;
using HorseRacing.Contracts.Models.Game.Dtos;
using HorseRacing.Contracts.Models.Game.Requests;
using HorseRacing.Contracts.Models.Game.Responses;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HorseRacing.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GameController : JwtBasedApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GameController(IMediator mediator, IMapper mapper, ILogger<BaseApiController> logger) : base(logger)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("create-game")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
        {
            var createGameResult = await _mediator.Send(new CreateGameCommand() {
                UserId = UserId.Create(request.Data.UserId),
                Name = request.Data.Name,
                BetAmount = request.Data.BetAmount,
                BetSuit = (SuitType)request.Data.BetSuit
            });

            return createGameResult.Match(
                res => Ok(_mapper.Map<CreateGameResponse>(res)),
                errors => Problem(errors));
        }

        [HttpPost("get-game")]
        public async Task<IActionResult> GetGameById([FromBody] GetGameRequest request)
        {
            var gameResult = await _mediator.Send(new GetGameQuery() { GameId = GameId.Create(request.Data.Id) });

            return gameResult.Match(
                res => Ok(_mapper.Map<GetGameResponse>(res)),
                errors => Problem(errors));
        }

        [HttpPost("get-waiting-games")]
        public async Task<IActionResult> GetWaitingGames()
        {
            var gameResult = await _mediator.Send(new GetWaitingGamesQuery());

            return gameResult.Match(
                res => Ok(new GetWaitingGamesResponse(_mapper.Map<GetWaitingGamesResponseDto>(res))),
                errors => Problem(errors));
        }

        //[HttpPost("join-game")]
        //public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest request)
        //{
        //    var gameResult = await _mediator.Send(new JoinGameCommand()
        //    {
        //        GameId = GameId.Create(request.Data.GameId),
        //        UserId = UserId.Create(request.Data.UserId),
        //    });

        //    return gameResult.Match(
        //        res => Ok(),
        //        errors => Problem(errors));
        //}

        [HttpPost("join-game-with-bet")]
        public async Task<IActionResult> JoinGameWithBet([FromBody] JoinGameWithBetRequest request)
        {
            var gameResult = await _mediator.Send(new JoinGameWithBetCommand()
            {
                GameId = GameId.Create(request.Data.GameId),
                UserId = UserId.Create(request.Data.UserId),
                BetAmount = request.Data.BetAmount,
                BetSuit = (SuitType)request.Data.BetSuit
            });

            return gameResult.Match(
                res => Ok(),
                errors => Problem(errors));
        }

        //[HttpPost("place-bet")]
        //public async Task<IActionResult> PlaceBet([FromBody] PlaceBetRequest request)
        //{
        //    var gameResult = await _mediator.Send(new PlaceBetCommand()
        //    {
        //        GameId = GameId.Create(request.Data.GameId),
        //        UserId = UserId.Create(request.Data.UserId),
        //        BetSuit = (SuitType)request.Data.BetSuit,
        //        BetAmount = request.Data.BetAmount
        //    });

        //    return gameResult.Match(
        //        res => Ok(),
        //        errors => Problem(errors));
        //}

        [HttpPost("get-available-suit")]
        public async Task<IActionResult> GetAvailableSuit([FromBody] GetAvailableSuitRequest request)
        {
            var gameResult = await _mediator.Send(new GetAvailableSuitQuery()
            {
                GameId = GameId.Create(request.Data.Id)
            });

            return gameResult.Match(
                res => Ok(_mapper.Map<GetAvailableSuitResponse>(res)),
                errors => Problem(errors));
        }

        [HttpPost("start-game")]            
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            var gameResult = await _mediator.Send(new StartGameCommand()
            {
                GameId = GameId.Create(request.Data.Id)
            });

            return gameResult.Match(
                res => Ok(),
                errors => Problem(errors));
        }

        [HttpPost("get-game-result")]
        public async Task<IActionResult> GetGameResult([FromBody] GetGameResultRequest request)
        {
            var gameResult = await _mediator.Send(new GetGameResultQuery()
            {
                GameId = GameId.Create(request.Data.Id)
            });

            return gameResult.Match(
                res => Ok(_mapper.Map<GetGameResultResponse>(res)),
                errors => Problem(errors));
        }
    }
}
