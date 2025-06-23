using CareerSim.Models.Shape;

namespace CareerSim.Services.IServices
{
    public interface IMatrixSvgService
    {
        string GenerateMatrixSvg(List<List<string>> shapeIds, List<Shape> shapeList, int cellSize = 50);
        string SaveSvgToFile(string svgContent);
    }
}
