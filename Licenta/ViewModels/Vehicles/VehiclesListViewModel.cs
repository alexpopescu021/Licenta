using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;

namespace Licenta.ViewModels.Vehicles
{
    public class VehiclesListViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
    }
}
