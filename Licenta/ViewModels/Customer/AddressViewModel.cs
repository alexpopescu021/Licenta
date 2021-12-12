using Licenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels.Customer
{
    public class AddressViewModel
    {
        public IEnumerable<LocationAddress> Addresses { get; set; }
    }
}
