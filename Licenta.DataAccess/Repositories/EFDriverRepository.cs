using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EFDriverRepository : EFBaseRepository<Driver>, IDriverRepository
    {
        public EFDriverRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public Driver GetByUserId(string userId)
        {
            var driver = dbContext.Drivers.FirstOrDefault(o => o.UserId == userId);
            return driver;
        }
        public Driver GetDriverWithRoute(Guid id)
        {
            return dbContext.Drivers
                .Include(o => o.CurrentRoute)
                .ThenInclude(o => o.RouteEntries)
                .ThenInclude(a => a.Order)
                .Include(o => o.RoutesHistoric).FirstOrDefault(o => o.Id == id);
        }
        public Driver GetDriverWithRouteFromUserId(Guid id)
        {
            return dbContext.Drivers
                .Include(o => o.CurrentRoute)
                .ThenInclude(o => o.RouteEntries)
                .ThenInclude(a => a.Order)
                .Include(o => o.RoutesHistoric).FirstOrDefault(o => o.UserId == id.ToString());
        }
        public IEnumerable<Driver> GetDriversOnRoute(Guid routeId)
        {
            return dbContext.Drivers
                .Include(o => o.CurrentRoute)
                .ThenInclude(o => o.RouteEntries)
                .Include(o => o.RoutesHistoric)
                .ThenInclude(o => o.Routes)
                .ThenInclude(o => o.RouteEntries)
                .Where(o => o.CurrentRoute.Id == routeId)
                .AsEnumerable();

        }

        public IEnumerable<Driver> GetAllDriversWithRoute(Guid id)
        {
            return dbContext.Drivers
                .Include(o => o.CurrentRoute)
                .ThenInclude(o => o.RouteEntries)
                .Include(o => o.RoutesHistoric)
                .Where(o => o.Id == id).AsEnumerable();
        }

        public ICollection<RouteEntry> GetRouteEntries(Guid id)
        {
            var driver = GetDriverWithRoute(id);
            ICollection<RouteEntry> completeRouteEntries = new List<RouteEntry>();
            if (driver.CurrentRoute != null && driver.CurrentRoute.RouteEntries.Count > 0)
            {
                var routeEntries = driver.CurrentRoute.RouteEntries;
                foreach (var routeEntry in routeEntries)
                {
                    var tempRouteEntry = dbContext.RouteEntries.Include(o => o.Order).FirstOrDefault(o => o.Id == routeEntry.Id);
                    var order = dbContext.Orders.Include(o => o.PickUpAddress).Include(o => o.DeliveryAddress).Include(o => o.Recipient)
                        .ThenInclude(o => o.ContactDetails).Include(o => o.Sender).ThenInclude(o => o.ContactDetails).FirstOrDefault(o => o.Id == tempRouteEntry.Order.Id);
                    tempRouteEntry?.SetOrder(order);

                    completeRouteEntries.Add(tempRouteEntry);
                }
            }
            return completeRouteEntries;
        }

        public new IEnumerable<Driver> GetAll()
        {
            var driversList = dbContext.Drivers
                                .Include(driver => driver.CurrentRoute)
                                .ThenInclude(driver => driver.RouteEntries)
                                .ThenInclude(driver => driver.Order)
                                .ThenInclude(driver => driver.PickUpAddress)
                                .Include(driver => driver.CurrentRoute.RouteEntries)
                                .ThenInclude(driver => driver.Order.DeliveryAddress)
                                .Include(driver => driver.RoutesHistoric)
                                .ThenInclude(driver => driver.Routes)
                                .ThenInclude(d => d.Vehicle)
                                .ToList();

            foreach (var driver in driversList)
            {
                var driverCurrentRoute = driver.CurrentRoute;
                if (driverCurrentRoute != null)
                {
                    foreach (var routeEntry in driverCurrentRoute.RouteEntries)
                    {
                        var routeEntryDb = dbContext.RouteEntries
                            .SingleOrDefault(r => r.Id == routeEntry.Id);
                        driverCurrentRoute.RouteEntries.Add(routeEntryDb);
                    }
                }
            }

            foreach (var driver in driversList)
            {
                if (driver.RoutesHistoric != null)
                {
                    foreach (var route in driver.RoutesHistoric.Routes)
                    {
                        var routeDb = dbContext.Routes.Where(r => r.Id == route.Id)
                                                      .Include(r => r.RouteEntries)
                                                      .SingleOrDefault();

                        foreach (var routeEntry in routeDb.RouteEntries)
                        {
                            dbContext.RouteEntries.Where(re => re.Id == routeEntry.Id)
                                .Include(re => re.Order)
                                .ThenInclude(re => re.DeliveryAddress)
                                .Include(re => re.Order)
                                .ThenInclude(re => re.PickUpAddress)
                                .SingleOrDefault();
                            route.RouteEntries.Add(routeEntry);
                        }
                    }
                }

            }

            return driversList;
        }
        public RoutesHistory GetRoutesHistory(Guid id)
        {
            var driver = GetById(id);
            var driverRoutes = dbContext.Drivers.Include(o => o.RoutesHistoric)
                .ThenInclude(o => o.Routes).FirstOrDefault(o => o.Id == driver.Id);
            ICollection<Route> routes = new List<Route>();
            if (driverRoutes != null && driverRoutes.RoutesHistoric.Routes.Count > 0)
            {
                foreach (var route in driverRoutes.RoutesHistoric.Routes)
                {
                    var routeOrders = dbContext.Routes.Include(o => o.RouteEntries).FirstOrDefault(o => o.Id == route.Id);
                    ICollection<RouteEntry> routeEntries = new List<RouteEntry>();
                    if (routeOrders is { RouteEntries: { } })
                    {
                        foreach (var routeEntry in routeOrders.RouteEntries)
                        {
                            var temp = dbContext.RouteEntries.Include(o => o.Order).ThenInclude(o => o.PickUpAddress)
                                .Include(o => o.Order).ThenInclude(o => o.DeliveryAddress).FirstOrDefault(o => o.Id == routeEntry.Id);
                            routeEntries.Add(temp);
                        }
                        routeOrders.SetRouteEntries(routeEntries);
                        routes.Add(routeOrders);
                    }

                }
                driver.RoutesHistoric.SetRoutes(routes);
            }
            return driver.RoutesHistoric;
        }

        public Driver GetRouteWithVehicle(Guid id)
        {
            return dbContext.Drivers.Include(o => o.CurrentRoute).ThenInclude(o => o.Vehicle).FirstOrDefault(o => o.Id == id);
        }
    }
}
