using CareerSim.Data;
using CareerSim.Models.Logs;
using CareerSim.Services.IServices;

public class ErrorLogService : IErrorLogService
{
    private readonly AppDbContext _context;

    public ErrorLogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(Exception ex, string? path, string? userId, string? browser)
    {
        var log = new ErrorLog
        {
            Message = ex.Message,
            StackTrace = ex.StackTrace,
            Path = path,
            UserId = userId,
            BrowserInfo = browser
        };

        _context.ErrorLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
