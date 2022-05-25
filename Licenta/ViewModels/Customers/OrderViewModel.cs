using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Orders
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Customer Customer { get; set; }
        public int selectedOrder { get; set; }
    }
}
