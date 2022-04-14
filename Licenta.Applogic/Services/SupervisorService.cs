using System;
using System.Collections.Generic;
using System.Text;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.ApplicationLogic.Services
{
    public class SupervisorService
    {
        private readonly IPersistenceContext persistenceContext;
        private readonly ISupervisorRepository supervisorRepository;
        private readonly IDriverRepository driverRepository;
        private readonly IVehicleRepository vehicleRepository;

        public SupervisorService(IPersistenceContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
            this.driverRepository = persistenceContext.DriverRepository;
            this.supervisorRepository = persistenceContext.SupervisorRepository;
            this.vehicleRepository = persistenceContext.VehicleRepository;
        }

        public Supervisor GetByUserId(string userId)
        {
            return this.supervisorRepository?.GetByUserId(userId);
        }
    }
}
