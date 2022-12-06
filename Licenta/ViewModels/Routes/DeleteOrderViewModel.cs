using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Routes
{
    public class DeleteOrderViewModel
    {
        public string RouteId { get; set; }

        [Required(ErrorMessage = "Choosing an Order is required.")]
        [Display(Name = "Order:")]
        public string OrderId { get; set; }
        public List<SelectListItem> OrderList;
    }
}
