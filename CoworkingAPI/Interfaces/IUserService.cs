using CoworkingAPI.Controllers;
using CoworkingAPI.Model;
using CoworkingAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers(Pagination pagination);
        Task<User> CreateUser(User user, string password);
        Task<User> UpdateUser(int id, User user);
        Task<bool> DeleteUser(int id);
        Task<User> Register(User user);
        Task<string> Login(string username, string password);
        Task<Booking> AddBookingToUser(int userId, Booking booking);
        Task<IEnumerable<Booking>> GetBookingsByUserId(int userId);
        Task<bool> CancelBooking(int bookingId);
    }
}
