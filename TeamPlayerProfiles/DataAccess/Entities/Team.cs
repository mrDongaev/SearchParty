﻿using Common.Models;
using DataAccess.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Team : IProfile, IUpdateable
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int PlayerCount { get; set; }

        public ICollection<Player> Players { get; protected set; } = [];

        [NotMapped]
        public ICollection<PlayerInTeam> PlayersInTeam { get; set; } = [];

        public ICollection<TeamPlayer> TeamPlayers { get; protected set; } = [];
    }
}
