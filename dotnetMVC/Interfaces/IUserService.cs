using dotnetMVC.Models;

namespace dotnetMVC.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        //Task<User?> RegisterAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
