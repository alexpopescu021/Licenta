﻿using System.ComponentModel.DataAnnotations;

namespace Licenta.ViewModels.Vehicles
{
    public class UpdateVehicleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string NameUpdate { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Maxim carry weight")]
        public int MaximCarryWeight { get; set; }

        [Required]
        [Display(Name = "VIN(Vehicle Identification Number)")]
        public string Vin { get; set; }
    }
}
