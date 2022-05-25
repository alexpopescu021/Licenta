using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Drivers
{
    public class EditInfoViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
