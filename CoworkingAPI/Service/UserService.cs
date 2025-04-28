using CoworkingAPI.DataBase;
using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using CoworkingAPI.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly ContextDB _context;
    private readonly IConfiguration _configuration;

    public UserService(ContextDB context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> Register(User user)
    {
        // Проверка уникальности
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            throw new Exception("Username already exists");

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            throw new Exception("Email already in use");

        // Проверка существования роли
        if (!await _context.Roles.AnyAsync(r => r.Id == user.RoleId))
            throw new Exception("Invalid role");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

        if (user == null)
            throw new Exception("Invalid credentials");

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT secret key is not configured")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> GetUserById(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Bookings)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

   



    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllUsers(Pagination pagination)
    {
        return await _context.Users
            .Include(u => u.Role)
            .OrderBy(u => u.Username)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
    }
    public async Task<bool> CancelBooking(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null)
            return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User> CreateUser(User user, string password)
    {
        if (await UserExists(user.Username))
            throw new Exception("Username already exists");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUser(int id, User user)
    {
        var existingUser = await GetUserById(id);
        if (existingUser == null)
            throw new Exception("User not found");

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.RoleId = user.RoleId;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();

        return existingUser;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await GetUserById(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Booking> AddBookingToUser(int userId, Booking booking)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        booking.UserId = userId;
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking;
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserId(int userId)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Workplace)
            .ToListAsync();
    }
}