using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Models
{
    public class Player : IInflatable
    {
        public int PlayerID { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public string BattingStyle { get; set; }

        public string BowlingStyle { get; set; }

        public string PlayingRole { get; set; }
    }
}
