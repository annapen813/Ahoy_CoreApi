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

        [HttpPost]
        public 
    }
}
