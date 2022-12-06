using Licenta.ApplicationLogic.Services;
using Licenta.Model;
using Licenta.ViewModels.Routes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Licenta.Controllers
{
    public class RoutesController : Controller
    {

        private readonly ILogger<RouteService> logger;
        private readonly RouteService routeService;
        private readonly VehicleService vehicleService;
        private readonly OrderService orderService;
        public RoutesController(ILogger<RouteService> logger, RouteService routeservice, VehicleService vehicleService, OrderService orderService)
        {
            this.logger = logger;
            routeService = routeservice;
            this.vehicleService = vehicleService;
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        private List<SelectListItem> GetVehicleList()
        {
            var vehicles = vehicleService.GetAll();

            var vehicleNames = new List<SelectListItem>();

            foreach (var vehicle in vehicles)
            {
                if (vehicle.Status == VehicleStatus.Free)
                {
                    vehicleNames.Add(new SelectListItem(vehicle.Name, vehicle.Id.ToString()));
                }
            }
            return vehicleNames;
        }
        private List<SelectListItem> GetOrderList()
        {
            var orders = orderService.GetAllOrders();
            var orderNames = new List<SelectListItem>();

            foreach (var order in orders)
            {
                var text = order.DeliveryAddress.PostalCode + " " + order.DeliveryAddress.Street;
                orderNames.Add(new SelectListItem(text, order.Id.ToString()));
            }
            return orderNames;
        }

        private List<SelectListItem> GetUnfinishedOrderList()
        {
            var orders = orderService.GetUnfinishedOrders();
            var orderNames = new List<SelectListItem>();

            foreach (var order in orders)
            {
                var text = order.DeliveryAddress.PostalCode + " " + order.DeliveryAddress.Street;
                orderNames.Add(new SelectListItem(text, order.Id.ToString()));
            }
            return orderNames;
        }

        private List<SelectListItem> GetOrderListFromRoute(string id)
        {
            var route = routeService.GetById(id);

            var routes = route.RouteEntries;

            return routes.Select(order => new SelectListItem(order.Order.DeliveryAddress.Street, order.Id.ToString())).ToList();
        }

        [HttpGet]
        public IActionResult AddOrder(string RouteId)
        {
            try
            {
                var newOrderViewModel = new AddOrderViewModel()
                {
                    OrderList = GetOrderList(),
                    RouteId = RouteId,
                    OrderType = GetOrderType()
                };
                return PartialView("_AddOrderPartial", newOrderViewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to load information for Order {@Exception}", e.Message);
                logger.LogDebug("Failed to load information for Order {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        private List<SelectListItem> GetOrderType()
        {

            var orderNames = new List<SelectListItem>
            {
                new SelectListItem("Both", OrderType.Both.ToString()),
                new SelectListItem("Delivery", OrderType.Delivery.ToString()),
                new SelectListItem("PickUp", OrderType.PickUp.ToString())
            };
            return orderNames;
        }

        [HttpPost]
        public IActionResult AddOrder([FromForm] AddOrderViewModel orderData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = orderService.GetById(orderData.OrderId);
                    var route = routeService.GetById(orderData.RouteId);
                    var entry = new RouteEntry() { Id = Guid.NewGuid() };
                    entry.SetOrder(order);
                    order.SetStatus(OrderStatus.Assigned);
                    entry.SetOrder(order);
                    entry.SetType(orderData.type);
                    routeService.AddEntry(route.Id.ToString(), entry);
                }
                return PartialView("_AddOrderPartial", orderData);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create a new Order {@Exception}", e.Message);
                logger.LogDebug("Failed to create a new Order {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult OrderList(string id)
        {
            var splitId = id.Split(';');
            var orderId = splitId[0];
            var RouteId = splitId[1];

            var model = new AddOrderViewModel()
            {
                OrderId = orderId,
                OrderList = GetUnfinishedOrderList(),
                RouteId = RouteId

            };
            return PartialView("_AddOrderPartial", model);

        }

        [HttpPost]
        public IActionResult OrderList([FromForm] AddOrderViewModel data)
        {
            return AddOrder(data.RouteId);
        }

        [HttpGet]
        public IActionResult NewRoute(string id)
        {
            try
            {
                var vehicleId = id ?? throw new ArgumentNullException(nameof(id));
                var newRouteViewModel = new NewRouteViewModel()
                {
                    VehicleId = vehicleId,
                    VehicleList = GetVehicleList()
                };

                return PartialView("_NewRoutePartial", newRouteViewModel);
            }

            catch (Exception e)
            {
                logger.LogError("Failed to load information for Route {@Exception}", e.Message);
                logger.LogDebug("Failed to load information for Route {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult NewRoute([FromForm] NewRouteViewModel routeData)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var vehicle = vehicleService.GetById(routeData.VehicleId);

                    routeService.CreateRoute(vehicle);


                    return PartialView("_NewRoutePartial", routeData);
                }

                return PartialView("_NewRoutePartial", routeData);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create a new Route {@Exception}", e.Message);
                logger.LogDebug("Failed to create a new Route {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        public IActionResult RoutesTable()
        {
            var routesView = new RouteViewModel()
            {
                Routes = routeService.GetAllRoutes()
            };
            return PartialView("_RoutesTablePartial", routesView);
        }


        [HttpGet]
        public IActionResult Remove([FromRoute] string Id)
        {

            var removeViewModel = new RemoveRouteViewModel()
            {
                Id = Id
            };

            return PartialView("_RemoveRoutePartial", removeViewModel);
        }

        [HttpPost]
        public IActionResult Remove(RemoveRouteViewModel removeData)
        {

            routeService.RemoveRoute(removeData.Id);

            return PartialView("_RemoveRoutePartial", removeData);
        }

        [HttpGet]
        public IActionResult RemoveOrderList([FromRoute] string id)
        {
            var splitId = id.Split(';');
            var RouteId = splitId[1];

            var model = new DeleteOrderViewModel()
            {
                OrderList = GetOrderListFromRoute(RouteId),
                routeId = RouteId

            };
            return PartialView("_RemoveOrderPartial", model);
        }

        [HttpPost]
        public IActionResult RemoveOrderList([FromForm] DeleteOrderViewModel data)
        {
            return RemoveOrder(data.routeId);


        }

        [HttpGet]
        public IActionResult RemoveOrder(string RouteId)
        {
            try
            {
                var newOrderViewModel = new DeleteOrderViewModel()
                {
                    OrderList = GetOrderList(),
                    routeId = RouteId
                };
                return PartialView("_RemoveOrderPartial", newOrderViewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to load information for Order {@Exception}", e.Message);
                logger.LogDebug("Failed to load information for Order {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public IActionResult RemoveOrder([FromForm] DeleteOrderViewModel orderData)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var routeEntry = routeService.GetEntryById(orderData.orderId);
                    var route = routeService.GetById(orderData.routeId);
                    var order = routeService.GetOrderIdFromEntry(routeEntry.Id);
                    order.SetStatus(OrderStatus.Created);
                    routeService.RemoveEntry(route.Id.ToString(), routeEntry);


                }
                return PartialView("_RemoveOrderPartial", orderData);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create a new Order {@Exception}", e.Message);
                logger.LogDebug("Failed to create a new Order {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult ChangeVehicle([FromRoute] string id)
        {
            var RouteId = id ?? throw new ArgumentNullException(nameof(id));
            try
            {
                var newRouteViewModel = new ChangeVehicleViewModel()
                {
                    RouteId = RouteId,
                    VehicleList = GetVehicleList()
                };

                return PartialView("_ChangeVehiclePartial", newRouteViewModel);
            }

            catch (Exception e)
            {
                logger.LogError("Failed to load information for Route {@Exception}", e.Message);
                logger.LogDebug("Failed to load information for Route {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult ChangeVehicle([FromForm] ChangeVehicleViewModel data)
        {
            var route = routeService.GetById(data.RouteId);
            var vehicle = vehicleService.GetById(data.VehicleId);
            routeService.ChangeVehicle(route, vehicle);

            return PartialView("_ChangeVehiclePartial", data);
        }


        [HttpGet]
        public IActionResult Map([FromRoute] string id)
        {
            var route = routeService.GetById(id);
            var entries = new RouteEntriesViewModel
            {
                RouteEntries = route.RouteEntries
            };

            return View(entries);
        }

        [HttpGet]
        public IActionResult ShowEntry([FromRoute] string id)
        {

            var routeEntry = routeService.GetEntryById(id);

            var routeEntryViewModel = new RouteEntryViewModel()
            {
                RouteEntry = routeEntry
            };

            return PartialView("_RouteEntryPartial", routeEntryViewModel);
        }

        public IActionResult RouteMap()
        {
            return View();

        }
    }
}