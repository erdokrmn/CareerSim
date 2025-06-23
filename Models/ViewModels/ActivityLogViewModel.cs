namespace CareerSim.Models.ViewModels
{
    public class ActivityLogViewModel
    {
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? BrowserInfo { get; set; }
    }
}
