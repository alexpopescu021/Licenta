using System;
using System.Collections.Generic;
using System.Text;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.ApplicationLogic.Services
{
    public class DispatcherService
    {
        private readonly IDispatcherRepository DispatcherRepository;
        private readonly IPersistenceContext PersistenceContext;
        private readonly IDriverRepository driverRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IRouteRepository routeRepository;

        public DispatcherService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
            DispatcherRepository = persistenceContext.DispatcherRepository;
            driverRepository = persistenceContext.DriverRepository;
            vehicleRepository = persistenceContext.VehicleRepository;
            routeRepository = persistenceContext.RouteRepository;
        }
        public Dispatcher GetByUserId(string userId)
        {
            return DispatcherRepository.GetByUserId(userId);
        }

        public void ConnectDriverToRoute(Route route, Driver driver, Dispatcher dispatcher)
        {
            driver.SetCurrentRoute(route);
            route.SetStatus(RouteStatus.Assigned);
            this.routeRepository.Update(route);
            this.driverRepository.Update(driver);
            this.PersistenceContext.SaveChanges();
        } 
        
        public void DisconnectDriverToRoute(Route route, Driver driver, Dispatcher dispatcher)
        {
            driver.SetCurrentRouteNull();
            route.SetStatus(RouteStatus.NotAssigned);
            this.routeRepository.Update(route);
            this.driverRepository.Update(driver);
            this.PersistenceContext.SaveChanges();
        }
    }
}
