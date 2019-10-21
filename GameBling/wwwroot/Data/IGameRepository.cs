using System.Collections.Generic;
using GameBling.Data.Entities;

namespace GameBling.Data
{
    public interface IGameRepository
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetBetUsers(IEnumerable<string> userNames);
        IEnumerable<User> GetBots();
        IEnumerable<Card> GetCards();
        int AddCard(Card card);
        bool SaveAll();
    }
}