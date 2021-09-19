using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Models;
using AuctionService.Providers;

namespace AuctionService.Managers
{
    public class PlayersManager : IPlayersManager
    {
        IProvider databaseProvider;

        public PlayersManager(IProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        public async Task<PlayerStatistics> GetNextPlayer()
        {
            try
            {
                PlayerStatistics nextPlayer = await databaseProvider.ExecuteScalarReaderAsync<PlayerStatistics>(Constants.NextPlayerQuery);
                return nextPlayer;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PlayerStatus>> GetSoldPlayers()
        {
            try
            {
                List<PlayerStatus> soldPlayers = await databaseProvider.ExecuteMultiReaderAsync<PlayerStatus>(Constants.SoldPlayersQuery);
                //LOGIC to be added
                return soldPlayers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PlayerStatus>> GetRemainingPlayers()
        {
            try
            {
                List<PlayerStatus> remainingPlayers = await databaseProvider.ExecuteMultiReaderAsync<PlayerStatus>(Constants.RemainingPlayersQuery);
                //Logic to be added
                return remainingPlayers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SellPlayerToTeam(int playerID, int teamID)
        {
            try
            {
                bool result = await databaseProvider.ExecuteNonQueryAsync(string.Format(Constants.SellPlayerQuery, playerID, teamID));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteSoldPlayer(int playerID)
        {
            try
            {
                bool result = await databaseProvider.ExecuteNonQueryAsync(string.Format(Constants.DeleteSoldPlayerQuery, playerID));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
