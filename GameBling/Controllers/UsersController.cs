using System;
using GameBling.Data;
using GameBling.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GameBling.Controllers
{
    [Route("api/[Controller]")]
    public class UsersController : Controller
    {
        private readonly IGameRepository _repository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IGameRepository repository, ILogger<UsersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("api/users")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get users: {ex}");
                return BadRequest("Failed to get users");
            }
        }

        [HttpGet]
        [Route("getUserById/{id}")]
        public IActionResult GetUserbyId([FromRoute] string id)
        {
            try
            {
                return Ok(_repository.GetUserById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by id: {ex}");
                return null;
            }
        }

        [HttpPost]
        [Route("updateUser")]
        public bool updateUser([FromBody] User user)
        {
            try
            {
                _repository.updateUser(user);
                _repository.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by id: {ex}");
                return false;
            }
        }

        [HttpGet]
        [Route("funds/{id}/{balance}")]
        public bool UpdateFunds([FromRoute] string id, [FromRoute] int balance)
        {
            try
            {
                if (_repository.UpdateFunds(id, balance) == false)
                    return false;
                _repository.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by id: {ex}");
                return false;
            }
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public bool deleteUser([FromRoute] string id)
        {
            try
            {
                _repository.deleteUser(id);
                _repository.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by id: {ex}");
                return false;
            }
        }

    }

}
