using Licenta.Model;
using System.Collections.Generic;

namespace Licenta.ViewModels.Customers
{
    public class CustomerListViewModel
    {
        public IEnumerable<Customer> CustomerViews { get; set; }
    }
}
