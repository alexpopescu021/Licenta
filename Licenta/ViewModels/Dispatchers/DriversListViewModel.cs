using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Dispatchers
{
    public class DriversListViewModel
    {
        public IEnumerable<Driver> Drivers { get; set; }

        public IEnumerable<Route> Routes { get; set; }

    }
}
