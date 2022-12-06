using System.Collections.Generic;
using Licenta.Model;

namespace Licenta.ViewModels.Customers
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Customer Customer { get; set; }
        public int SelectedOrder { get; set; }
    }
}
