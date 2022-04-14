using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;

namespace Licenta.ViewModels.Customers
{
    public class CustomerListViewModel
    {
        public IEnumerable<Customer> CustomerViews { get; set; }
    }
}
