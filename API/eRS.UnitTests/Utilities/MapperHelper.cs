using AutoMapper;
using Playground.Service.Mappers;

namespace eRS.UnitTests.Utilities;

public static class MapperHelper
{
    public static IMapper CreateMapper()
    {
        var myProfile = new MappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var autoMapper = new Mapper(configuration);

        return autoMapper;
    }
}
