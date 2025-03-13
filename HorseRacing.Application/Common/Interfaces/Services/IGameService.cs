using ErrorOr;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Получение случайной доступной масти
        /// </summary>
        /// <param name="game">Игра</param>
        Task<ErrorOr<SuitType>> GetRandomAvilableSuit(Game game);
    }
}
