using Licenta.Model;
using System;
using System.Collections.Generic;

namespace Licenta.DataAccess.Abstractions
{
    public interface IRouteRepository : IBaseRepository<Route>
    {
        Route GetRouteById(Guid routeId);
        new IEnumerable<Route> GetAll();
        RouteEntry Add(RouteEntry entry, Guid routeId);
        //Route Create();
        IEnumerable<RouteEntry> GetAllRouteEntries();

        RouteEntry GetEntry(Guid id);
        void Remove(RouteEntry entry, Guid routeId);
        Order GetOrder(Guid guid);
    }
}
