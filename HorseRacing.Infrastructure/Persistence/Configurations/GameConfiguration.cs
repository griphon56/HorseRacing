using HorseRacing.Domain.GameAggregate;
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
            ConfigureGameEventTable(builder);
            ConfigureGameHorsePositionTable(builder);
            ConfigureGamePlayerTable(builder);
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

            builder.Property(m => m.DefaultBet).HasPrecision(18, 2);
            builder.Property<byte[]>("_versionRow").HasColumnName("VersionRow").IsRowVersion();

            builder.Property(m => m.Name).HasMaxLength(100);

            BaseChangeInfoConfigurationUtilities.AttachChangeInfoForConfiguration<Game, GameId>(builder);
        }

        private void ConfigureGameResultTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GameResults, a =>
            {
                a.ToTable("GameResults");
                a.HasKey(m => m.Id);

                a.WithOwner().HasForeignKey(m => m.GameId);

                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameResultId.Create(value));
                a.Property(m => m.UserId).HasConversion(id => id.Value, value => UserId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));

                a.HasOne<User>().WithMany().HasForeignKey(m => m.UserId);
            });

            builder.Metadata.FindNavigation(nameof(Game.GameResults))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureGameDeckCardTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GameDeckCards, a =>
            {
                a.ToTable("GameDeckCards");
                a.HasKey(m => m.Id);

                a.WithOwner().HasForeignKey(m => m.GameId);
                
                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameDeckCardId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Game.GameDeckCards))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureGameEventTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GameEvents, a =>
            {
                a.ToTable("GameEvents");
                a.HasKey(m => m.Id);

                a.WithOwner().HasForeignKey(m => m.GameId);
                
                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameEventId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Game.GameEvents))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureGameHorsePositionTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GameHorsePositions, a =>
            {
                a.ToTable("GameHorsePositions");
                a.HasKey(m => m.Id);

                a.WithOwner().HasForeignKey(m => m.GameId);
                
                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GameHorsePositionId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));
            });

            builder.Metadata.FindNavigation(nameof(Game.GameHorsePositions))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureGamePlayerTable(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsMany(u => u.GamePlayers, a =>
            {
                a.ToTable("GamePlayers");
                a.HasKey(m => m.Id);
                a.WithOwner().HasForeignKey(m => m.GameId);

                a.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => GamePlayerId.Create(value));
                a.Property(m => m.UserId).HasConversion(id => id.Value, value => UserId.Create(value));
                a.Property(m => m.GameId).HasConversion(id => id.Value, value => GameId.Create(value));

                a.Property(m => m.BetAmount).HasPrecision(18, 2);

                a.HasOne<User>().WithMany().HasForeignKey(m => m.UserId);
            });

            builder.Metadata.FindNavigation(nameof(Game.GamePlayers))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
