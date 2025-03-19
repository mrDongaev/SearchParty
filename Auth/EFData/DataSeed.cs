using Domain;
using Microsoft.AspNetCore.Identity;
using WebSearchPartyApi;

namespace EFData
{
    public class DataSeed
    {
        public static async Task SeedDataAsync(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                AppUser[] users = new AppUser[100];
                string emptyGuid = Guid.Empty.ToString();
                for (int i = 0; i < users.Length; i++)
                {
                    string indexStr = (i + 1).ToString();
                    users[i] = new AppUser
                    {
                        Id = ("a" + emptyGuid[1..^indexStr.Length] + indexStr),
                        DisplayName = $"user{i + 1}",
                        UserName = $"user{i + 1}",
                        Email = $"user{i + 1}@test.com",
                    };
                }
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Qwe123@");
                }
            }
        }
    }
}
