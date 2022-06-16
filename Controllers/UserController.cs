using Ahoy_Data.Data;
using Ahoy_Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ahoy_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AhoyContext _context;

        public UserController(AhoyContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public IEnumerable<UserInfo> GetUsers()
        {
            return _context.Users.ToList();
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.Where(x => x.UserId == id && x.Status == true).FirstOrDefault();
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser(UserInfo user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email,
                Status = true,
                CreateDate = DateTime.Now
            });
        }
    }
}
