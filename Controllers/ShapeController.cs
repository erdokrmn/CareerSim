using Microsoft.AspNetCore.Mvc;
using CareerSim.Data;
using CareerSim.Models;
using CareerSim.Services.IServices;
using CareerSim.Models.Shape;

namespace CareerSim.Controllers
{
    public class ShapeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IShapeSvgService _svgService;

        public ShapeController(AppDbContext context, IShapeSvgService svgService)
        {
            _context = context;
            _svgService = svgService;
        }

        // GET: /Shape/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Shape/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shape shape)
        {
            if (!ModelState.IsValid)
                return View(shape);

            // 1. Aynı özellikte bir kayıt veritabanında var mı kontrol et
            var existingShape = _context.Shapes.FirstOrDefault(s =>
                s.ShapeType == shape.ShapeType &&
                s.FillStyle == shape.FillStyle &&
                s.Color == shape.Color &&
                s.Rotation == shape.Rotation
            );

            if (existingShape != null)
            {
                // Aynı kayıt varsa kullanıcıyı tekrar yönlendir veya mesaj göster
                TempData["Error"] = "Bu özelliklerde bir şekil zaten mevcut.";
                return RedirectToAction("Create");
            }

            // 2. SVG oluştur ve kaydet
            var svgContent = _svgService.GenerateSvgContent(shape);
            var svgPath = _svgService.SaveSvgToFile(svgContent);
            shape.SvgPath = svgPath;

            // 3. Veritabanına ekle
            _context.Shapes.Add(shape);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create");
        }

    }
}
