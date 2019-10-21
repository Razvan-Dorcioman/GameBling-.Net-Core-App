using GameBling.Data.Entities;
using Microsoft.EntityFrameworkCore;
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

        public User GetUserById(string id)
        {
            return _ctx.Users
                .FirstOrDefault(u => u.Id.Equals(id));
        }

        public bool updateUser(User user)
        {
            try
            {
                _ctx.Update(user);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
                throw;
            }
        }

        public bool UpdateFunds(string id, int balance)
        {
            try
            {
                User user = this.GetUserById(id);
                if (((-1) * user.Balance) < balance)
                {
                    user.Balance += balance;
                    _ctx.Users
                    .Update(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbUpdateException)
            {
                //This either returns a error string, or null if it can’t handle that error
                return false;
                throw; //couldn’t handle that error, so rethrow
            }
        }

        public bool deleteUser(string id)
        {
            try
            {
                User user = this.GetUserById(id);
                _ctx.Remove(user);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<User> GetBetUsers(IEnumerable<string> userNames)
        {
            return _ctx.Users
                .Where(p => userNames.Contains(p.UserName.ToLower()))
                .ToList();
        }

        public int UpdateBetUsers(IEnumerable<User> users)
        {
            try
            {
                _ctx.UpdateRange(users);
                return 1;
            }
            catch
            {
                throw;
            }
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


        public bool AddCard(Card card)
        {
            try
            {
                _ctx.Cards.Add(card);
                return true;
            }
            catch (DbUpdateException)
            {
                //This either returns a error string, or null if it can’t handle that error
                return false;
                throw; //couldn’t handle that error, so rethrow
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

    }

}
