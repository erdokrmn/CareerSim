namespace CareerSim.Models.Logs
{
    public class ErrorLog
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public string? StackTrace { get; set; }

        public string? Path { get; set; }

        public string? UserId { get; set; }

        public string? BrowserInfo { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
