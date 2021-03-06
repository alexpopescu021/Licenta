using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EFVehicleRepository : EFBaseRepository<Vehicle>, IVehicleRepository
    {
        public EFVehicleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<Vehicle> GetAll()
        {
            return dbContext.Vehicles;
        }


        public Vehicle GetByRegistrationNumber(string registrationNumber)
        {
            return dbContext.Vehicles.Where(o => o.RegistrationNumber == registrationNumber).FirstOrDefault();
        }



        //public IEnumerable<RouteEntry> GetDetailsRoute(Guid vehicleId, Guid routeId)
        //{
        //    var vehicleHistory = GetHistory(vehicleId);

        //    foreach (var vehicle in vehicleHistory)
        //    {
        //        foreach(var route in vehicle.Driver.RoutesHistoric.Routes)
        //        {
        //            if (route.Id == routeId)
        //            {
        //                return route.RouteEntries;
        //            }
        //        }
        //    }

        //    return new List<RouteEntry>();
        //}

        public IEnumerable<VehicleDriver> GetHistory(Guid id)
        {
            var vdList = dbContext.VehicleDrivers
                            .Where(vehicleDriver => vehicleDriver.Vehicle.Id == id)
                            .Include(vehicleDriver => vehicleDriver.Driver)
                            .ThenInclude(vehicleDriver => vehicleDriver.RoutesHistoric)
                            .ThenInclude(vehicleDriver => vehicleDriver.Routes);


            foreach (var vehicleDriver in vdList)
            {
                foreach (var route in vehicleDriver.Driver.RoutesHistoric.Routes)
                {
                    var routeDb = dbContext.Routes.Where(r => r.Id == route.Id)
                                                  .Include(r => r.RouteEntries)
                                                  .SingleOrDefault();

                    foreach (var routeEntry in routeDb.RouteEntries)
                    {
                        var routeEntryDb = dbContext.RouteEntries.Where(re => re.Id == routeEntry.Id)
                                                                 .Include(re => re.Order)
                                                                 .ThenInclude(re => re.DeliveryAddress)
                                                                 .Include(re => re.Order)
                                                                 .ThenInclude(re => re.PickUpAddress)
                                                                 .SingleOrDefault();
                        route.RouteEntries.Add(routeEntry);
                    }
                }

            }

            return vdList;
        }
        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return dbContext.Vehicles.Where(o => o.Status == VehicleStatus.Free).AsEnumerable();
        }
    }
}
