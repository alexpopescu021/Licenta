using Licenta.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Licenta.ApplicationLogic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace Licenta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly CustomerService customerService;

        public HomeController(ILogger<HomeController> _logger, CustomerService _customerService)
        {
            logger = _logger;
            customerService = _customerService;
        }

        public IActionResult Index()
        {
            if(User?.Identity.IsAuthenticated == true)
            { 
                if (!User.IsInRole("Admin"))
                {
                    bool ok = true;
                    var user = User.Identity.GetUserId();
                    if(user.Length != 0)
                    { 
                        var list = customerService.GetAllCustomers();
                        foreach(var customer in list)
                        {
                            if(customer.Id.ToString() == user)
                            {
                                ok = false;
                            }
                        }
                        if(ok)
                        {
                            customerService.CreateNewCustomerFromIdentity(user,null,null,null);
                        }
                    }

                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
