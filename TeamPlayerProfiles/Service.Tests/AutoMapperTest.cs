using AutoMapper;
using Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
