using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class TeamPositionOverlapException(): Exception("Team cannot have multiple player profiles in the same position within the team")
    {
    }
}
