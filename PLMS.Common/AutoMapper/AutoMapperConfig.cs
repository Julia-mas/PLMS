using AutoMapper;
using System.Reflection;

namespace PLMS.Common.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper ServiceMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.Load("PLMS.BLL"));
                cfg.AddMaps(Assembly.Load("PLMS.API"));
            });

            config.AssertConfigurationIsValid();

            return config.CreateMapper();

        }
    }
}
