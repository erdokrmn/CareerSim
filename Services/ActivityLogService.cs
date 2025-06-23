using CareerSim.Data;
using CareerSim.Models.Logs;
using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class ActivityLogService : IActivityLogService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(string userId, string action, string? ip = null, string? browser = null)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            Timestamp = DateTime.UtcNow,
            IpAddress = ip ?? _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
            BrowserInfo = browser ?? _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString()
        };

        _context.ActivityLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
