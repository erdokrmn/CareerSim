using System.ComponentModel.DataAnnotations;

namespace CareerSim.Models.Career
{
    public class CareerTask
    {
        public CareerTask()
        {
            Career = new Career();
        }
        public int Id { get; set; }
        public int CareerId { get; set; }
        public string Title { get; set; }  // Görev başlığı
        public string Description { get; set; }  // Görev açıklaması
        [Required]
        [StringLength(400, ErrorMessage = "Cevap en fazla 400 karakter olabilir.")]
        public string SampleAnswer { get; set; }

        public int Order { get; set; }  // 1-5 sıralaması

        public Career Career { get; set; }
    }

}
