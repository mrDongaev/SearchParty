using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class TeamCountOverflowException(uint maxTeamCount): Exception($"Team cannot have more than {maxTeamCount} members")
    {
    }
}
