using CareerSim.Models;
using Microsoft.AspNetCore.Identity;

namespace CareerSim.Services.IServices
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(string userName, string email, string password);
        Task<SignInResult> LoginAsync(string userName, string password, bool rememberMe);
        Task LogoutAsync();
        Task<IdentityResult> AssignRoleAsync(User user, string role);
    }
}
