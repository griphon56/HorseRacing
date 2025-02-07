using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Interfases;
using HorseRacing.Domain.UserAggregate;
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
		/// <value>Значение набора баз данных (DbSet).</value>
		public DbSet<User> Users { get; set; } = null!;

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
