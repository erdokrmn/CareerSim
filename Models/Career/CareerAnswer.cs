namespace CareerSim.Models.Career
{
    public class CareerAnswer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CareerTaskId { get; set; }
        public string AnswerText { get; set; }
        public DateTime SubmittedAt { get; set; }

        public CareerTask CareerTask { get; set; }
    }

}
