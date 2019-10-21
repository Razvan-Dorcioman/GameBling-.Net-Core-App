using GameBling.Data;
using GameBling.Data.Entities;
using GameBling.Hubs;
using GameBling.Hubs.DataStorage;
using GameBling.Hubs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : Controller
    {
        private IHubContext<EchoHub> _hub;
        private readonly UserManager<User> _userManager;
        static private IGameRepository _repository;

        public RouletteController(IHubContext<EchoHub> hub, UserManager<User> userManager, IGameRepository repository)
        {
            _hub = hub;
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet]
        [Route("getInitial/{id}")]
        public async Task<IActionResult> GetInitial([FromRoute] string id)
        {
            RouletteTicker rt = new RouletteTicker(_hub, _userManager, _repository);

            // Invoke signal to specified client
            if (id != null)
            {
                await _hub.Clients.Client(id).SendAsync("broadcastbetdata", DataManager.GetBets());
                return Ok(new { Message = "Request Completed" });
            }

            return Ok(new { Message = "Bad Request" });
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Post(IEnumerable<BetModel> betModels)
        {
            // generate value
            Random random = new Random();
            int theChosenOne = random.Next(2); // 0 or 1

            // dummy only one value
            // theChosenOne = 0;
            // System.Diagnostics.Debug.WriteLine(theChosenOne.ToString());

            if (betModels.ToList().Count != 0)
            {
                // dummy check
                /*
                foreach (BetModel bm in betModels)
                {
                    System.Diagnostics.Debug.WriteLine(bm.Username.ToString());
                    System.Diagnostics.Debug.WriteLine(bm.Value.ToString());
                    System.Diagnostics.Debug.WriteLine(bm.Amount.ToString());
                }
                */

                List<string> userNames = new List<string>();
                
                betModels.ToList().ForEach(bm =>
                {
                    userNames.Add(bm.Username.ToLower());
                });

                IEnumerable<User> userModels = _repository.GetBetUsers(userNames);

                List<User> umUpdated = new List<User>();

                userModels.ToList().ForEach(user =>
                {
                    int value = betModels.ToList().Find(x => x.Username.ToLower().Equals(user.UserName.ToLower())).Value;
                    int amount = betModels.ToList().Find(x => x.Username.ToLower().Equals(user.UserName.ToLower())).Amount;
                    
                    if(value == theChosenOne)
                    {
                        user.Balance += amount;
                    } else
                    {
                        user.Balance -= amount;
                    }
                    umUpdated.Add(user);
                });

                _repository.UpdateBetUsers(umUpdated);
                _repository.SaveAll();
            }

            // broadcast reset to all players, can bet
            //string json = JsonConvert.SerializeObject(new { canBet = true, value = theChosenOne });
            await _hub.Clients.All.SendAsync("broadcastcanbet", new { canBet = true, value = theChosenOne });

            return Ok(new { Message = "Request Completed" });
        }

    }
}