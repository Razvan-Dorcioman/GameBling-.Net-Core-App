using System.Collections.Generic;
using GameBling.Data.Entities;

namespace GameBling.Data
{
    public interface IGameRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        bool updateUser(User user);
        bool deleteUser(string id);
        IEnumerable<User> GetBetUsers(IEnumerable<string> userNames);
        bool UpdateFunds(string id, int balance);
        int UpdateBetUsers(IEnumerable<User> users);
        IEnumerable<User> GetBots();
        IEnumerable<Card> GetCards();
        bool AddCard(Card card);
        bool SaveAll();
    }
}