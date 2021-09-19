using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Models;

namespace AuctionService.Managers
{
    public interface IPlayersManager
    {
        Task<PlayerStatistics> GetNextPlayer();
        Task<List<PlayerStatus>> GetSoldPlayers();
        Task<List<PlayerStatus>> GetRemainingPlayers();
        Task<bool> SellPlayerToTeam(int playerID, int teamID);
        Task<bool> DeleteSoldPlayer(int playerID);
    }
}
