using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackOverFlow;
using WebServiceToken.Models;
using WebServiceToken.Services;

namespace WebServiceToken.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;

        public AuthController(IDataService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
            _configuration = configuration;
        }

        [HttpPost("users")]
        public ActionResult CreateUser([FromBody] UserForCreationDto dto)
        {
            if (_dataService.GetUser(dto.UserName) != null)
            {
                return BadRequest();
            }

            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value, 
                out var size);

            if (size == 0)
            {
                throw new ArgumentException();
            }

            var salt = PasswordService.GenerateSalt(size);

            var pwd = PasswordService.HashPassword(dto.Password, salt, size);

            _dataService.CreateUser(dto.UserName, pwd, dto.Email, salt, DateTime.Now);

            return CreatedAtRoute(null, salt);
        }

        [HttpPost("tokens")]
        public ActionResult Login([FromBody] UserForLoginDto dto)
        {
            var user = _dataService.GetUser(dto.UserName);

            if (user == null)
            {
                return BadRequest();
            }

            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value,
                out var size);

            if (size == 0)
            {
                throw new ArgumentException();
            }

            var pwd = PasswordService.HashPassword(dto.Password, user.Salt, size);

            if(user.Password != pwd)
            {
                return BadRequest();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Auth:Key"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.Now.AddSeconds(20),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);

            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new {user.UserName, token});

        }
        
        [HttpPut]
        public ActionResult UpdateUser([FromBody] User dto)
        {
            int.TryParse(HttpContext.User.Identity.Name, out var id);
            if (_dataService.GetUser(dto.UserName) == null)
            {
                return BadRequest();
            }

            int.TryParse(
                _configuration.GetSection("Auth:PwdSize").Value, 
                out var size);

            if (size == 0)
            {
                throw new ArgumentException();
            }

            var salt = PasswordService.GenerateSalt(size);

            var pwd = PasswordService.HashPassword(dto.Password, salt, size);

            var user = _dataService.UpdateUser(dto.UserName, pwd, dto.Email, salt);

            return Ok(salt);
        }

        [HttpDelete("{username}")]
        public ActionResult DeleteUser(string username)
        {
            if (!_dataService.DeleteUser(username))
                return NotFound();
            return Ok();
        }
        
    }
}
