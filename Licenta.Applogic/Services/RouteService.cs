using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.ApplicationLogic.Services
{
    public class RouteService
    {
        private readonly IPersistenceContext persistenceContext;
        private readonly IRouteRepository routeRepository;
        private readonly IDriverRepository driverRepository;

        public RouteService(IPersistenceContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
            routeRepository = persistenceContext.RouteRepository;
            driverRepository = persistenceContext.DriverRepository;
        }

        public Route CreateRoute(Vehicle vehicle)
        {
            var route = Route.Create(vehicle);
            vehicle.UpdateStatus(VehicleStatus.Busy);
            route.SetVehicle(vehicle);
            routeRepository.Add(route);
            persistenceContext.SaveChanges();
            return route;
        }

        public void ChangeVehicle(Route route, Vehicle vehicle)
        {
            route.Vehicle.UpdateStatus(VehicleStatus.Free);
            route.SetVehicle(vehicle);
            route.Vehicle.UpdateStatus(VehicleStatus.Busy);
            routeRepository.Update(route);
        }

        public Route AddEntry(string routeId, RouteEntry entry)
        {
            Guid.TryParse(routeId, out Guid routepGuid);
            var route = routeRepository.GetRouteById(routepGuid);
            routeRepository.Add(entry, route.Id);

            route.SetRouteEntry(entry);
            persistenceContext.SaveChanges();
            return route;
        }

        public Route RemoveEntry(string routeId, RouteEntry entry)
        {
            Guid.TryParse(routeId, out Guid routepGuid);
            var route = routeRepository.GetRouteById(routepGuid);
            routeRepository.Remove(entry, route.Id);

            route.DeleteRouteEntry(entry);
            persistenceContext.SaveChanges();
            return route;
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            return routeRepository.GetAll();
        }

        public Route GetById(string Id)
        {
            Guid.TryParse(Id, out Guid guid);
            return routeRepository.GetRouteById(guid);
        }

        public RouteEntry GetEntryById(string id)
        {
            Guid.TryParse(id, out Guid guid);
            return routeRepository.GetEntry(guid);
        }

        public Order GetOrderIdFromEntry(Guid id)
        {
            return routeRepository.GetOrder(id);
        }
        public bool Remove(string id)
        {
            Guid routeId = Guid.Empty;
            Guid.TryParse(id, out routeId);

            var result = routeRepository?.Remove(routeId);
            if (result == true)
            {
                persistenceContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool RemoveRoute(string id)
        {
            Guid.TryParse(id, out Guid routeId);
            var route = routeRepository.GetRouteById(routeId);

            var drivers = driverRepository.GetDriversOnRoute(route.Id);

            foreach (var driver in drivers)
            {
                driver.CurrentRoute.SetFinishTime();
                driver.SetCurrentRouteNull();
                driver.SetStatus(DriverStatus.Free);
            }

            route.Vehicle.UpdateStatus(VehicleStatus.Free);
            route.DeleteRouteEntries();
            var result = routeRepository?.Remove(routeId);
            if (result == true)
            {
                persistenceContext.SaveChanges();
                return true;
            }

            return false;
        }

        public void DeselectRoute(string id)
        {
            Guid.TryParse(id, out Guid routeId);
            var route = routeRepository.GetRouteById(routeId);

            var drivers = driverRepository.GetDriversOnRoute(route.Id);

            foreach (var driver in drivers)
            {
                driver.CurrentRoute.SetFinishTime();
                driver.SetCurrentRouteNull();
                driver.SetStatus(DriverStatus.Free);
            }
            foreach(var entry in route.RouteEntries)
            {
                entry.Order.SetStatus(OrderStatus.Created);
            }
            if(route.RouteEntries.Any(e => e.Order.Status == OrderStatus.Delivered) &&
               !route.RouteEntries.Any(e => e.Order.Status == OrderStatus.Delivered))
            {
                route.SetStatus(RouteStatus.Partially_Completed);
            }
            else if(route.RouteEntries.Any(e => e.Order.Status == OrderStatus.Delivered))
            {
                route.SetStatus(RouteStatus.Completed);
            }
            else
            {
                route.SetStatus(RouteStatus.NotAssigned);
            }
            
            route.Vehicle.UpdateStatus(VehicleStatus.Free);
            persistenceContext.SaveChanges();
        }
    }
}