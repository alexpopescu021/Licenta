using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;

namespace Licenta.ViewModels.Orders
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders {get; set;}
    }
}
