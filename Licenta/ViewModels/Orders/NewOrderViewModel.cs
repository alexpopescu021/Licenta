using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Licenta.ViewModels.Orders
{
    public class NewOrderViewModel
    {
        public enum LocationType
        {
            Delivery,
            Pickup
        }

        [Required(ErrorMessage = "Sender required")]
        [Display(Name = "Sender:")]
        public string SenderId { get; set; }

        [Display(Name = "Pickup Address:")]
        public string PickupLocationId { get; set; }

        [Display(Name = "Delivery Address:")]
        public string DeliveryLocationId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Recipient required.")]
        [Display(Name = "Recipient Name")]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Recipient Email")]
        [EmailAddress(ErrorMessage = "Wrong email format.")]
        public string RecipientEmail { get; set; }

        [Required(ErrorMessage = "Phone number required.")]
        [Display(Name = "Recipient Phone Number")]
        public string RecipientPhoneNo { get; set; }

        public List<SelectListItem> CustomerList { get; set; }
        public List<SelectListItem> PickupLocations { get; set; }
        public List<SelectListItem> DeliveryLocations { get; set; }

        [Range(0, 9999999, ErrorMessage = "Wrong Input.")]
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
    }
}
