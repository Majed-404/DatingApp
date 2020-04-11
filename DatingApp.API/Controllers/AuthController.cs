using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Core.IRepository;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _config = config;
            _authRepo = authRepo;

        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userDto)
        {
            // validate request ..
            if (!ModelState.IsValid)
                return BadRequest(userDto);

            userDto.UserName = userDto.UserName.ToLower();

            if (await _authRepo.UserExists(userDto.UserName))
                return BadRequest("Username already exists.");

            var _newUser = new User
            {
                UserName = userDto.UserName
            };

            var _createdUser = await _authRepo.Register(_newUser, userDto.Password);

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userDto)
        {
            var _userFromRepo = await _authRepo.Login(userDto.Username.ToLower(), userDto.Password);

            if (_userFromRepo == null)
                return Unauthorized();

            //build token..
            var _claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, _userFromRepo.UserName)
            };

            var _key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(_claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = _credentials
            };

            var _tokenHandler = new JwtSecurityTokenHandler();

            var _token = _tokenHandler.CreateToken(_tokenDescriptor);

            return Ok(new { token = _tokenHandler.WriteToken(_token) });

        }
    }
}