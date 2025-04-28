using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingAPI.Controllers
{
    [Route("api/users/{userId}/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUserService _userService;

        public BookingController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            var bookings = await _userService.GetBookingsByUserId(userId);
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(int userId, [FromBody] Booking booking)
        {
            try
            {
                var createdBooking = await _userService.AddBookingToUser(userId, booking);
                return CreatedAtAction(nameof(GetBookingsByUserId), new { userId }, createdBooking);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var isCancelled = await _userService.CancelBooking(bookingId);
            if (!isCancelled)
                return NotFound();

            return NoContent();
        }
    }
}
