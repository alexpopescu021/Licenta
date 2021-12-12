﻿using Licenta.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Licenta.ApplicationLogic.Services;
using Licenta.Model;
using Microsoft.AspNet.Identity;

namespace Licenta.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly CustomerService customerService;

        public CustomerController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager, CustomerService _customerService)
        {
            userManager = _userManager;
            customerService = _customerService;
        }
        public IActionResult Index()
        {
            return View();
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
            var user = User.Identity.GetUserId();
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



        //[HttpGet]
        //public IActionResult Remove([FromRoute] string Id)
        //{

        //    RemoveCustomerViewModel removeViewModel = new RemoveCustomerViewModel()
        //    {
        //        Id = Id
        //    };

        //    return PartialView("_RemoveCustomerPartial", removeViewModel);
        //}

        //[HttpPost]
        //public IActionResult Remove(RemoveCustomerViewModel removeData)
        //{
        //    try
        //    {
        //        customerService.RemoveCustomerById(removeData.Id);
        //    }
        //    catch (CustomerNotFoundException notFound)
        //    {
        //        logger.LogError("Failed to find the customer entity {@Exception}", notFound.Message);
        //        logger.LogDebug("Failed to find the customer entity {@ExceptionMessage}", notFound);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.LogError("Failed to remove the customer entity {@Exception}", e.Message);
        //        logger.LogDebug("Failed to remove customer entity {@ExceptionMessage}", e);
        //    }

        //    return PartialView("_RemoveCustomerPartial", removeData);
        //}
    }

}

