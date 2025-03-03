using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Interfases;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.Entities;
using HorseRacing.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace HorseRacing.Infrastructure.Persistence.DbContexts
{
    public class HRDbContext : DbContext
    {
        /// <summary>
		/// Перехватчик событий домена публикации.
		/// </summary>
		private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
        private readonly IHashPasswordService _hashPasswordService;
        /// <summary>
        ///  Инициализирует новый экземпляр класса <see cref="HRDbContext"/> .
        /// </summary>
        /// <param name="options">Варианты.</param>
        /// <param name="publishDomainEventsInterceptor">Перехватчик событий домена публикации.</param>
        public HRDbContext(DbContextOptions<HRDbContext> options
            , PublishDomainEventsInterceptor publishDomainEventsInterceptor, IHashPasswordService hashPasswordService)
            : base(options)
        {
            _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
            _hashPasswordService = hashPasswordService;
        }
        /// <summary>
		/// Пользователи
		/// </summary>
		public DbSet<User> Users { get; set; } = null!;
        /// <summary>
        /// Баланс пользователя
        /// </summary>
        public DbSet<Account> Accounts { get; set; } = null!;
        /// <summary>
        /// Игры
        /// </summary>
        public DbSet<Game> Games { get; set; } = null!;
        /// <summary>
        /// События игры
        /// </summary>
        public DbSet<GameEvent> GameEvents { get; set; } = null!;
        /// <summary>
        /// Карты в игре
        /// </summary>
        public DbSet<GameDeckCard> GameDeckCards { get; set; } = null!;
        /// <summary>
        /// Позиции лошади в игре
        /// </summary>
        public DbSet<GameHorsePosition> GameHorsePositions { get; set; } = null!;
        /// <summary>
        /// Игроки
        /// </summary>
        public DbSet<GamePlayer> GamePlayers { get; set; } = null!;
        /// <summary>
        /// Результаты игр
        /// </summary>
        public DbSet<GameResult> GameResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<IList<IDomainEvent>>().ApplyConfigurationsFromAssembly(typeof(HRDbContext).Assembly);
            //modelBuilder.ApplyConfiguration(new SystemSettingsConfiguration(_hashPasswordService));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
