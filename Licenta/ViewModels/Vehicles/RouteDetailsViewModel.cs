using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Vehicles
{
    public class RouteDetailsViewModel
    {
        public string VehicleId { get; set; }
        public IEnumerable<RouteEntry> RouteEntries { get; set; }
    }
}
