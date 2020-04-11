using System.Threading.Tasks;
using DatingApp.API.Core.IRepository;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;

        }


        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {

            userDto.UserName = userDto.UserName.ToLower();

            if(await _authRepo.UserExists(userDto.UserName))
                return BadRequest("Username already exists.");

            var _newUser = new User
            {
                UserName = userDto.UserName
            };

            var _createdUser = await _authRepo.Register(_newUser, userDto.Password);

            return StatusCode(201);
        }
    }
}