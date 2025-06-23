namespace CareerSim.Models.ViewModels
{
    public class CareerResultViewModel
    {
        public int AnswerId { get; set; } // CareerAnswer.Id
        public int TaskId { get; set; }   // CareerTask.Id → AI değerlendirme için kullanılacak

        public string CareerTitle { get; set; } = string.Empty;
        public bool HasPremiumEvaluation { get; set; }
        public string? FeedbackText { get; set; }

        public string? UserAnswer { get; set; }
        public DateTime? SubmittedAt { get; set; }

        public string? TaskDescription { get; set; }
        public string? TaskTitle { get; set; }       // (İsteğe bağlı) Görev başlığını da gösterebilirsin
        public string? SampleAnswer { get; set; }    // (İsteğe bağlı) Örnek cevap AI prompt için
    }
}
