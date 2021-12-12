using Licenta.ApplicationLogic.Services;
using Licenta.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.ApplicationLogic.Services;

namespace Licenta.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService orderService;
        private readonly CustomerService customerService;

        public OrdersController(OrderService orderService, CustomerService customerService)
        {
            this.orderService = orderService;
            
            this.customerService = customerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrdersTable()
        {
            var ordersView = new OrderViewModel()
            {
                Orders = orderService.GetAllOrders()
            };

            return PartialView("_OrdersTablePartial", ordersView);
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
