using Licenta.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Licenta.AppLogic.Services;

namespace Licenta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly CustomerService customerService;
        private readonly UserManager<IdentityUser> userManager;
        public HomeController(ILogger<HomeController> logger, CustomerService customerService, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.customerService = customerService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("Driver") && !User.IsInRole("Dispatcher"))
                {
                    var ok = true;
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await userManager.FindByIdAsync(userId);
                    if (userId.Length != 0)
                    {
                        var list = customerService.GetAllCustomers();
                        foreach (var customer in list)
                        {
                            if (customer.Id.ToString() == userId)
                            {
                                ok = false;
                            }
                        }
                        if (ok)
                        {
                            customerService.CreateNewCustomerFromIdentity(userId, user.UserName, user.PhoneNumber, user.Email);
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
