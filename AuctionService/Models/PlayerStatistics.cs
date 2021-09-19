using AuctionService.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Models
{
    public class PlayerStatistics : Player, IInflatable
    {
        public string MatchType { get; set; }
        
        public int Matches { get; set; }

        public int BattingInnings { get; set; }

        public long BattingRuns { get; set; }

        public float BattingAverage { get; set; }

        public float BattingStrikerate { get; set; }

        public long BallsFaced { get; set; }

        public int NotOuts { get; set; }

        public int HighScore { get; set; }

        public int Hundreds { get; set; }

        public int DoubleHundreds { get; set; }

        public int Fifties { get; set; }

        public int Sixes { get; set; }

        public int Fours { get; set; }

        public int BowlingInnings { get; set; }

        public long BallsBowled { get; set; }

        public long BowlingRuns { get; set; }

        public float BowlingAverage { get; set; }

        public float BowlingStrikerate { get; set; }

        public int Wickets { get; set; }

        public float Economy { get; set; }

        public int FiveFors { get; set; }

        public int TenFors { get; set; }

        public string BBI { get; set; }

        public string BBM { get; set; }

        public void Inflate(IDataReader reader)
        {
            NullableDataReader ndr = new NullableDataReader(reader);
            PlayerID = ndr.GetInt32("PlayerID");
            MatchType = ndr.GetNullableString("MatchType");
            Matches = ndr.GetInt32("Matches");
            BattingInnings = ndr.GetInt32("InningsBatted");
            BattingRuns = ndr.GetInt64("Runs");
            NotOuts = ndr.GetInt32("NotOuts");
            BattingAverage = ndr.GetFloat("BattingAverage");
            BattingStrikerate = ndr.GetFloat("BattingStrikeRate");
            BallsFaced = ndr.GetInt64("BallsFaced");
            HighScore = ndr.GetInt32("HighScore");
            Hundreds = ndr.GetInt32("100s");
            DoubleHundreds = ndr.GetInt32("200s");
            Fifties = ndr.GetInt32("50s");
            Sixes = ndr.GetInt32("6s");
            Fours = ndr.GetInt32("4s");
            BowlingInnings = ndr.GetInt32("InningsBowled");
            BallsBowled = ndr.GetInt64("BallsBowled");
            BowlingRuns = ndr.GetInt64("Runsgiven");
            BowlingAverage = ndr.GetFloat("BowlingAverage");
            BowlingStrikerate = ndr.GetFloat("BowlingStrikeRate");
            Wickets = ndr.GetInt32("Wickets");
            Economy = ndr.GetInt32("Economy");
            BBI = ndr.GetNullableString("BBI");
            BBM = ndr.GetNullableString("BBM");
            FiveFors = ndr.GetInt32("5W");
            TenFors = ndr.GetInt32("10W");
        }
    }
}
