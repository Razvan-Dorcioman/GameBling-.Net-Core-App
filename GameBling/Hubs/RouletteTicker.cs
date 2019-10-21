using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameBling.Data;
using GameBling.Data.Entities;
using GameBling.Hubs.DataStorage;
using GameBling.Hubs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace GameBling.Hubs
{
    public class RouletteTicker
    {
        private IHubContext<EchoHub> _hub;
        private UserManager<User> _userManager;
        private readonly IGameRepository _repository;

        static private Timer _timer = null;
        private int count = 1500;
        private bool wait = false;
        private float wait_count = 500;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(10);

        public RouletteTicker(IHubContext<EchoHub> hub, UserManager<User> userManager, IGameRepository repository)
        {
            _hub = hub;
            _userManager = userManager;
            _repository = repository;

            if (_timer == null)  _timer = new Timer(Execute, null, _updateInterval, _updateInterval);
        }

        private async Task CheckAndResetAsync()
        {
            // send all bets to post in roulette controllers

            // url for api post
            string url = "https://gamebling.azurewebsites.net/api/roulette/update";
            //string url = "http://localhost:61031/api/roulette/update";

            // convert bets to json
            string json = JsonConvert.SerializeObject(DataManager.GetBets());

            // clean bets on server
            DataManager.CleanBets();

            // broadcast reset to all players, can bet
            //await _hub.Clients.All.SendAsync("broadcastcanbet", true);

            // make post request
            using (HttpClient client = new HttpClient())
            {
                await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            }
        }

        public void Execute(object stateInfo)
        {
            // send timer
            if (wait == false)
            {
                if (count - 1 > 0)
                {
                    count -= 1;
                }
                else
                {
                    wait = true;
                    count = 0;
                    // broadcast all players, can NOT bet
                    _hub.Clients.All.SendAsync("broadcastcanbet", false);
                }
                _hub.Clients.All.SendAsync("timer", count);
            }
            else
            {
                if(wait_count > 0)
                {
                    wait_count -= 1;
                } else
                {
                    wait = false;
                    wait_count = 500;
                    count = 1500;

                    CheckAndResetAsync();
                }
            }
        }
    }
}
