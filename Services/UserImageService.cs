using CareerSim.Models;
using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class UserImageService : IUserImageService
{
    private const string DefaultImagePath = "/images/default/user.png";

    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserImageService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetProfileImageUrlAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || !httpContext.User.Identity?.IsAuthenticated == true)
            return DefaultImagePath;

        var user = await _userManager.GetUserAsync(httpContext.User);

        if (user == null || string.IsNullOrEmpty(user.ProfileImagePath))
            return DefaultImagePath;

        return "/uploads/profile/" + user.ProfileImagePath;

    }
}
