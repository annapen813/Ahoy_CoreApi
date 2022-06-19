using Ahoy_Data.Data;
using Ahoy_Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ahoy_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly AhoyContext _context;

        public HotelController(AhoyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Hotel> GetHotels()
        {
            return _context.Hotels.Where(x => x.TotalRooms > 0 && x.Status == true).ToList();
        }

        [HttpGet("hotelid")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHotelById(int id)
        {
            var hotel = _context.Hotels.Where(x => x.HotelId.Equals(id) && x.TotalRooms > 0 && x.Status == true).FirstOrDefault();
            return hotel != null ? Ok(hotel) : NotFound();
        }

        [HttpGet("byname")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHotelByName(string name)
        {
            var hotels = _context.Hotels.Where(x => x.HotelName.Contains(name) && x.TotalRooms > 0 && x.Status == true).ToList();
            return hotels.Any() ? Ok(hotels) : NotFound();
        }

        [HttpGet("byrating")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHotelByStar(string starrate)
        {
            var hotels = _context.Hotels.Where(x => x.StarRate.ToString().Contains(starrate) && x.TotalRooms > 0 && x.Status == true).ToList();
            return hotels.Any() ? Ok(hotels) : NotFound();
        }

        [HttpGet("byfacilities")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHotelByFacility(string facilities)
        {
            var hotels = _context.Hotels.Where(x => x.Facilities.Contains(facilities) && x.TotalRooms > 0 && x.Status == true).ToList();
            return hotels.Any() ? Ok(hotels) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateHotel(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotels), new
            {
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                Latitude = hotel.Latitude,
                Longitude = hotel.Longitude,
                Rate = hotel.RatePerNight,
                StartRate = hotel.StarRate,
                Address = hotel.Address,
                Description = hotel.Description,
                TotalRooms =  hotel.TotalRooms,
                Status = hotel.Status,
                CreateDate = DateTime.Now
            });
        }
    }
}
