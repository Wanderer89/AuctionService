using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Managers;

namespace AuctionService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        IPlayersManager playersManager;

        public PlayersController(IPlayersManager playersManager)
        {
            this.playersManager = playersManager;
        }


        [HttpGet]
        [Route("NextPlayer")]
        public async Task<IActionResult> GetNextplayer()
        {
            var response = await playersManager.GetNextPlayer();
            return Ok(response);
        }

        [HttpGet]
        [Route("SoldPlayers")]
        public async Task<IActionResult> GetSoldPlayers()
        {
            var response = await playersManager.GetSoldPlayers();
            return Ok(response);
        }

        [HttpGet]
        [Route("RemainingPlayers")]
        public async Task<IActionResult> GetRemainingPlayers()
        {
            var response = await playersManager.GetRemainingPlayers();
            return Ok(response);
        }

        [HttpPost]
        [Route("{playerID}/Assign/{teamID}")]
        public async Task<IActionResult> SellPlayerToTeam(int playerID, int teamID)
        {
            var response = await playersManager.SellPlayerToTeam(playerID, teamID);
            return Ok(response);
        }

        [HttpPatch]
        [Route("{playerID}")]
        public async Task<IActionResult> DeleteSoldPlayer(int playerID)
        {
            var response = await playersManager.DeleteSoldPlayer(playerID);
            return Ok(response);
        }
    }
}
