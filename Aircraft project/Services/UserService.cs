using Aircraft_project.Data;
using Aircraft_project.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft_project.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationResult> RegisterUserAsync(Users user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
            {
                return new RegistrationResult { Success = false, Errors = new List<string> { "Passwords do not match." } };
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return new RegistrationResult { Success = false, Errors = new List<string> { "Email already in use." } };
            }

            user.Password = HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegistrationResult { Success = true };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
