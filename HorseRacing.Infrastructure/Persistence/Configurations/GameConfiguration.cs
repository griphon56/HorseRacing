using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using HorseRacing.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HorseRacing.Infrastructure.Persistence.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        /// <summary>
        /// Настройка таблицы пользователей
        /// </summary>
        /// <param name="builder">Строитель.</param>
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            ConfigureGameTable(builder);
            ConfigureGameDeckCardTable(builder);
            ConfigureGameResultTable(builder);
        }

        /// <summary>
        /// Configures the user table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void ConfigureGameTable(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value,
                value => GameId.Create(value));

            builder.Property(m => m.Name).HasMaxLength(100);

            BaseChangeInfoConfigurationUtilities.AttachChangeInfoForConfiguration<Game, GameId>(builder);
        }

        private void ConfigureGameDeckCardTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GameDeckCards, a =>
            {
                a.ToTable("GameDeckCards");
                a.WithOwner().HasForeignKey(m => m.GameId);
                a.HasKey(m => m.Id);
                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameDeckCardId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(User.Account))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureGameResultTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsOne(u => u.GameResult, a =>
            {
                a.ToTable("GameResults");
                a.WithOwner().HasForeignKey(m => m.GameId);
                a.HasKey(m => m.Id);
                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameResultId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(User.Account))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
