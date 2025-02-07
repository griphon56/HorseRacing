using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HorseRacing.Infrastructure.Persistence.Configurations.Base
{
    public static class BaseChangeInfoConfigurationUtilities
    {
        public static void AttachChangeInfoForConfiguration<T, Y>(EntityTypeBuilder<T> builder)
            where T : AggregateRootChangeInfoGuid<Y>
            where Y : IdentityBaseGuid
        {
            builder.Property(m => m.CreatedUserId).HasConversion(id => id!.Value,
            value => UserId.Create(value));

            builder.Property(m => m.ChangedUserId).HasConversion(id => id!.Value,
                value => UserId.Create(value));

            builder.HasOne<User>().WithMany().HasForeignKey(m => m.CreatedUserId);
            builder.HasOne<User>().WithMany().HasForeignKey(m => m.ChangedUserId);
        }
    }
}
