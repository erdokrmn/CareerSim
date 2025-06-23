using CareerSim.Models.Question.Enum;
using System.ComponentModel.DataAnnotations;

namespace CareerSim.Models.Question
{
    public class MultipleChoiceQuestion
    {
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? OptionE { get; set; }

        [Required]
        public string CorrectOption { get; set; }

        public string? ImagePath { get; set; } // Nullable

        public QuestionFormat QuestionFormat { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        // Foreign Key
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }

}
