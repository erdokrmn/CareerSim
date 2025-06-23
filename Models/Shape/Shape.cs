using CareerSim.Models.Shape.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CareerSim.Models.Shape
{
    public class Shape
    {
        public int Id { get; set; }

        public ShapeType ShapeType { get; set; }     // Örn: "triangle", "circle"
        public FillStyle FillStyle { get; set; }  // enum kullanımı

        public string Color { get; set; }         // Örn: "red", "black"
        public int Rotation { get; set; }         // Örn: 0, 90, 180, 270

        [ValidateNever]
        public string SvgPath { get; set; }
    }
}
