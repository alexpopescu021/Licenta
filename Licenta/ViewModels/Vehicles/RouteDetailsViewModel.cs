using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;

namespace Licenta.ViewModels.Vehicles
{
    public class RouteDetailsViewModel
    {
        public string VehicleId { get; set; }
        public IEnumerable<RouteEntry> RouteEntries { get; set; }
    }
}
