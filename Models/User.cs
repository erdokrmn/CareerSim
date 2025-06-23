using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerSim.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User :IdentityUser
    {
        public string? Name { get; set; }
        public string? ProfileImagePath { get; set; }

    }
}
