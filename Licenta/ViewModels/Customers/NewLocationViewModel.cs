﻿using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Customers
{
    public class NewLocationViewModel
    {
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Country required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street required")]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Street number required")]
        [Display(Name = "Street number")]
        public string StreetNumber { get; set; }

        [Required(ErrorMessage = "Postal Code required")]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Display(Name = "Tag")]
        public string Tag { get; set; }

    }
}
