namespace CareerSim.Services.IServices
{
    public interface IActivityLogService
    {
        Task LogAsync(string userId, string action, string? ip = null, string? browser = null);
    }
}
