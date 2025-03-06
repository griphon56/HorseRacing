using Asp.Versioning;
using HorseRacing.Api.Controllers.Base;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame;
using HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGame;
using HorseRacing.Contracts.Models.Game.Requests;
using HorseRacing.Contracts.Models.Game.Responses;
using HorseRacing.Domain.GameAggregate.ValueObjects;
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
            var createGameResult = await _mediator.Send(new CreateGameCommand() { Name = request.Data.Name });

            return createGameResult.Match(
                registerResult => Ok(_mapper.Map<CreateGameResponse>(registerResult)),
                errors => Problem(errors));
        }

        [HttpPost("get-game")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var gameResult = await _mediator.Send(new GetGameQuery() { GameId = GameId.Create(id) });

            return gameResult.Match(
                res => Ok(_mapper.Map<GetGameResponse>(res)),
                errors => Problem(errors));
        }

        ///// <summary>
        ///// Позволяет игроку присоединиться к игре.
        ///// Данные поступают через FormBody.
        ///// </summary>
        ///// <param name="id">Идентификатор игры</param>
        ///// <param name="joinGameDto">Данные игрока и его ставка</param>
        //[HttpPost("{id}/join")]
        //public async Task<IActionResult> JoinGame(int id, [FromForm] JoinGameDto joinGameDto)
        //{
        //    var game = await _gameRepository.GetByIdAsync(id);
        //    if (game == null)
        //        return NotFound();

        //    // Добавляем игрока в игру (метод агрегата добавляет игрока и регистрирует доменные события)
        //    game.AddPlayer(joinGameDto.UserId, joinGameDto.BetSuit, joinGameDto.BetAmount);

        //    // Сохраняем изменения агрегата
        //    await _gameRepository.UpdateAsync(game);
        //    return Ok();
        //}

        ///// <summary>
        ///// Запускает игру (начало гонки).
        ///// </summary>
        ///// <param name="id">Идентификатор игры</param>
        //[HttpPut("{id}/start")]
        //public async Task<IActionResult> StartGame(int id)
        //{
        //    var game = await _gameRepository.GetByIdAsync(id);
        //    if (game == null)
        //        return NotFound();

        //    // В агрегате Game реализован метод Start(), который инициализирует состояние (раскладывает колоду, устанавливает позиции лошадей, регистрирует события)
        //    game.Start();

        //    await _gameRepository.UpdateAsync(game);
        //    return Ok(game);
        //}

        ///// <summary>
        ///// Завершает игру, фиксируя результаты.
        ///// </summary>
        ///// <param name="id">Идентификатор игры</param>
        //[HttpPut("{id}/finish")]
        //public async Task<IActionResult> FinishGame(int id)
        //{
        //    var game = await _gameRepository.GetByIdAsync(id);
        //    if (game == null)
        //        return NotFound();

        //    // Завершение игры – метод Finish() агрегата фиксирует результаты и генерирует доменные события для обработки результатов
        //    game.Finish();

        //    await _gameRepository.UpdateAsync(game);
        //    return Ok(game);
        //}

        ///// <summary>
        ///// Возвращает результаты игры.
        ///// </summary>
        ///// <param name="id">Идентификатор игры</param>
        //[HttpGet("{id}/results")]
        //public async Task<ActionResult<GameResultsDto>> GetGameResults(int id)
        //{
        //    var game = await _gameRepository.GetByIdAsync(id);
        //    if (game == null)
        //        return NotFound();

        //    // Преобразуем результаты игры в DTO. Это может быть список результатов для каждого игрока.
        //    var results = new GameResultsDto
        //    {
        //        // Пример заполнения данных из агрегата Game
        //        // Results = game.GameResults.Select(r => new GameResultItemDto { UserId = r.UserId, BetSuit = r.BetSuit, Position = r.Position }).ToList()
        //    };
        //    return Ok(results);
        //}
    }
}
