using Licenta.Model;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.ViewModels.Drivers
{
    public class RoutesHistoryViewModel
    {
        public List<Route> Routes { get; set; }
        public RoutesHistoryViewModel()
        {
            Routes = new List<Route>();
        }
        public void ConfigureRoutes(ICollection<Route> routes)
        {
            Routes = routes.ToList();

        }

    }
}
