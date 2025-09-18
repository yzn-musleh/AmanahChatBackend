using Application.Common;
using AutoMapper;

namespace Application
{
    public static class ConfigureMapperProfile
    {
        public static IMapperConfigurationExpression AddApplicationMapperProfile(
            this IMapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.AddProfile(typeof(ApplicationAutoMapperProfile));

            return mapperConfiguration;
        }
    }
}
