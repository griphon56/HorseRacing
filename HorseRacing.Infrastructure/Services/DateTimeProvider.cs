using HorseRacing.Application.Common.Interfaces.Services;

namespace HorseRacing.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}
