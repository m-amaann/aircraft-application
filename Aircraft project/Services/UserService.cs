using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.EntityFrameworkCore;
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


        // Register
        public async Task<RegistrationResult> RegisterUserAsync(Users user, string confirmPassword)
        {
            // Checking a Email already used
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return new RegistrationResult { Success = false, Errors = new List<string> { "Email already in use." } };
            }

            // Verifying password input field
            if (user.Password != confirmPassword)
            {
                //If not match it, this message will shows an error
                return new RegistrationResult { Success = false, Errors = new List<string> { "Passwords do not match." } };
            }

            user.Password = HashPassword(user.Password); //Save as Hashing password in DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegistrationResult { Success = true };
        }




        // Authenticating User Login Function
        public async Task<Users> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(u => u.Email == email && u.Password == HashPassword(password));

            var hashedPassword = HashPassword(password);
            return user;
        }




        //HASH Function
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
