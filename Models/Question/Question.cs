using CareerSim.Models.Question.Enum;

namespace CareerSim.Models.Question
{
    public class Question
    {
        public int Id { get; set; }
        public QuestionType QuestionType { get; set; }

        // Navigation Properties (her biri one-to-one)
        public MultipleChoiceQuestion MultipleChoiceDetail { get; set; }
        public ClassicQuestion ClassicDetail { get; set; }
        // Diğer tipler eklenebilir: MatrixDetail, MemoryDetail vb.
    }
}
