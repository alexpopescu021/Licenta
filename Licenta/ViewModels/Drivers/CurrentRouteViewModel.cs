using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;

namespace Licenta.ViewModels.Drivers
{
    public class CurrentRouteViewModel
    {
      
        public ICollection<RouteEntry> RouteEntries { get; set; }
        public Guid DriverId { get; set; }

    }
}
