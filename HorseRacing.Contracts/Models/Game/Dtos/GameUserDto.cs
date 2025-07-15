namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GameUserDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required Guid UserId { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public required string FullName { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
        public required int BetAmount { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public required int BetSuit { get; set; }
    }
}
