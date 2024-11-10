﻿using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSearchPartyApi;

namespace EFData
{
    public class DataSeed
    {
        public static async Task SeedDataAsync(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                            {
                                new AppUser
                                    {
                                        DisplayName = "TestUserFirst",

                                        UserName = "TestUserFirst",

                                        Email = "testuserfirst@test.com"
                                    },

                                new AppUser
                                    {
                                        DisplayName = "TestUserSecond",

                                        UserName = "TestUserSecond",

                                        Email = "testusersecond@test.com"
                                    }
                              };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "qazwsX123@");
                }
            }
        }
    }
}
