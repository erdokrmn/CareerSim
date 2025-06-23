namespace CareerSim.Models.Career
{
    public class UserCareerProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CareerId { get; set; }
        public int CurrentTaskOrder { get; set; }  // Görev sırası (1-5)
        public bool IsCompleted { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

}
