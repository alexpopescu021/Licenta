using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Customers
{
    public class NewCustomerViewModel
    {
        [Required(ErrorMessage = "Name required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number required")]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Email required")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
