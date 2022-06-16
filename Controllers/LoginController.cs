using Ahoy_Data.Data;
using Ahoy_Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ahoy_CoreApi.Helper;

namespace Ahoy_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AhoyContext _context;
        private readonly JwtSettings _jwtSettings;


        public LoginController(AhoyContext context, JwtSettings jwtSetting)
        {
            _context = context;
            _jwtSettings = jwtSetting;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(Login userinfo)
        {
            var Token = new UserToken();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userinfo.Username);
            if (user != null && user.Password == userinfo.Password)
            {
                Token = JwtHelpers.GenTokenkey(new UserToken()
                {
                    EmailId = user.Email,
                    GuidId = Guid.NewGuid(),
                    UserName = user.UserName,
                    Id = user.UserId,
                }, _jwtSettings);

                return Ok(new
                {
                    token = Token.Token,
                    expiration = Token.ExpiredTime
                });
            }
            return Ok(new Response { Status = StatusCodes.Status400BadRequest, Message = "User Name Or Password is invalid." });
        }
    }
}
