using System;
using System.Linq;
using Interfaces;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ChatServerApi.Controllers
{
    [Route("api/chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private ILoggerManager _logger;
        private IChatRepository _repository;

        public ChatController(ILoggerManager logger, IChatRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid receiverId)
        {
            try
            {
                var sender = (User)HttpContext.Items["User"];

                Result result = await _repository.Get(sender.UserId, receiverId);

                _logger.LogInfo($"Returned all owners from database.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Chat value)
        {
            try
            {
                Result result = await _repository.Save(value);
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Chat value)
        {
            try
            {
                Result result = await _repository.Update(value);
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                Result result = await _repository.Delete(id);
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}