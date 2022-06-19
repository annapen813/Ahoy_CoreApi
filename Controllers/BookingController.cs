using Ahoy_Data.Data;
using Ahoy_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ahoy_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AhoyContext _context;

        public BookingController(AhoyContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public IEnumerable<BookingInfo> GetBookingInfos()
        {
            return _context.BookingInfos.ToList();
        }

        [HttpGet("userid")]
        [ProducesResponseType(typeof(BookingInfo), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBookingInfosPerUser(int userid)
        {
            var user = _context.Users.Where(x => x.UserId == userid && x.Status == true).FirstOrDefault();
            var bookingInfos = _context.BookingInfos.Where(x => x.UserId == user.UserId).ToList();
            return bookingInfos == null ? NotFound() : Ok(bookingInfos);
        }

        [HttpGet("hotelid")]
        [ProducesResponseType(typeof(BookingInfo), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBookingInfosPerHotel(int hotelid)
        {
            var bookingInfos = _context.BookingInfos.Where(x => x.HotelId == hotelid).ToList();
            return bookingInfos == null ? NotFound() : Ok(bookingInfos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUser(BookingInfo bookinginfo)
        {
            await _context.BookingInfos.AddAsync(bookinginfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingInfos), new
            {
                BookingInfoId = bookinginfo.BookingInfoId,
                HotelId = bookinginfo.HotelId,
                UserId = bookinginfo.UserId,
                CheckinDate = bookinginfo.CheckinDate,
                CheckoutDate = bookinginfo.CheckoutDate,
                TotalDays = (bookinginfo.CheckoutDate - bookinginfo.CheckinDate).TotalDays
            });
        }
    }
}
