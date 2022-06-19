using Ahoy_Data.Data;
using Ahoy_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ahoy_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AhoyContext _context;

        public ReviewController(AhoyContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUser(Review reviewinfo)
        {
            await _context.Reviews.AddAsync(reviewinfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(null, new
            {
                ReviewId = reviewinfo.ReviewId,
                HotelId = reviewinfo.HotelId,
                UserId = reviewinfo.UserId,
                Description = reviewinfo.Description
            });
        }
    }
}
