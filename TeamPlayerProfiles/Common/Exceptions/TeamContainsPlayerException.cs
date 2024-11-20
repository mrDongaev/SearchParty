using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class TeamContainsPlayerException() : Exception("Team already contains the applying player profile")
    {
    }
}
