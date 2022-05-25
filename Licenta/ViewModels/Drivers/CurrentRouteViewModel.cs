using Licenta.Model;
using System;
using System.Collections.Generic;

namespace Licenta.ViewModels.Drivers
{
    public class CurrentRouteViewModel
    {

        public ICollection<RouteEntry> RouteEntries { get; set; }
        public Guid DriverId { get; set; }

    }
}
