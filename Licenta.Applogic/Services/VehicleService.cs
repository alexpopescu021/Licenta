﻿using System;
using System.Collections.Generic;
using System.Linq;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.AppLogic.Services
{
    public class VehicleService
    {
        private readonly IPersistenceContext persistenceContext;
        private readonly IVehicleRepository vehicleRepository;

        public VehicleService(IPersistenceContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
            vehicleRepository = persistenceContext.VehicleRepository;
        }

        public Vehicle GetById(string id)
        {
            Guid.TryParse(id, out var vehicleId);

            return vehicleRepository?.GetById(vehicleId);
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return vehicleRepository?.GetAll()
                                      .AsEnumerable();
        }

        public Vehicle Add(string name,
                           string type,
                           string registrationNumber,
                           int maximCarryWeight,
                           string vin)
        {
            var vehicleToAdd = Vehicle.Create(name, type, registrationNumber, maximCarryWeight, vin);
            vehicleRepository?.Add(vehicleToAdd);
            persistenceContext?.SaveChanges();
            return vehicleToAdd;
        }

        public bool Remove(string id)
        {
            Guid.TryParse(id, out var vehicleId);

            var result = vehicleRepository?.Remove(vehicleId);
            if (result == true)
            {
                persistenceContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Vehicle Update(string id,
                              string name,
                              string type,
                              string registrationNumber,
                              int maximCarryWeight,
                              string vin)
        {
            var vehicleToUpdate = GetById(id);
            vehicleToUpdate.Update(name, type, registrationNumber, maximCarryWeight, vin);
            persistenceContext.SaveChanges();
            return vehicleToUpdate;
        }

        public IEnumerable<VehicleDriver> GetHistory(string id)
        {
            var vehicleId = Guid.Parse(id);
            return vehicleRepository?.GetHistory(vehicleId);
        }

        //public IEnumerable<RouteEntry> GetDetailsRoute(string vehicleId, string routeId)
        //{
        //    var guidVehicleId = Guid.Parse(vehicleId);
        //    var guidRouteId = Guid.Parse(routeId);

        //    return this.vehicleRepository.GetDetailsRoute(guidVehicleId, guidRouteId);

        //}

        public bool IsVehicle(string vehicleId)
        {
            Guid.TryParse(vehicleId, out var vehicleGuid);
            if (vehicleRepository.GetById(vehicleGuid) == null)
            {
                return false;
            }

            return true;
        }
        public Vehicle GetByRegistrationNumber(string registrationNumber)
        {
            return vehicleRepository.GetByRegistrationNumber(registrationNumber);
        }

        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return vehicleRepository.GetAvailableVehicles();
        }
    }
}
