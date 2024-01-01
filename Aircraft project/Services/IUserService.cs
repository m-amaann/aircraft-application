using Aircraft_project.Models;

namespace Aircraft_project.Services
{
    public interface IUserService
    {
        Task<RegistrationResult> RegisterUserAsync(Users user, string confirmPassword);
        Task<Users> AuthenticateUserAsync(string email, string password);
    }
}
