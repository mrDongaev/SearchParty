﻿using Common.Models.Enums;
using Service.Contracts.Hero;

namespace Service.Contracts.Player
{
    public class CreatePlayerDto
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PositionName Position { get; set; }

        public ISet<HeroDto> Heroes { get; set; }
    }
}
