using IdentityAPI.Commands;
using IdentityAPI.Repository;
using IdentityAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] UserGenerateTokenCommand userGenerateTokenCommand)
        {
            var token = await _identityService.GenerateToken(userGenerateTokenCommand);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterCommand userRegisterCommand)
        {
            try
            {
                var user = await _identityService.RegisterUser(userRegisterCommand);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
