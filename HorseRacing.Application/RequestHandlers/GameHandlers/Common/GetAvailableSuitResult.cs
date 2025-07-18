﻿using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class GetAvailableSuitResult: BaseModelResult
    {
        public required List<GameAvailableSuitView> AvailableSuits { get; set; }
    }
}
