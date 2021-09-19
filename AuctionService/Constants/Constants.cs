using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Providers
{
    public class Constants
    {
        #region ConnectionConstants

        public static string DataSource = "DataSource";
        public static string UserID = "UserID";
        public static string Password = "Password";
        public static string Database = "Database";
        public static string ConnectionTimeout = "ConnectionTimeout";

        #endregion

        #region SQL Queries

        public static string NextPlayerQuery = "";
        public static string RemainingPlayersQuery = "";
        public static string SoldPlayersQuery = "";
        public static string SellPlayerQuery = "";
        public static string DeleteSoldPlayerQuery = "";

        #endregion
    }
}
