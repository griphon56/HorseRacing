﻿using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using HorseRacing.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HorseRacing.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Настройка таблицы пользователей
        /// </summary>
        /// <param name="builder">Строитель.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureUserTable(builder);
        }

        /// <summary>
        /// Configures the user table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void ConfigureUserTable(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedNever().HasConversion(id => id.Value,
                value => UserId.Create(value));

            builder.Property(m => m.UserName).HasMaxLength(256);
            builder.Property(m => m.FirstName).HasMaxLength(100);
            builder.Property(m => m.LastName).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(150);
            builder.Property(m => m.Phone).HasMaxLength(50);
            builder.Property(m => m.Password).HasMaxLength(1000);

            BaseChangeInfoConfigurationUtilities.AttachChangeInfoForConfiguration<User, UserId>(builder);

            builder.HasData(User.GetDefaultUser());
        }
    }
}
