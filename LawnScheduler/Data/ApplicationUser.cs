using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LawnScheduler.Data
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
    }
}
