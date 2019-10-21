using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBling.Data;
using GameBling.Data.Entities;
using GameBling.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GameBling.Controllers
{
    [Route("api/[Controller]")]
    public class CardsController : Controller
    {

        private readonly IGameRepository _repository;
        private readonly ILogger<UsersController> _logger;
        private readonly GameContext _ctx;
        private readonly UserManager<User> _userManager;

        public CardsController(IGameRepository repository, ILogger<UsersController> logger, UserManager<User> userManager, GameContext ctx)
        {
            _repository = repository;
            _logger = logger;
            _ctx = ctx;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            try
            {
                return Ok(_repository.GetCards());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get users: {ex}");
                return BadRequest("Failed to get users");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] CardViewModel model)
        {
            if (ModelState.IsValid)
            {

                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string cardNumberHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.CardNumber,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                var card = new Card
                {
                    CardNumber = cardNumberHashed,
                    ExpirationDate = DateTime.Parse(model.ExpirationDate),
                    CVC = Int32.Parse(model.CVC),
                    CardHolderName = model.CardHolderName,
                    User = await _userManager.FindByNameAsync(model.Username)
                };

                bool result = _repository.AddCard(card);

                bool result2 = _repository.SaveAll();

                if (result & result2)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            return View();
            
        }

    }
}