using GameBling.Hubs.Models;
using System.Collections.Generic;

namespace GameBling.Hubs.DataStorage
{
    public static class DataManager
    {
        public static List<BetModel> data = new List<BetModel> { };

        public static BetModel AddBet(BetModel _bm)
        {
            // normalize username
            //_bm.Username = _bm.Username.ToLower();
            // add bet to session collection
            data.Add(_bm);
            return _bm;
        }

        public static List<BetModel> GetBets()
        {
            return data;
        }

        public static void CleanBets()
        {
            data.Clear();
        }
    }
}
