using Common.Models.Enums;
using Service.Contracts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Team
{
    public static class TeamPlayerService
    {
        public sealed class Write
        {
            public PositionName Position { get; set; }

            public Guid PlayerId { get; set; }
        }

        public sealed class Read
        {
            public PositionName Position { get; set; }

            public PlayerDto Player { get; set; }
        }
    }
}
