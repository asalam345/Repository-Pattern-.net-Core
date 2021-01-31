using System;
using System.Linq;
using Interfaces;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System.Net.Http;

namespace ChatServerApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILoggerManager _logger;
        private IUserRepository _repository;

        public UserController(ILoggerManager logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
			try
			{
                Result result = await _repository.Authenticate(model);

                if (result == null)
                    return BadRequest(new { message = "Email Address is incorrect" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
			try
			{
                Result result = await _repository.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }

                Result result = await _repository.CreateUser(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<bool> Logoff()
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpContext.Session.Clear();
                });
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
       
    }
}