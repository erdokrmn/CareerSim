namespace CareerSim.Models.ViewModels
{
    public class ErrorLogViewModel
    {
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
        public string? Path { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }    // 
        public string? FullName { get; set; }    // 

        public string? BrowserInfo { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
