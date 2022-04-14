using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Model;

namespace Licenta.DataAccess.Abstractions
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        IEnumerable<VehicleDriver> GetHistory(Guid id);
        //IEnumerable<RouteEntry> GetDetailsRoute(Guid guidVehicleId1, Guid guidVehicleId2);
        Vehicle GetByRegistrationNumber(string registrationNumber);
        IEnumerable<Vehicle> GetAvailableVehicles();
    }
}
