﻿using AuthenticationService.DTO;
using AuthenticationService.Entities;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [AllowAnonymous]
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] UserDTO user)
        {
            var userAuthenticated = await _userService.ValidateUser(new User
            {
                Username = user.Username,
                Password = user.Password
            });
            if (userAuthenticated)
            {
                var token = _tokenService.GenerateToken(user.Username);
                return Ok(new { token });
            }
            return BadRequest("Username or password are incorrect.");
        }
    }
}
