using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Routes
{
    public class DeleteOrderViewModel
    {
        public string routeId { get; set; }

        [Required(ErrorMessage = "Choosing an Order is required.")]
        [Display(Name = "Order:")]
        public string orderId { get; set; }
        public List<SelectListItem> OrderList;
    }
}
