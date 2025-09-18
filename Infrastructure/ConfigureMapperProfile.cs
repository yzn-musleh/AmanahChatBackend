using Application.Common;
using AutoMapper;
using Infrastructure.Common;

namespace Infrastructure
{
    public static class ConfigureMapperProfile
    {
        public static IMapperConfigurationExpression AddInfrastructureAutoMapperProfile(
            this IMapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.AddProfile(typeof(InfrastructureAutoMapperProfile));

            return mapperConfiguration;
        }
    }
}
