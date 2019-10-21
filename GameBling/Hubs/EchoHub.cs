using GameBling.Hubs.DataStorage;
using GameBling.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameBling.Hubs
{
    public class EchoHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public void Echo(BetModel data) {
            // add bet to current list of bets for this session
            DataManager.AddBet(data);
            // broadcast updated list to all players
            Clients.All.SendAsync("broadcastbetdata", DataManager.GetBets());
        }
    }
}
