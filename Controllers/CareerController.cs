using System.Text;
using System.Threading.Tasks;
using CareerSim.Data;
using CareerSim.Models;
using CareerSim.Models.Career;
using CareerSim.Models.ViewModels;
using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class CareerController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IOpenAiService _openAiService;

    public CareerController(AppDbContext context, UserManager<User> userManager, IOpenAiService openAiService)
    {
        _context = context;
        _userManager = userManager;
        _openAiService = openAiService;
    }

    public async Task<IActionResult> CareerList()
    {
        var careers = await _context.Careers.ToListAsync();
        return View("CareerList", careers);
    }

    public async Task<IActionResult> Start(int careerId)
    {
        var user = await _userManager.GetUserAsync(User);

        var progress = await _context.UserCareerProgresses
            .FirstOrDefaultAsync(p => p.UserId == user.Id && p.CareerId == careerId);

        if (progress == null)
        {
            progress = new UserCareerProgress
            {
                UserId = user.Id,
                CareerId = careerId,
                CurrentTaskOrder = 1, // 🔥 HER ZAMAN 1'den başlamalı
                IsCompleted = false,
                StartedAt = DateTime.UtcNow
            };

            _context.UserCareerProgresses.Add(progress);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ActiveTask", new { careerId });
    }


    public async Task<IActionResult> ActiveTask(int careerId)
    {
        var user = await _userManager.GetUserAsync(User);

        var allTasks = await _context.CareerTasks
            .Where(t => t.CareerId == careerId)
            .OrderBy(t => t.Order)
            .ToListAsync();

        var answeredOrders = await _context.CareerAnswers
            .Where(a => a.UserId == user.Id && a.CareerTask.CareerId == careerId)
            .Select(a => a.CareerTask.Order)
            .ToListAsync();

        var nextTask = allTasks
            .FirstOrDefault(t => !answeredOrders.Contains(t.Order));

        var model = new CareerTaskViewModel
        {
            CareerId = careerId,
            CareerTitle = (await _context.Careers.FindAsync(careerId))?.Title ?? "Bilinmeyen Kariyer",
            IsCompleted = nextTask == null
        };

        if (nextTask != null)
        {
            model.TaskId = nextTask.Id;
            model.TaskTitle = nextTask.Title;
            model.TaskDescription = nextTask.Description;
        }
        else
        {
            var lastAnswer = await _context.CareerAnswers
                .Include(a => a.CareerTask)
                .Where(a => a.UserId == user.Id && a.CareerTask.CareerId == careerId)
                .OrderByDescending(a => a.SubmittedAt)
                .FirstOrDefaultAsync();

            model.LastAnswerText = lastAnswer?.AnswerText;
            model.TaskId = lastAnswer?.CareerTaskId ?? 0; // AI için
            model.AnswerId = lastAnswer?.Id ?? 0; // AI için

        }

        return View("ActiveTask", model);
    }

    [HttpGet]
    public async Task<IActionResult> MySummaries()
    {
        var userId = _userManager.GetUserId(User);

        var feedbacks = await _context.CareerFeedbacks
            .Include(f => f.CareerAnswer)
                .ThenInclude(a => a.CareerTask)
                    .ThenInclude(t => t.Career)
            .Where(f => f.CareerAnswer.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        var list = feedbacks
            .Select(f => new MySummaryViewModel
            {
                CareerId = f.CareerAnswer.CareerTask.CareerId,
                CareerTitle = f.CareerAnswer.CareerTask.Career.Title,
                CreatedAt = f.CreatedAt
            })
            .DistinctBy(x => x.CareerId)
            .ToList();

        return View("MySummaries", list);
    }


    [HttpGet]
    public async Task<IActionResult> EvaluateSummary(int careerId)
    {
        var userId = _userManager.GetUserId(User);

        // Kariyeri çek
        var career = await _context.Careers.FindAsync(careerId);
        if (career == null) return NotFound();

        // Kullanıcının bu kariyer için daha önce kaydettiği feedback var mı?
        var existingFeedback = await _context.CareerFeedbacks
            .Include(f => f.CareerAnswer)
                .ThenInclude(a => a.CareerTask)
            .Where(f =>
                f.CareerAnswer.UserId == userId &&
                f.CareerAnswer.CareerTask.CareerId == careerId)
            .OrderByDescending(f => f.CreatedAt)
            .FirstOrDefaultAsync();

        string summaryText;
        List<CareerSummaryItemViewModel> items;

        if (existingFeedback != null)
        {
            // 1️⃣ Daha önce oluşturulan özeti kullan
            summaryText = existingFeedback.FeedbackText;

            // Items’ı yine de göstermek istersen:
            var answers = await _context.CareerAnswers
                .Include(a => a.CareerTask)
                .Where(a => a.UserId == userId && a.CareerTask.CareerId == careerId)
                .OrderBy(a => a.CareerTask.Order)
                .ToListAsync();

            items = answers.Select(a => new CareerSummaryItemViewModel
            {
                TaskTitle = a.CareerTask.Title,
                TaskDescription = a.CareerTask.Description,
                SampleAnswer = a.CareerTask.SampleAnswer,
                UserAnswer = a.AnswerText
            }).ToList();
        }
        else
        {
            // 2️⃣ İlk defa AI çağrısı yap
            var answers = await _context.CareerAnswers
                .Include(a => a.CareerTask)
                .Where(a => a.UserId == userId && a.CareerTask.CareerId == careerId)
                .OrderBy(a => a.CareerTask.Order)
                .ToListAsync();

            items = answers.Select(a => new CareerSummaryItemViewModel
            {
                TaskTitle = a.CareerTask.Title,
                TaskDescription = a.CareerTask.Description,
                SampleAnswer = a.CareerTask.SampleAnswer,
                UserAnswer = a.AnswerText
            }).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Sen bir eğitmensin. Aşağıdaki görevleri ve öğrenci cevaplarını değerlendir.");
            sb.AppendLine("Her görev için sadece öğrencinin cevabına göre 10 üzerinden puan ver. Örnek cevaplar sadece referans içindir, puanlamaya dahil edilmez.");
            sb.AppendLine("Anlamsız, konu dışı, çok kısa ya da rastgele girilmiş cevaplar varsa 0–1 puan arasında değerlendir.");
            sb.AppendLine("Her görev için puan ve kısa yorum ver. En sonunda sadece öğrenci cevaplarının puanlarına göre bir ortalama puan çıkar.");


            foreach (var it in items)
            {
                sb.AppendLine($"\n🧩 Görev: {it.TaskTitle}");
                sb.AppendLine($"Açıklama: {it.TaskDescription}");
                sb.AppendLine($"Örnek Cevap: {it.SampleAnswer}");
                sb.AppendLine($"Öğrencinin Cevabı: {it.UserAnswer}");
            }

            summaryText = await _openAiService.EvaluateAsync(sb.ToString());

            // Feedback’i kaydet
            _context.CareerFeedbacks.Add(new CareerFeedback
            {
                CareerAnswerId = answers.Last().Id,
                FeedbackText = summaryText,
                CreatedAt = DateTime.UtcNow,
                IsPremium = true
            });
            await _context.SaveChangesAsync();
        }

        var vm = new CareerSummaryViewModel
        {
            CareerTitle = career.Title,
            Items = items,
            SummaryText = summaryText
        };
        return View("CareerSummary", vm);
    }





    [HttpPost]
    public async Task<IActionResult> SubmitAnswer(CareerTaskViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var answer = new CareerAnswer
        {
            UserId = user.Id,
            CareerTaskId = model.TaskId,
            AnswerText = model.AnswerText,
            SubmittedAt = DateTime.UtcNow
        };

        _context.CareerAnswers.Add(answer);

        var progress = await _context.UserCareerProgresses
            .FirstOrDefaultAsync(p => p.UserId == user.Id && p.CareerId == model.CareerId);

        if (progress != null)
        {
            progress.CurrentTaskOrder++;

            var maxOrder = await _context.CareerTasks
                .Where(t => t.CareerId == model.CareerId)
                .MaxAsync(t => t.Order);

            if (progress.CurrentTaskOrder > maxOrder)
            {
                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("ActiveTask", new { careerId = model.CareerId });
    }

    public async Task<IActionResult> CareerResult(int careerId)
    {
        var user = await _userManager.GetUserAsync(User);

        // En son çözülen task için cevabı al
        var answer = await _context.CareerAnswers
            .Where(a => a.UserId == user.Id && a.CareerTask.CareerId == careerId)
            .OrderByDescending(a => a.SubmittedAt)
            .Include(a => a.CareerTask)
            .FirstOrDefaultAsync();

        // Cevap yoksa kullanıcı henüz çözmemiştir
        if (answer == null)
            return RedirectToAction("CareerList");

        // AI geri bildirimi varsa al
        var feedback = await _context.CareerFeedbacks
            .FirstOrDefaultAsync(f => f.CareerAnswerId == answer.Id);

        var model = new CareerResultViewModel
        {
            CareerTitle = answer.CareerTask.Title,
            HasPremiumEvaluation = feedback != null,
            FeedbackText = feedback?.FeedbackText,
            AnswerId = answer.Id
        };

        return View("CareerResult", model);
    }


    public async Task<IActionResult> CareerFeedback(int careerId)
    {
        var user = await _userManager.GetUserAsync(User);

        var answer = await _context.CareerAnswers
            .Where(a => a.UserId == user.Id && a.CareerTask.CareerId == careerId)
            .OrderByDescending(a => a.SubmittedAt)
            .FirstOrDefaultAsync();

        if (answer == null)
            return RedirectToAction("CareerResult", new { careerId });

        var feedback = await _context.CareerFeedbacks
            .FirstOrDefaultAsync(f => f.CareerAnswerId == answer.Id);

        if (feedback == null)
            return RedirectToAction("CareerResult", new { careerId });

        return View("CareerFeedback", feedback);
    }

}
