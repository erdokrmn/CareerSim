using CareerSim.Models.Career;

public class CareerFeedback
{
    public int Id { get; set; }

    public int CareerAnswerId { get; set; }  // 👈 AI değerlendirmesi hangi cevaba ait
    public CareerAnswer CareerAnswer { get; set; }

    public string FeedbackText { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPremium { get; set; }
}
