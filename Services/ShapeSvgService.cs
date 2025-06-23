using CareerSim.Models.Shape;
using CareerSim.Models.Shape.Enum;
using CareerSim.Services.IServices;

namespace CareerSim.Services
{
    public class ShapeSvgService : IShapeSvgService
    {
        private readonly IWebHostEnvironment _env;

        public ShapeSvgService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateSvgContent(Shape shape)
        {
            // Basit örnek içerik — üretim kurallarına göre genişletilecek
            string fill = shape.FillStyle == FillStyle.Filled ? shape.Color : "none";
            string stroke = shape.FillStyle == FillStyle.Filled ? "none" : shape.Color;

            string rotation = $"transform=\"rotate({shape.Rotation} 50 50)\"";

            if (shape.ShapeType == ShapeType.Triangle)
            {
                return $"<svg width=\"100\" height=\"100\" xmlns=\"http://www.w3.org/2000/svg\">" +
                       $"<polygon points=\"50,10 90,90 10,90\" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"2\" {rotation}/></svg>";
            }
            else if (shape.ShapeType == ShapeType.Square)
            {
                return $"<svg width=\"100\" height=\"100\" xmlns=\"http://www.w3.org/2000/svg\">" +
                       $"<rect x=\"25\" y=\"25\" width=\"50\" height=\"50\" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"2\" {rotation}/></svg>";
            }
            // Diğer şekiller eklenebilir

            return string.Empty;
        }

        public string SaveSvgToFile(string svgContent)
        {
            var fileName = Guid.NewGuid().ToString() + ".svg";
            var path = Path.Combine(_env.WebRootPath, "svgs", fileName);
            File.WriteAllText(path, svgContent);
            return "/svgs/" + fileName;
        }
    }

}
