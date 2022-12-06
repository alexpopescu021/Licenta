using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    internal class EFRouteRepository : EFBaseRepository<Route>, IRouteRepository
    {
        public EFRouteRepository(ApplicationDbContext context) : base(context)
        { }

        public IEnumerable<RouteEntry> GetAllRouteEntries()
        {
            return dbContext.RouteEntries
                        .Include(r => r.Order)
                        .AsEnumerable();
        }


        public Route GetRouteById(Guid routeId)
        {
            var route = dbContext.Routes.Include(v => v.Vehicle).Include(o => o.RouteEntries).ThenInclude(o => o.Order).FirstOrDefault(o => o.Id == routeId);
            ICollection<RouteEntry> routeEntries = new List<RouteEntry>();
            if (route.RouteEntries != null)
            {
                foreach (var routeEntry in route.RouteEntries)
                {
                    var temp = dbContext.RouteEntries.Include(o => o.Order).ThenInclude(o => o.PickUpAddress)
                        .Include(o => o.Order).ThenInclude(o => o.DeliveryAddress).FirstOrDefault(o => o.Id == routeEntry.Id);
                    routeEntries.Add(temp);
                }
                route.SetRouteEntries(routeEntries);
            }
            return route;
        }

        public new IEnumerable<Route> GetAll()
        {
            return dbContext.Routes
                        .Include(r => r.Vehicle)
                        .Include(e => e.RouteEntries)
                        .ThenInclude(e => e.Order)
                        .ThenInclude(o => o.PickUpAddress)
                        .Include(o => o.RouteEntries)
                        .ThenInclude(o => o.Order)
                        .ThenInclude(o => o.DeliveryAddress)
                        .AsEnumerable();
        }

        public RouteEntry Add(RouteEntry entry, Guid routeId)
        {
            var route = dbContext.Routes.Include(r => r.RouteEntries).FirstOrDefault(e => e.Id == routeId);
            route.RouteEntries.Add(entry);

            dbContext.RouteEntries.Add(entry);
            dbContext.SaveChanges();
            return entry;

        }

        public RouteEntry GetEntry(Guid id)
        {
            //var Trailer = dbContext.Trailers.Where(trailer => trailer.Id == trailerId).FirstOrDefault();
            var entry = dbContext.RouteEntries
                        .Where(e => e.Id == id)
                        .Include(e => e.Order)
                        .Include(o => o.Order.PickUpAddress)
                        .Include(o => o.Order.DeliveryAddress)
                        .Include(o => o.Order.Sender)
                        .Include(o => o.Order.Recipient)
                        .FirstOrDefault();
            return entry;
        }
        public Order GetOrder(Guid guid)
        {
            var entry = dbContext.RouteEntries
                .FirstOrDefault(e => e.Id == guid);
            var order = dbContext.Orders.FirstOrDefault(o => o.Id == entry.Order.Id);
            return order;
        }
        public void Remove(RouteEntry entry, Guid routeId)
        {

            var route = dbContext.Routes.Include(r => r.RouteEntries).FirstOrDefault(e => e.Id == routeId);
            foreach (var dbentry in route.RouteEntries)
            {

                if (dbentry.Order.Id == entry.Order.Id)
                {

                    route.RouteEntries.Remove(dbentry);
                    dbContext.RouteEntries.Remove(dbentry);
                    dbContext.SaveChanges();
                    break;
                }

            }

        }

    }
}
