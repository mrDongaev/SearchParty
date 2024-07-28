using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public static class TestDataSeeder
    {
        public static async Task SeedTestData(TeamPlayerProfilesContext context)
        {
            var random = new Random();
            var getRand = () => random.Next(0, 5);
            Player[] players = new Player[20];
            List<Hero> heroes = await context.Heroes.ToListAsync();
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Name = $"player{i + 1}",
                    Description = "",
                    PositionId = getRand() + 1,
                };
                var max = getRand() + 1;
                for (int j = 0; j < max; j++)
                {
                    players[i].Heroes.Add(heroes[getRand()]);
                }
            }
            foreach (var player in players)
            {
                context.Add(player);
            }
            await context.SaveChangesAsync();
        }
    }
}
