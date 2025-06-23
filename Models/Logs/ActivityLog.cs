namespace CareerSim.Models.Logs
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Action { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? IpAddress { get; set; }

        public string? BrowserInfo { get; set; }
    }
}
