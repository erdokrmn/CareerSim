using System.ComponentModel.DataAnnotations;

namespace CareerSim.Models.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ad-soyad gereklidir.")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
        public string Email { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? CurrentProfileImagePath { get; set; } // mevcut fotoğrafı göstermek için
    }
}
