using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Vehicles
{
    public class VehiclesListViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
    }
}
