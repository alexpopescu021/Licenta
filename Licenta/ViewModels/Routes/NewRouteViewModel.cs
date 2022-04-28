using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Routes
{
    public class NewRouteViewModel
    {
        public List<string> OrderIds { get; set; }

        [Required(ErrorMessage = "Choosing a vehicle is required.")]
        [Display(Name = "Vehicle:")]
        public string VehicleId { get; set; }

        public List<SelectListItem> VehicleList { get; set; }
        public List<SelectListItem> OrderList { get; set; }


    }
}
