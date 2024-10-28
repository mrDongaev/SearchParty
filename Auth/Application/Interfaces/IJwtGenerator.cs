﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.User.Settings;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
         UserToken CreateJwtToken(AppUser user);
    }
}
