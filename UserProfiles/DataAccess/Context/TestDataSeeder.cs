using DataAccess.Entities;

namespace DataAccess.Context
{
    public static class TestDataSeeder
    {
        public static async Task SeedTestData(UserProfilesContext context)
        {
            Random random = new Random();
            var getRand = (int a, int b) => random.Next(a, b);
            User[] users = new User[20];
            string emptyGuid = Guid.Empty.ToString();
            for (int i = 0; i < users.Length; i++)
            {
                string indexStr = (i + 1).ToString();
                users[i] = new User
                {
                    Id = Guid.Parse(emptyGuid[..^indexStr.Length] + indexStr),
                    Name = $"user{i + 1}",
                    Description = $"user-description{i + 1}",
                    SteamFriendCode = getRand(1000000, 1000000000).ToString(),
                    DiscordName = $"user{i + 1}",
                    TelegramLink = $"@user{i + 1}",
                };
            }
            foreach (var user in users)
            {
                context.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}
