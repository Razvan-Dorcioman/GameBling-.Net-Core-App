using GameBling.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.Data
{
    public class GameSeeder
    {
        private readonly GameContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<User> _userManager;

        public GameSeeder(GameContext ctx, IHostingEnvironment hosting, UserManager<User> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            if(!_ctx.Cards.Any())
            {
                var cards = _ctx.Cards.Add(new Card()
                {
                    Id = 1,
                    CardNumber = "2345 2344 2134 4213",
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    CVC = 200,
                    CardHolderName = "BeeDoneTeam",
                    User = await _userManager.FindByNameAsync("BeeDone")
                });
                //_ctx.Cards.AddRange(cards);

                _ctx.SaveChanges();

            }

            User user = await _userManager.FindByEmailAsync("beedone@gmail.com");
            if(user == null)
            {
                user = new User()
                {
                    UserName = "BeeDone",
                    Email = "beedone@gmail.com",
                    Bot = false,
                    Admin = true
                };

                var result = await _userManager.CreateAsync(user, "123!@#qweQWE");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder!");
                }
            }
        }
    }
}
