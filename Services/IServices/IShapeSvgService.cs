using CareerSim.Models.Shape;

namespace CareerSim.Services.IServices
{
    public interface IShapeSvgService
    {
        string GenerateSvgContent(Shape shape);
        string SaveSvgToFile(string svgContent);
    }
}
