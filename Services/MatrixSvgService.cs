using CareerSim.Models.Shape;
using CareerSim.Models.Shape.Enum;
using CareerSim.Services.IServices;

namespace CareerSim.Services
{
    public class MatrixSvgService : IMatrixSvgService
    {
        private readonly IWebHostEnvironment _env;

        public MatrixSvgService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateMatrixSvg(List<List<string>> shapeIds, List<Shape> shapeList, int cellSize = 50)
        {
            var sb = new System.Text.StringBuilder();
            int rows = shapeIds.Count;
            int cols = shapeIds[0].Count;

            sb.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width='{cols * cellSize}' height='{rows * cellSize}'>");

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string id = shapeIds[i][j]; // artık "3" gibi sade gelir

                    if (!int.TryParse(id, out int shapeId))
                    {
                        Console.WriteLine($"Geçersiz şekil ID'si: {id}");
                        continue;
                    }

                    var shape = shapeList.FirstOrDefault(s => s.Id == shapeId);

                    if (shape == null)
                    {
                        Console.WriteLine($"Eşleşmeyen ID: {shapeId}");
                        continue;
                    }

                    string fill = shape.FillStyle == FillStyle.Filled ? shape.Color : "none";
                    string stroke = shape.FillStyle == FillStyle.Filled ? "none" : shape.Color;
                    string rotation = $"transform='rotate({shape.Rotation} {(j * cellSize + cellSize / 2)} {(i * cellSize + cellSize / 2)})'";

                    if (shape.ShapeType == ShapeType.Triangle)
                    {
                        sb.AppendLine($"<polygon points='{j * cellSize + cellSize / 2}, {i * cellSize + 10} " +
                                      $"{j * cellSize + cellSize - 10}, {i * cellSize + cellSize - 10} " +
                                      $"{j * cellSize + 10}, {i * cellSize + cellSize - 10}' " +
                                      $"fill='{fill}' stroke='{stroke}' stroke-width='2' {rotation} />");
                    }
                    else if (shape.ShapeType == ShapeType.Square)
                    {
                        sb.AppendLine($"<rect x='{j * cellSize + 10}' y='{i * cellSize + 10}' width='{cellSize - 20}' height='{cellSize - 20}' " +
                                      $"fill='{fill}' stroke='{stroke}' stroke-width='2' {rotation} />");
                    }
                }
            }

            sb.AppendLine("</svg>");
            return sb.ToString();
        }

        public string SaveSvgToFile(string svgContent)
        {
            var fileName = Guid.NewGuid().ToString() + ".svg";
            var folder = Path.Combine(_env.WebRootPath, "svgs");
            Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);
            File.WriteAllText(fullPath, svgContent);

            return "/svgs/" + fileName;
        }
    }
}
