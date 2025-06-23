using CareerSim.Data;
using CareerSim.Models.Career;
using CareerSim.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerSim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ErrorLogs()
        {
            var logs = await _context.ErrorLogs
                .OrderByDescending(e => e.Timestamp)
                .Join(
                    _context.Users,
                    error => error.UserId,
                    user => user.Id,
                    (error, user) => new ErrorLogViewModel
                    {
                        Message = error.Message,
                        StackTrace = error.StackTrace,
                        Path = error.Path,
                        UserId = error.UserId,
                        UserName = user.UserName,
                        FullName = user.Name,
                        BrowserInfo = error.BrowserInfo,
                        Timestamp = error.Timestamp
                    }
                )
                .ToListAsync();

            return View(logs);
        }
        public async Task<IActionResult> ActivityLogs()
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(x => x.Timestamp)
                .Join(
                    _context.Users,
                    log => log.UserId,
                    user => user.Id,
                    (log, user) => new ActivityLogViewModel
                    {
                        UserName = user.UserName,
                        FullName=user.Name,
                        Action = log.Action,
                        Timestamp = log.Timestamp,
                        IpAddress = log.IpAddress,
                        BrowserInfo = log.BrowserInfo
                    }
                )
                .ToListAsync();

            return View(logs);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CareerList()
        {
            var careers = await _context.Careers.ToListAsync();
            return View("CareerList", careers);
        }

        public IActionResult AddCareer()
        {
            return View("CareerForm", new Career());
        }

        [HttpPost]
        public async Task<IActionResult> AddCareer(Career model)
        {
            if (!ModelState.IsValid)
                return View("CareerForm", model);

            _context.Careers.Add(model);
            await _context.SaveChangesAsync();

            // Formu temizlemek i�in:
            ModelState.Clear();
            ViewBag.SuccessMessage = "Kariyer ba�ar�yla eklendi.";
            return View("CareerForm", new Career());
        }


        public async Task<IActionResult> EditCareer(int id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career == null)
                return NotFound();

            return View("CareerForm", career);
        }

        [HttpPost]
        public async Task<IActionResult> EditCareer(Career model)
        {
            if (!ModelState.IsValid)
                return View("CareerForm", model);

            _context.Careers.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("CareerList");
        }

        public async Task<IActionResult> DeleteCareer(int id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career != null)
            {
                _context.Careers.Remove(career);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("CareerList");
        }

        public async Task<IActionResult> CareerTaskList(int careerId)
        {
            var career = await _context.Careers.FindAsync(careerId);
            if (career == null) return NotFound();

            ViewBag.CareerId = careerId;
            ViewBag.CareerTitle = career.Title;

            var tasks = await _context.CareerTasks
                .Where(t => t.CareerId == careerId)
                .OrderBy(t => t.Order)
                .ToListAsync();

            return View("CareerTaskList", tasks);
        }

        #region G�rev ��lemleri
        public async Task<IActionResult> AddCareerTask(int careerId)
        {
            var career = await _context.Careers.FindAsync(careerId);
            if (career == null) return NotFound();

            var existingTasks = await _context.CareerTasks
                .Where(t => t.CareerId == careerId)
                .OrderBy(t => t.Order)
                .ToListAsync();

            int nextOrder = existingTasks.Any() ? existingTasks.Max(t => t.Order) + 1 : 1;

            ViewBag.CareerTitle = career.Title;
            ViewBag.CareerId = careerId;
            ViewBag.ExistingTasks = existingTasks;

            var model = new CareerTask
            {
                CareerId = careerId,
                Order = nextOrder
            };

            return View("CareerTaskForm", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddCareerTask(CareerTask model)
        {
            model.Career = await _context.Careers.FindAsync(model.CareerId);
            ModelState.Remove("Career.Title");
            ModelState.Remove("Career.Description");

            if (!ModelState.IsValid)
            {
                ViewBag.CareerTitle = model.Career?.Title ?? "Bilinmeyen Kariyer";

                var existingTasks = await _context.CareerTasks
                    .Where(t => t.CareerId == model.CareerId)
                    .OrderBy(t => t.Order)
                    .ToListAsync();

                ViewBag.ExistingTasks = existingTasks;

                return View("CareerTaskForm", model);
            }

            _context.CareerTasks.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("CareerTaskList", new { careerId = model.CareerId });
        }



        public async Task<IActionResult> EditCareerTask(int id)
        {
            var task = await _context.CareerTasks.FindAsync(id);
            if (task == null) return NotFound();

            return View("CareerTaskForm", task);
        }

        [HttpPost]
        public async Task<IActionResult> EditCareerTask(CareerTask model)
        {
            model.Career = await _context.Careers.FindAsync(model.CareerId);
            ModelState.Remove("Career.Title");
            ModelState.Remove("Career.Description");

            if (!ModelState.IsValid)
            {
                ViewBag.CareerTitle = model.Career?.Title ?? "Bilinmeyen Kariyer";

                var existingTasks = await _context.CareerTasks
                    .Where(t => t.CareerId == model.CareerId)
                    .OrderBy(t => t.Order)
                    .ToListAsync();

                ViewBag.ExistingTasks = existingTasks;

                return View("CareerTaskForm", model);
            }

            _context.CareerTasks.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("CareerTaskList", new { careerId = model.CareerId });
        }

        public async Task<IActionResult> DeleteCareerTask(int id)
        {
            var task = await _context.CareerTasks.FindAsync(id);
            if (task != null)
            {
                _context.CareerTasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("CareerTaskList", new { careerId = task?.CareerId });
        }

        #endregion

    }
}
