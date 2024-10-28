using AutoMapper;
using Service.Mapping;

namespace WebAPI.Tests
{
    public class AutoMapperTest
    {
        [Test]
        public void AutoMapper_BetweenWebApiAndService_IsCorrect()
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