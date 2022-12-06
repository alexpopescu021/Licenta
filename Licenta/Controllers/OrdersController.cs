using Licenta.Model;
using Licenta.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Licenta.AppLogic.Services;
using Licenta.ViewModels.Customers;

namespace Licenta.Controllers
{
    public class OrdersController : Microsoft.AspNetCore.Mvc.Controller
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
            var ordersView = new OrderViewModel();
            if (User.IsInRole("Dispatcher"))
            {
                ordersView.Orders = orderService.GetAllOrders();
            }
            else if (!User.IsInRole("Admin") && !User.IsInRole("Driver") && !User.IsInRole("Dispatcher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var senderId = customerService.GetCustomerById(userId).Id.ToString();
                ordersView.Orders = orderService.GetOrdersForCurrentCustomer(senderId);
            }

            return PartialView("_OrdersTablePartial", ordersView);
        }

        private static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem CreateListItem(LocationAddress location)
        {
            var dropdownText = location.Tag == null ? $"{ location.PostalCode }, { location.Street}" : $"{ location.Tag }";
            var selectLocation = new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(dropdownText, location.Id.ToString());
            return selectLocation;
        }

        private List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetCustomerList()
        {
            var customers = customerService.GetAllCustomers();
            var customerNames = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

            foreach (var customer in customers)
            {
                if (customer.LocationAddresses.Count > 0)
                {
                    customerNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(customer.Name, customer.Id.ToString()));
                }
            }
            return customerNames;
        }

        private List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetLocationsList(string customerId)
        {
            var locations = customerService.GetCustomerAddresses(customerId);

            return locations.Select(CreateListItem).ToList();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult NewOrder(string id)
        {
            var pickupLocations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            var deliveryLocations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            var unchosenLocations = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            string senderId = null;
            string pickupId = null;
            string deliveryId = null;

            try
            {
                if (id != null)
                {
                    var splitId = id.Split(';');
                    senderId = splitId[0];
                    var locationId = splitId[1];
                    var locationType = splitId[3];

                    string secondLocationId = null;
                    if (locationId != splitId[2])
                    {
                        secondLocationId = splitId[2];
                    }

                    if (customerService.IsLocation(locationId))
                    {
                        var chosenLocation = customerService.GetLocationAddress(locationId);
                        var customer = customerService.GetCustomerById(senderId);

                        unchosenLocations.AddRange(from location in customer.LocationAddresses where location.Id != chosenLocation.Id select CreateListItem(location));

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
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    senderId = customerService.GetCustomerById(userId).Id.ToString();
                    pickupLocations.AddRange(GetLocationsList(senderId));
                    deliveryLocations.AddRange(GetLocationsList(senderId));
                    deliveryLocations.RemoveAt(0);
                }

                var newOrderViewModel = new NewOrderViewModel()
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

        [Microsoft.AspNetCore.Mvc.HttpPost]
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
                        orderData.StreetNumber, orderData.PostalCode, null);

                    customerService.AddLocation(bonusLocation);

                    orderData.PickupLocationId ??= bonusLocation.Id.ToString();

                    orderData.DeliveryLocationId ??= bonusLocation.Id.ToString();
                }
                
                var recipient = orderService.CreateNewRecipient(orderData.RecipientName,
                            orderData.RecipientPhoneNo,
                            orderData.RecipientEmail);
                var awb = CreateAwb(8);
                orderData.Awb = awb;
                orderService.CreateOrder(recipient,
                    sender,
                    orderData.PickupLocationId,
                    orderData.DeliveryLocationId,
                    orderData.Price,
                    orderData.Awb);
                return PartialView("_NewOrderPartial", orderData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public string GetAwb(string awb)
        {
            var response = orderService.GetByAwb(awb);
            return response != null ? $"Your order is {response.Status}" : "No order found with the specific AWB";
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.JsonResult ActionName(string yourValue)
        {

            return Json(new { success = true, result = yourValue }, JsonRequestBehavior.AllowGet);
        }

        private static readonly Random Rd = new Random();
        internal static string CreateAwb(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            var chars = new char[stringLength];

            for (var i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[Rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
