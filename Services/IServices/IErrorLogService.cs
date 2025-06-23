namespace CareerSim.Services.IServices
{
    public interface IErrorLogService
    {
        Task LogAsync(Exception ex, string? path, string? userId, string? browser);
    }
}
