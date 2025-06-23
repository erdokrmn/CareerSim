using CareerSim.Models.Question.Enum;

namespace CareerSim.Models.ViewModels
{
    public class MatrixQuestionInputModel
    {
        public string QuestionMatrix { get; set; }
        public List<string> OptionMatrices { get; set; }

        public string QuestionText { get; set; }
        public string CorrectOption { get; set; }
        public QuestionFormat QuestionFormat { get; set; }
        public DifficultyLevel Difficulty { get; set; }
    }
}
