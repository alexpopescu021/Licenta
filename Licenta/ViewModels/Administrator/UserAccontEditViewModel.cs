using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Administrator
{
    public class UserAccontEditViewModel
    {
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]

        public string UserId { get; set; }
        [Required]

        public string Role { get; set; }

    }
}
