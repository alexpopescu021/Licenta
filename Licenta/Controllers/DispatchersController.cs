﻿using Licenta.ViewModels.Dispatchers;
using Licenta.ViewModels.Routes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Licenta.AppLogic.Services;

namespace Licenta.Controllers
{
    public class DispatchersController : Controller
    {

        private readonly DriverService driverService;
        private readonly ILogger<DriverService> logger;
        private readonly VehicleService vehicleService;
        private readonly RouteService routeService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly DispatcherService dispatcherService;
        public DispatchersController(DriverService driverService, ILogger<DriverService> logger
            , VehicleService vehicleService, UserManager<IdentityUser> userManager, DispatcherService dispatcherService,
            RouteService routeService)
        {
            this.driverService = driverService;
            this.logger = logger;
            this.vehicleService = vehicleService;
            this.userManager = userManager;
            this.dispatcherService = dispatcherService;
            this.routeService = routeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DriversTable()
        {
            try
            {
                var driversViewModel = new DriversListViewModel()
                {
                    Drivers = driverService.GetAllDrivers()
                };

                return PartialView("_DriversTablePartial", driversViewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to load Driver entities {@Exception}", e.Message);
                logger.LogDebug("Failed to load Driver entities {@ExceptionMessage}", e);
                return BadRequest("Failed to load Driver entities");
            }

        }
        [HttpGet]
        public IActionResult AssignRoute([FromRoute] string id)
        {
            var driverId = id;
            var assignRouteViewModel = new AssignRouteViewModel
            {
                DriverId = driverId,
                RouteList = GetRouteList()
            };
            return PartialView("_AssignRoutePartial", assignRouteViewModel);

        }

        [HttpPost]
        public IActionResult AssignRoute([FromForm] AssignRouteViewModel data)
        {
            var userId = userManager.GetUserId(User);
            var dispatcherDb = dispatcherService.GetByUserId(userId);

            if (ModelState.IsValid)
            {
                var driver = driverService.GetByUserId(data.DriverId);
                var route = routeService.GetById(data.RouteId);
                foreach(var entry in route.RouteEntries)
                {
                    entry.Order.SetStatus(Model.OrderStatus.Assigned);
                }
                dispatcherService.ConnectDriverToRoute(route, driver);
            }
            return PartialView("_AssignRoutePartial", data);

        }

        [HttpGet]
        public IActionResult RemoveRouteFromDriver([FromRoute] string id)
        {

            var removeViewModel = new RemoveRouteViewModel()
            {
                Id = id
            };

            return PartialView("_RemoveRoutePartial", removeViewModel);
        }

        [HttpPost]
        public IActionResult RemoveRouteFromDriver(RemoveRouteViewModel removeData)
        {

            routeService.DeselectRoute(removeData.Id);

            return PartialView("_RemoveRoutePartial", removeData);
        }
        private List<SelectListItem> GetRouteList()
        {
            var routes = routeService.GetAllRoutes();
            var routesNames = new List<SelectListItem>();

            foreach (var route in routes)
            {
                var text = route.Vehicle.Name + " " + route.StartTime;
                routesNames.Add(new SelectListItem(text, route.Id.ToString()));
            }
            return routesNames;
        }

    }
}