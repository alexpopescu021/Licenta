using Licenta.ApplicationLogic.Services;
using Licenta.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licenta.Model;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

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

        private SelectListItem CreateListItem(LocationAddress location)
        {
            string dropdownText = $"{ location.PostalCode }, { location.Street}";
            SelectListItem selectLocation = new SelectListItem(dropdownText, location.Id.ToString());
            return selectLocation;
        }

        private List<SelectListItem> GetCustomerList()
        {
            var customers = customerService.GetAllCustomers();
            List<SelectListItem> customerNames = new List<SelectListItem>();

            foreach (var customer in customers)
            {
                if (customer.LocationAddresses.Count > 0)
                {
                    customerNames.Add(new SelectListItem(customer.Name, customer.Id.ToString()));
                }
            }
            return customerNames;
        }

        private List<SelectListItem> GetLocationsList(string customerId)
        {
            List<SelectListItem> locationsList = new List<SelectListItem>();
            var locations = customerService.GetCustomerAddresses(customerId);
            foreach (var location in locations)
            {
                locationsList.Add(CreateListItem(location));
            }

            return locationsList;
        }

        [HttpGet]
        public IActionResult NewOrder(string id)
        {
            List<SelectListItem> pickupLocations = new List<SelectListItem>();
            List<SelectListItem> deliveryLocations = new List<SelectListItem>();
            List<SelectListItem> unchosenLocations = new List<SelectListItem>();
            string senderId = null;
            string pickupId = null;
            string deliveryId = null;

            try
            {
                if (id != null)
                {
                    string[] splitId = id.Split(';');
                    senderId = splitId[0];
                    string locationId = splitId[1];
                    string locationType = splitId[3];

                    string secondLocationId = null;
                    if (locationId != splitId[2])
                    {
                        secondLocationId = splitId[2];
                    }

                    if (customerService.IsLocation(locationId))
                    {
                        var chosenLocation = customerService.GetLocationAddress(locationId);
                        var customer = customerService.GetCustomerById(senderId);

                        foreach (var location in customer.LocationAddresses)
                        {
                            if (location.Id != chosenLocation.Id)
                            {
                                unchosenLocations.Add(CreateListItem(location));
                            }
                        }

                        if (locationType.Equals(NewOrderViewModel.LocationType.Pickup.ToString()))
                        {
                            pickupId = locationId;
                            deliveryId = secondLocationId;
                            pickupLocations.Add(CreateListItem(chosenLocation));
                            pickupLocations.AddRange(unchosenLocations);
                            deliveryLocations.AddRange(unchosenLocations);
                        }

                        if (locationType.Equals(NewOrderViewModel.LocationType.Delivery.ToString()))
                        {
                            deliveryId = locationId;
                            pickupId = secondLocationId;
                            deliveryLocations.Add(CreateListItem(chosenLocation));
                            deliveryLocations.AddRange(unchosenLocations);
                            pickupLocations.AddRange(unchosenLocations);
                        }
                    }
                    else
                    {
                        var locations = GetLocationsList(senderId);
                        pickupLocations.AddRange(locations);
                        deliveryLocations.AddRange(locations);
                        deliveryLocations.RemoveAt(0);
                    }
                }

                if (senderId == null && GetCustomerList().Count > 0)
                {
                    senderId = GetCustomerList().ElementAt(0).Value;
                    pickupLocations.AddRange(GetLocationsList(senderId));
                    deliveryLocations.AddRange(GetLocationsList(senderId));
                    deliveryLocations.RemoveAt(0);
                }

                NewOrderViewModel newOrderViewModel = new NewOrderViewModel()
                {
                    CustomerList = GetCustomerList(),
                    PickupLocations = pickupLocations,
                    DeliveryLocations = deliveryLocations,
                    SenderId = senderId,
                    DeliveryLocationId = deliveryId,
                    PickupLocationId = pickupId
                };

                return PartialView("_NewOrderPartial", newOrderViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult NewOrder([FromForm] NewOrderViewModel orderData)
        {
            try
            {
                var sender = customerService.GetCustomerById(orderData.SenderId);

                if (orderData.PickupLocationId == null && orderData.DeliveryLocationId == null)
                {
                    return PartialView("_NewOrderPartial", orderData);
                }
                if (orderData.PickupLocationId == null || orderData.DeliveryLocationId == null)
                {
                    var bonusLocation = LocationAddress.Create(orderData.Country,
                        orderData.City, orderData.Street,
                        orderData.StreetNumber, orderData.PostalCode);



                    customerService.AddLocation(bonusLocation);

                    if (orderData.PickupLocationId == null)
                    {
                        orderData.PickupLocationId = bonusLocation.Id.ToString();
                    }

                    if (orderData.DeliveryLocationId == null)
                    {
                        orderData.DeliveryLocationId = bonusLocation.Id.ToString();
                    }
                }

                var recipient = orderService.CreateNewRecipient(orderData.RecipientName,
                            orderData.RecipientPhoneNo,
                            orderData.RecipientEmail);

                orderService.CreateOrder(recipient,
                    sender,
                    orderData.PickupLocationId,
                    orderData.DeliveryLocationId,
                    orderData.Price);

                return RedirectToAction("Index");
                //return PartialView("_NewOrderPartial", orderData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
