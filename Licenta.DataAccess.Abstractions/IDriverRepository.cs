using Licenta.Model;
using System;
using System.Collections.Generic;

namespace Licenta.DataAccess.Abstractions
{
    public interface IDriverRepository : IBaseRepository<Driver>
    {
        Driver GetByUserId(string userId);
        ICollection<RouteEntry> GetRouteEntries(Guid id);
        IEnumerable<Driver> GetDriversOnRoute(Guid routeId);
        Driver GetDriverWithRoute(Guid id);
        IEnumerable<Driver> GetAllDriversWithRoute(Guid id);
        new IEnumerable<Driver> GetAll();
        RoutesHistory GetRoutesHistory(Guid id);
        Driver GetRouteWithVehicle(Guid id);
        Driver GetDriverWithRouteFromUserId(Guid driverGuid);
    }
}
