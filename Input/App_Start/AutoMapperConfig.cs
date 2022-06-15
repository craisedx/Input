using AutoMapper;
using Input.ViewModels.Mappings;

namespace Input
{
    public class AutoMapperConfig
    {
        public static void RegisterMappers()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MappingProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
        }
    }
}