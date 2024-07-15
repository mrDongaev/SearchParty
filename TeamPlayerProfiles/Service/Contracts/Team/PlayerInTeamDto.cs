using Service.Contracts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Team
{
    public class PlayerInTeamDto
    {
        public string Position { get; set; }

        public PlayerDto Player { get; set; }
    }
}
