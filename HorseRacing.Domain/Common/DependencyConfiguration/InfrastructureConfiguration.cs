using HorseRacing.Domain.Common.DependencyConfiguration.Base;

namespace HorseRacing.Domain.Common.DependencyConfiguration
{
    public class InfrastructureConfiguration : BaseDependencyConfiguration
    {
        public bool EnableAuthorizationConfiguration { get; set; } = true;
        public bool EnableCachingConfiguration { get; set; } = false;
    }
}
