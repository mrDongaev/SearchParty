using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class TeamOwnerNotPresentException(): Exception($"Team must contain a player profile, created by its owner")
    {
    }
}
