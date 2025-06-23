using System.Text.Json;
using CareerSim.Data;
using CareerSim.Models.Question;
using CareerSim.Models.Question.Enum;
using CareerSim.Models.ViewModels;
using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerSim.Controllers
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMatrixSvgService _svgService;

        public QuestionController(AppDbContext context, IMatrixSvgService svgService)
        {
            _context = context;
            _svgService = svgService;
        }
        [HttpGet]
        public IActionResult CreateMatrixQuestion()
        {
            var shapes = _context.Shapes.ToList();
            ViewBag.Shapes = shapes;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatrixQuestion(MatrixQuestionInputModel model)
        {
            var allShapes = _context.Shapes.ToList();

            // Soru matrisi
            var parsedMatrix = JsonSerializer.Deserialize<List<List<string>>>(model.QuestionMatrix ?? "[]");
            var questionSvg = _svgService.GenerateMatrixSvg(parsedMatrix, allShapes);
            var questionPath = _svgService.SaveSvgToFile(questionSvg);

            // Şıklar
            var optionPaths = new List<string>();
            foreach (var json in model.OptionMatrices ?? new List<string>())
            {
                if (string.IsNullOrWhiteSpace(json)) continue;

                var optionMatrix = JsonSerializer.Deserialize<List<List<string>>>(json);
                var svg = _svgService.GenerateMatrixSvg(optionMatrix, allShapes);
                var path = _svgService.SaveSvgToFile(svg);
                optionPaths.Add(path);
            }

            var question = new Question
            {
                QuestionType = QuestionType.MultipleChoice
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var mcq = new MultipleChoiceQuestion
            {
                QuestionId = question.Id,
                QuestionText = model.QuestionText,
                ImagePath = questionPath,
                OptionA = optionPaths.ElementAtOrDefault(0),
                OptionB = optionPaths.ElementAtOrDefault(1),
                OptionC = optionPaths.ElementAtOrDefault(2),
                OptionD = optionPaths.ElementAtOrDefault(3),
                OptionE = optionPaths.ElementAtOrDefault(4),
                CorrectOption = model.CorrectOption,
                Difficulty = model.Difficulty,
                QuestionFormat = model.QuestionFormat
            };
            _context.MultipleChoiceQuestions.Add(mcq);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateMatrixQuestion");
        }



        public IActionResult Solve()
        {
            var questions = _context.Questions
                .Include(q => q.MultipleChoiceDetail)
                .Where(q => q.QuestionType == QuestionType.MultipleChoice)
                .Take(25)
                .ToList();

            return View(questions);
        }

    }
}
