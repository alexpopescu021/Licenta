using Licenta.ViewModels.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Licenta.AppLogic.Services;

namespace Licenta.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleService vehicleService;
        private readonly DriverService driverService;
        private readonly ILogger<VehicleService> logger;

        public VehiclesController(VehicleService vehicleService,
                                  ILogger<VehicleService> logger,
                                  DriverService driverService)
        {
            this.vehicleService = vehicleService;
            this.logger = logger;
            this.driverService = driverService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var viewModel = new VehiclesListViewModel
                {
                    Vehicles = vehicleService.GetAll(),
                    Drivers = driverService.GetAllDrivers()
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to retrieve vehicle list {@Exception}", e.Message);
                logger.LogDebug("Failed to retrieve vehicle list {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        public IActionResult VehiclesTable()
        {
            try
            {
                var viewModel = new VehiclesListViewModel
                {
                    Vehicles = vehicleService.GetAll(),
                    Drivers = driverService.GetAllDrivers()
                };
                return PartialView("_VehiclesTable", viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to retrieve vehicle list {@Exception}", e.Message);
                logger.LogDebug("Failed to retrieve vehicle list {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new NewVehicleViewModel());
        }

        [HttpPost]
        public IActionResult Create([FromForm] NewVehicleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", new NewVehicleViewModel());
            }

            try
            {
                vehicleService.Add(viewModel.Name,
                                   viewModel.Type,
                                   viewModel.RegistrationNumber,
                                   viewModel.MaximCarryWeight,
                                   viewModel.Vin);
                return PartialView("_Create", new NewVehicleViewModel());
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create a new vehicle {@Exception}", e.Message);
                logger.LogDebug("Failed to create a new vehicle {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        public IActionResult Remove(string id)
        {
            try
            {
                vehicleService.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                logger.LogError("Failed to remove a vehicle {@Exception}", e.Message);
                logger.LogDebug("Failed to remove a vehicle {ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            try
            {
                var vehicleFromDb = vehicleService.GetById(id);
                var viewModel = new UpdateVehicleViewModel
                {
                    Id = id,
                    NameUpdate = vehicleFromDb.Name,
                    Type = vehicleFromDb.Type,
                    RegistrationNumber = vehicleFromDb.RegistrationNumber,
                    MaximCarryWeight = vehicleFromDb.MaximCarryWeightKg,
                    Vin = vehicleFromDb.Vin
                };
                return PartialView("_Update", viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to retrieve vehicle {@Exception}", e.Message);
                logger.LogDebug("Failed to retrieve vehicle {ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Update([FromForm] UpdateVehicleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Update", viewModel);
            }

            try
            {
                vehicleService.Update(viewModel.Id,
                                      viewModel.NameUpdate,
                                      viewModel.Type,
                                      viewModel.RegistrationNumber,
                                      viewModel.MaximCarryWeight,
                                      viewModel.Vin);
                return PartialView("_Update", viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to update vehicle {@Exception}", e.Message);
                logger.LogDebug("Failed to update vehicle {ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult History(string id)
        {
            try
            {
                var viewModel = new HistoryVehicleViewModel
                {
                    Vehicle = vehicleService.GetById(id),
                    VehicleDriver = vehicleService.GetHistory(id)
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to retrieve vehicle {@Exception}", e.Message);
                logger.LogDebug("Failed to retrieve vehicle {@ExceptionMessage}", e);
                return BadRequest(e.Message);
            }
        }
    }
}