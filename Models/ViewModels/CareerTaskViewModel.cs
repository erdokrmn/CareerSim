namespace CareerSim.Models.ViewModels
{
    public class CareerTaskViewModel
    {
        public int CareerId { get; set; }
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public string AnswerText { get; set; }
        public bool IsCompleted { get; set; }           // Görev kalmadı mı?
        public string? LastAnswerText { get; set; }     // En son verdiği cevap
        public string? CareerTitle { get; set; }        // Kariyer başlığı
        public int AnswerId { get; set; }

    }

}
