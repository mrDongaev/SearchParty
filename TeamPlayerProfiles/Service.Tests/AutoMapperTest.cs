using AutoMapper;
using Service.Mapping;

namespace Service.Tests
{
    public class AutoMapperTest
    {
        [Test]
        public void AutoMapper_BetweenServiceAndDataAccess_IsCorrect()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HeroMappingProfile>();
                cfg.AddProfile<PositionMappingProfile>();
                cfg.AddProfile<PlayerMappingProfile>();
                cfg.AddProfile<TeamMappingProfile>();
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}
