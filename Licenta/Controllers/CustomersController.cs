using Licenta.Model;
using Licenta.ViewModels.Customers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Licenta.AppLogic.Services;

namespace Licenta.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerService customerService;

        public CustomersController(CustomerService customerService)
        {
            this.customerService = customerService;
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
            catch (Exception)
            {
                //log
            }

            return PartialView("_CustomerTable", customerViewModel);
        }

        [HttpGet]
        public IActionResult UpdateCustomer([FromRoute] string id)
        {
            try
            {
                var customerToUpdate = customerService.GetCustomerById(id);

                var editCustomerViewModel = new UpdateCustomerViewModel()
                {
                    Id = id,
                    Name = customerToUpdate.Name,
                    PhoneNo = customerToUpdate.ContactDetails.PhoneNo,
                    Email = customerToUpdate.ContactDetails.Email
                };

                return PartialView("_UpdateCustomerPartial", editCustomerViewModel);

            }
            catch (Exception)
            {
                //log
                return BadRequest("Failed to edit customer entity");
            }
        }

        [HttpPost]
        public IActionResult UpdateCustomer([FromForm] UpdateCustomerViewModel updatedData, [FromRoute] string id)
        {
            if (updatedData.Name == null ||
                   updatedData.PhoneNo == null)
            {
                return PartialView("_UpdateCustomerPartial", new NewCustomerViewModel());
            }

            try
            {
                customerService.GetCustomerById(id);
                customerService.UpdateCustomer(Guid.Parse(id),
                                                updatedData.Name,
                                                updatedData.PhoneNo,
                                                updatedData.Email);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //log
                return BadRequest("Failed to edit customer entity");
            }
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var locationModel = new NewLocationViewModel()
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
                            locationData.PostalCode,
                            locationData.Tag);

                customerService.AddLocationToCustomer(customer.Id, newLocation);

                return RedirectToPage("/Account/Manage/PickupLocations", new { area = "Identity" });
            }
            catch (Exception) { return null; }

        }

        public IActionResult AddressTable()
        {
            var user = User.Identity.GetUserId();
            var addressView = new AddressViewModel()
            {
                Addresses = customerService.GetCustomerAddresses(user)
            };

            return PartialView("_AddressTablePartial", addressView);
        }


        [HttpGet]
        public IActionResult EditLocation([FromRoute] string id)
        {
            try
            {
                var locationToUpdate = customerService.GetLocationAddress(id);

                var editLocationViewModel = new EditLocationViewModel()
                {
                    Id = id,
                    Country = locationToUpdate.Country,
                    City = locationToUpdate.City,
                    Street = locationToUpdate.Street,
                    StreetNumber = locationToUpdate.StreetNumber,
                    PostalCode = locationToUpdate.PostalCode,
                    Tag = locationToUpdate.Tag
                };

                return PartialView("_EditLocationPartial", editLocationViewModel);

            }
            catch (Exception)
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
                                updatedLocation.PostalCode,
                                updatedLocation.Tag);

                return RedirectToPage("/Account/Manage/PickupLocations", new { area = "Identity" });
            }
            catch (Exception)
            {

                return BadRequest("Failed to edit location entity");
            }
        }


        [HttpGet]
        public IActionResult Remove([FromRoute] string id)
        {

            var removeViewModel = new RemoveCustomerViewModel()
            {
                Id = id
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
            catch (Exception)
            {
                //log
            }

            return PartialView("_RemoveCustomerPartial", removeData);
        }
    }

}

