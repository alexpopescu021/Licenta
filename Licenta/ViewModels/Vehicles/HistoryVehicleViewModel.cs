using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Vehicles
{
    public class HistoryVehicleViewModel
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<VehicleDriver> VehicleDriver { get; set; }
    }
}
