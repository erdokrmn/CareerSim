using CareerSim.Models;
using CareerSim.Models.ViewModels;
using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly IActivityLogService _activityLogService;
    public UserController(UserManager<User> userManager, IWebHostEnvironment env, IActivityLogService activityLogService)
    {
        _userManager = userManager;
        _env = env;
        _activityLogService = activityLogService;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Auth");

        var model = new UpdateProfileViewModel
        {
            UserName = user.UserName,
            FullName = user.Name,
            Email = user.Email,
            CurrentProfileImagePath = !string.IsNullOrEmpty(user.ProfileImagePath)
                ? $"/uploads/profile/{user.ProfileImagePath}"
                : "/images/default/user.png"
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(UpdateProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Auth");

        // Başka kullanıcı bu maili kullanıyorsa engelle
        var emailOwner = await _userManager.FindByEmailAsync(model.Email);
        if (emailOwner != null && emailOwner.Id != user.Id)
        {
            ModelState.AddModelError("Email", "Bu e-posta başka bir kullanıcı tarafından kullanılıyor.");
            return View(model);
        }

        user.UserName = model.UserName;
        user.Name = model.FullName;
        user.Email = model.Email;

        // Profil fotoğrafı yüklendiyse kaydet
        if (model.ProfileImage != null && model.ProfileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/profile");
            Directory.CreateDirectory(uploadsFolder);

            // ❗ Eski resmi sil (eğer varsayılan user.png değilse)
            if (!string.IsNullOrEmpty(user.ProfileImagePath) && user.ProfileImagePath != "user.png")
            {
                var oldPath = Path.Combine(uploadsFolder, user.ProfileImagePath);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfileImage.CopyToAsync(stream);
            }

            user.ProfileImagePath = fileName;
        }

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // 🔍 Aktivite logu
            await _activityLogService.LogAsync(user.Id, "Profil bilgilerini güncelledi");

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

}
