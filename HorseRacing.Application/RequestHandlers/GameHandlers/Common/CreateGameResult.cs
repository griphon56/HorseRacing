﻿using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class CreateGameResult : BaseModelResult
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public required GameId GameId { get; set; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public StatusType Status { get; set; }
        /// <summary>
        /// Наименование комнаты
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Дата начала игры
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime? DateEnd { get; set; }
    }
}
