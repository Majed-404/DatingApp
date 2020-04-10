using System.Threading.Tasks;
using DatingApp.API.Core.Interfaces;
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
        public async Task<IActionResult> Redister(string _userName, string _password)
        {
            // validate request ..


            _userName = _userName.ToLower();

            if(await _authRepo.UserExists(_userName))
                return BadRequest("Username already exists.");

            var _newUser = new User
            {
                UserName = _userName
            };

            var _createdUser = await _authRepo.Register(_newUser, _password);

            return StatusCode(201);
        }
    }
}