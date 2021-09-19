using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Models
{
    public class PlayerStatus : Player, IInflatable
    {
        public bool IsAuctioned { get; set; }

        public bool IsSold { get; set; }

        public string AssignedTeamID { get; set; }

        public string AssignedTeam { get; set; }

        public string BaseValue { get; set; }

        public string SoldValue { get; set; }
    }
}
