using CareerSim.Models.Question.Enum;
using System.ComponentModel.DataAnnotations;

namespace CareerSim.Models.Question
{
    public class ClassicQuestion
    {
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string ExpectedAnswer { get; set; }

        public string? ImagePath { get; set; } // Nullable: SVG veya görsel olmayabilir

        public QuestionFormat QuestionFormat { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        // Foreign Key
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
