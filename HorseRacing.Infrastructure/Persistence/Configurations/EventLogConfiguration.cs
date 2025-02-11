using HorseRacing.Domain.EventLogAggregate;
using HorseRacing.Domain.EventLogAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HorseRacing.Infrastructure.Persistence.Configurations
{
    /// <summary>
	/// Конфигурация журнала событий.
	/// </summary>
	public class EventLogConfiguration : IEntityTypeConfiguration<EventLog>
    {
        /// <summary>
        /// Настройка таблицы журнала событий
        /// </summary>
        /// <param name="builder">Строитель.</param>
        public void Configure(EntityTypeBuilder<EventLog> builder)
        {
            ConfigureEventLogTable(builder);
        }

        /// <summary>
        /// Configures the EventLog table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void ConfigureEventLogTable(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable("EventLog");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasConversion(id => id.Value,
                value => EventLogId.Create(value)).ValueGeneratedOnAdd();

            builder.Property(m => m.EventTitle).HasMaxLength(1000);
            builder.Property(m => m.InitiatorInfoText).HasMaxLength(500);
            builder.Property(m => m.Level).HasMaxLength(128);

            builder.Property(m => m.UserId).HasConversion(id => id.Value,
            value => UserId.Create(value));

            builder.HasOne<User>().WithMany().HasForeignKey(m => m.UserId);
        }
    }
}
