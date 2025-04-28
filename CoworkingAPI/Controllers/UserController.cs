using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using CoworkingAPI.Requests;
using Microsoft.AspNetCore.Authorization;

namespace CoworkingAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] Pagination pagination)
        {
            var users = await _userService.GetAllUsers(pagination);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user, [FromQuery] string password)
        {
            try
            {
                var createdUser = await _userService.CreateUser(user, password);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(id, user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUser(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{userId}/bookingsAdd")]
        public async Task<IActionResult> AddBookingToUser(int userId, [FromBody] Booking booking)
        {
            try
            {
                var createdBooking = await _userService.AddBookingToUser(userId, booking);
                return CreatedAtAction(nameof(GetBookingsByUserId), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{userId}/bookingByID")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            var bookings = await _userService.GetBookingsByUserId(userId);
            return Ok(bookings);
        }
    }
}
