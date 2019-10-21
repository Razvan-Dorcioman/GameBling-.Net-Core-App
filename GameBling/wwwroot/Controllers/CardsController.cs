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

namespace GameBling.Controllers
{
    [Route("api/[Controller]")]
    public class CardsController : Controller
    {

        private readonly IGameRepository _repository;
        private readonly ILogger<UsersController> _logger;
        private readonly GameContext _ctx;

        public CardsController(IGameRepository repository, ILogger<UsersController> logger, UserManager<User> userManager, GameContext ctx)
        {
            _repository = repository;
            _logger = logger;
            _ctx = ctx;
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
        [Route("/api/cards/addcard")]
        public async Task<IActionResult> AddCard([FromBody]CardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var card = new Card
                {
                    CardNumber = model.CardNumber,
                    ExpirationDate = model.ExpirationDate,
                    CVC = model.CVC,
                    CardHolderName = model.CardHolderName
                };

                await _ctx.SaveChangesAsync();
            }
            return View();
            //return _repository.AddCard(Card);
        }

    }
}