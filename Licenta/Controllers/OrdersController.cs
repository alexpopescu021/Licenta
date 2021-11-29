using Licenta.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewOrder(string id)
        {
            
                return PartialView("_NewOrderPartial", null);
            
        }

        [HttpPost]
        public IActionResult NewOrder([FromForm] NewOrderViewModel orderData)
        {
            
                return PartialView("_NewOrderPartial", orderData);
          
        }
    }
}
