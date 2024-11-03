using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public static class TestDataSeeder
    {
        public static async Task SeedTestData(TeamPlayerProfilesContext context)
        {
            Random random = new Random();
            var getRand = (int a, int b) => random.Next(a, b);
            Player[] players = new Player[20];
            Team[] teams = new Team[10];
            User[] users = new User[20];
            List<Hero> heroes = await context.Heroes.ToListAsync();
            string emptyGuid = Guid.Empty.ToString();

            for (int i = 0; i < users.Length; i++)
            {
                string indexStr = (i + 1).ToString();
                users[i] = new User
                {
                    Id = Guid.Parse("a" + emptyGuid[1..^indexStr.Length] + indexStr),
                    Mmr = (uint)getRand(0, 20001),
                };
            }
            foreach (var user in users)
            {
                context.Add(user);
            }

            for (int i = 0; i < players.Length; i++)
            {
                string indexStr = (i + 1).ToString();
                players[i] = new Player
                {
                    Id = Guid.Parse(emptyGuid[..^indexStr.Length] + indexStr),
                    UserId = users[i].Id,
                    Name = $"player{i + 1}",
                    Description = $"player-description{i + 1}",
                    PositionId = getRand(1, 6),
                };
                var max = getRand(0, 6);
                for (int j = 0; j < max; j++)
                {
                    players[i].Heroes.Add(heroes[getRand(0, 5)]);
                }
            }
            foreach (var player in players)
            {
                context.Add(player);
            }

            for (int i = 0; i < teams.Length; i++)
            {
                string indexStr = (i + 1).ToString();
                teams[i] = new Team
                {
                    Id = Guid.Parse("b" + emptyGuid[1..^indexStr.Length] + indexStr),
                    UserId = users[i].Id,
                    Name = $"team{i + 1}",
                    Description = $"team-description{i + 1}",
                };
                var max = getRand(0, 6);
                var playerList = players.ToList();
                for (int j = 0; j < max; j++)
                {
                    Player player;
                    if (j == 0)
                    {
                        player = playerList.Find(p => p.UserId == teams[i].UserId);

                    }
                    else
                    {
                        player = playerList[getRand(0, playerList.Count)];
                    }
                    playerList.Remove(player);
                    teams[i].TeamPlayers.Add(new TeamPlayer()
                    {
                        PlayerId = player.Id,
                        TeamId = teams[i].Id,
                        UserId = player.UserId,
                        PositionId = j + 1,
                    });
                }
            }
            foreach (var team in teams)
            {
                team.PlayerCount = team.TeamPlayers.Count;
                context.Add(team);
            }

            await context.SaveChangesAsync();
        }
    }
}
