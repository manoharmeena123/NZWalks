using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate the incoming request
            if (loginRequest.Username == null && loginRequest.Password == null)
            {
                return BadRequest("Please fill the form");
            }

            // check if user is authorized
            var user = await userRepository.AuthenticateAsync(
                loginRequest.Username, loginRequest.Password);
            if (user != null)
            {
                // Generate a Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);

            }
            return BadRequest("Username or Password is incorrect.");
        }
    }
}
