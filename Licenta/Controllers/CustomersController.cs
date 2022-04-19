using Licenta.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Licenta.ApplicationLogic.Services;
using Licenta.Model;
using Microsoft.AspNetCore.Http.Extensions;

namespace Licenta.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly CustomerService customerService;

        public CustomersController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager, CustomerService _customerService)
        {
            userManager = _userManager;
            customerService = _customerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CustomerTable()
        {
            CustomerListViewModel customerViewModel = null;
            try
            {
                customerViewModel = new CustomerListViewModel()
                {
                    CustomerViews = customerService.GetAllCustomers()
                };

            }
            catch (Exception e)
            {
                //log
            }

            return PartialView("_CustomerTable", customerViewModel);
        }

        [HttpGet]
        public IActionResult UpdateCustomer([FromRoute] string Id)
        {
            try
            {
                var customerToUpdate = customerService.GetCustomerById(Id);

                var editCustomerViewModel = new UpdateCustomerViewModel()
                {
                    Id = Id,
                    Name = customerToUpdate.Name,
                    PhoneNo = customerToUpdate.ContactDetails.PhoneNo,
                    Email = customerToUpdate.ContactDetails.Email
                };

                return PartialView("_UpdateCustomerPartial", editCustomerViewModel);

            }
            catch (Exception e)
            {
                //log
                return BadRequest("Failed to edit customer entity");
            }
        }

        [HttpPost]
        public IActionResult UpdateCustomer([FromForm] UpdateCustomerViewModel updatedData, [FromRoute] string Id)
        {
            if ( updatedData.Name == null ||
                   updatedData.PhoneNo == null)
            {
                return PartialView("_UpdateCustomerPartial", new NewCustomerViewModel());
            }

            try
            {
                var customerToUpdate = customerService.GetCustomerById(Id);
                customerService.UpdateCustomer(Guid.Parse(Id),
                                                updatedData.Name,
                                                updatedData.PhoneNo,
                                                updatedData.Email);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //log
                return BadRequest("Failed to edit customer entity");
            }
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewLocationViewModel locationModel = new NewLocationViewModel()
            {

                CustomerId = userId
            };

            return PartialView("_AddLocationPartial", locationModel);
        }

        [HttpPost]
        public IActionResult AddLocation([FromForm] NewLocationViewModel locationData)
        {
            if (!ModelState.IsValid || locationData == null)
            {
                return PartialView("_AddLocationPartial", locationData);
            }

            try
            {
                var customer = customerService.GetCustomerById(locationData.CustomerId);

                var newLocation = LocationAddress.Create(
                            locationData.Country,
                            locationData.City,
                            locationData.Street,
                            locationData.StreetNumber,
                            locationData.PostalCode);

                customerService.AddLocationToCustomer(customer.Id, newLocation);

                return RedirectToPage("/Account/Manage/PickupLocations", new { area = "Identity" } );
            }
            catch (Exception notFound) { return null; }
            //{
            //    logger.LogError("Failed to find the customer entity {@Exception}", notFound.Message);
            //    logger.LogDebug("Failed to find the customer entity {@ExceptionMessage}", notFound);
            //    return BadRequest("Failed to find user");
            //}
            //catch (Exception e)
            //{
            //    logger.LogError("Failed to add a new Location {@Exception}", e.Message);
            //    logger.LogDebug("Failed to add a new Location {@ExceptionMessage}", e);
            //    return BadRequest("Failed to create a new Location");
            //}
        }

        public IActionResult AddressTable()
        {
            var user = userManager.GetUserId(User);
            var addressView = new AddressViewModel()
            {
                Addresses = customerService.GetCustomerAddresses(user)
            };

            return PartialView("_AddressTablePartial", addressView);
        }


        [HttpGet]
        public IActionResult EditLocation([FromRoute] string Id)
        {
            try
            {
                var locationToUpdate = customerService.GetLocationAddress(Id);

                var editLocationViewModel = new EditLocationViewModel()
                {
                    Id = Id,
                    Country = locationToUpdate.Country,
                    City = locationToUpdate.City,
                    Street = locationToUpdate.Street,
                    StreetNumber = locationToUpdate.StreetNumber,
                    PostalCode = locationToUpdate.PostalCode
                };

                return PartialView("_EditLocationPartial", editLocationViewModel);

            }
            catch (Exception e)
            {
                
                return BadRequest("Failed to update location entity");
            }
        }


        [HttpPost]
        public IActionResult EditLocation([FromForm] EditLocationViewModel updatedLocation)
        {
            if (!ModelState.IsValid || updatedLocation == null)
            {
                return PartialView("_EditLocationPartial", new EditLocationViewModel());
            }

            try
            {
                var locationToUpdate = customerService.GetLocationAddress(updatedLocation.Id);

                customerService.UpdateLocationAddress(locationToUpdate.Id,
                                updatedLocation.Country,
                                updatedLocation.City,
                                updatedLocation.Street,
                                updatedLocation.StreetNumber,
                                updatedLocation.PostalCode);

                return RedirectToPage("/Account/Manage/PickupLocations", new { area = "Identity" });
            }
            catch (Exception e)
            {
                
                return BadRequest("Failed to edit location entity");
            }
        }


        [HttpGet]
        public IActionResult Remove([FromRoute] string Id)
        {

            RemoveCustomerViewModel removeViewModel = new RemoveCustomerViewModel()
            {
                Id = Id
            };

            return PartialView("_RemoveCustomerPartial", removeViewModel);
        }

        [HttpPost]
        public IActionResult Remove(RemoveCustomerViewModel removeData)
        {
            try
            {
                customerService.RemoveCustomerById(removeData.Id);
            }
            catch (Exception e)
            {
                //log
            }

            return PartialView("_RemoveCustomerPartial", removeData);
        }
    }

}

