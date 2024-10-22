﻿using JWT_Auth.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("generateToken")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GenerateToken([FromBody] User user)
        {
            if (user.username == Constants.username && user.password == Constants.password)
            {


                //Generate the JWT token and return

                // Claims
                var claims = new[]
                {
                new Claim("Full Name", "Hasitha Mihiran"), // first add claims. use namespace using System.Security.Claims;
                new Claim(JwtRegisteredClaimNames.Sub,"JWT_Auth_API")
            };

                var keyBytes = Encoding.UTF8.GetBytes(Constants.Secret);

                var key = new SymmetricSecurityKey(keyBytes);

                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    Constants.Audience,
                    Constants.Issuer,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { accessToken = tokenString });
            }
            else
            {                
                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
