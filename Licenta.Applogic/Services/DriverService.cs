using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.ApplicationLogic.Services
{
    public class DriverService
    {
        private readonly IDriverRepository DriverRepository;
        private readonly IPersistenceContext PersistenceContext;
        private readonly IRouteRepository RouteRepository;
        private readonly OrderService OrderService;
        public DriverService(IPersistenceContext persistenceContext, OrderService orderService)
        {
            PersistenceContext = persistenceContext;
            DriverRepository = persistenceContext.DriverRepository;
            RouteRepository = persistenceContext.RouteRepository;
            OrderService = orderService;
        }
        public Driver GetByUserId(string userId)
        {
            return DriverRepository.GetByUserId(userId);
        }
        public ICollection<RouteEntry> GetRouteEntries(Guid id)
        {
            return DriverRepository.GetRouteEntries(id);

        }

        public Driver GetDriverWithRoute(string driverId)
        {
            Guid.TryParse(driverId, out Guid driverGuid);
            return DriverRepository.GetDriverWithRoute(driverGuid);
        }
        public Driver GetDriverWithRouteFromUserId(string driverId)
        {
            Guid.TryParse(driverId, out Guid driverGuid);
            return DriverRepository.GetDriverWithRouteFromUserId(driverGuid);
        }
        public Driver EndCurrentRoute(Driver driver)
        {
            driver = DriverRepository.GetDriverWithRoute(driver.Id);

            if(driver.CurrentRoute.RouteEntries.Any(r => r.Order.Status == OrderStatus.Delivered) &&
               !driver.CurrentRoute.RouteEntries.Any(r => r.Order.Status == OrderStatus.Delivered))
                {
                driver.CurrentRoute.SetStatus(RouteStatus.Partially_Completed);
            }
            else if(driver.CurrentRoute.RouteEntries.All(r => r.Order.Status == OrderStatus.Delivered))
            {
                driver.CurrentRoute.SetStatus(RouteStatus.Completed);
            }
            driver.CurrentRoute.SetFinishTime();
            driver.SetCurrentRouteNull();
            SetDriverStatus(driver, DriverStatus.Free);
            DriverRepository.Update(driver);
            return driver;
        }

        public void SetDriverStatus(Driver driver, DriverStatus status)
        {
            driver.SetStatus(status);
            var routeEntries = GetRouteEntries(driver.Id);
            OrderService.StartRoute(routeEntries);
            if (status == DriverStatus.Driving)
            {
                driver.CurrentRoute.SetStartTime();
            }

            DriverRepository.Update(driver);
            PersistenceContext.SaveChanges();
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            return DriverRepository.GetAll();
        }

        public RoutesHistory GetRoutesHistory(Guid id)
        {
            return DriverRepository.GetRoutesHistory(id);
        }
        public Route GetRouteById(Guid id)
        {

            return RouteRepository.GetRouteById(id);
        }

        public Driver GetById(Guid id)
        {
            return DriverRepository.GetById(id);
        }

        public void UpdateDriver(Driver driver, string newName, string newEmail)
        {
            driver.SetEmail(newEmail);
            driver.SetName(newName);
        }
    }
}
