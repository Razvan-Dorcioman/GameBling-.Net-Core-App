using GameBling.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly GameContext _ctx;

        public GameRepository(GameContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _ctx.Users
                .OrderBy(p => p.UserName)
                .ToList();
        }

        public IEnumerable<User> GetBetUsers(IEnumerable<string> userNames)
        {
            return _ctx.Users
                .Where(p => userNames.Contains(p.UserName))
                .ToList();
        }

        public IEnumerable<User> GetBots()
        {
            return _ctx.Users
                .Where(p => p.Bot == true)
                .ToList();
        }

        public IEnumerable<Card> GetCards()
        {
            return _ctx.Cards
                .OrderBy(p => p.CardNumber)
                .ToList();
        }

        public int AddCard(Card card)
        {
            try
            {
                _ctx.Cards.Add(card);
                _ctx.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

    }

}
