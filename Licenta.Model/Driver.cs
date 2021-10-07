﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TransportLogistics.Model
{
    public enum DriverStatus {Free, Driving }
    public class Driver: DataEntity
    {
        //public ICollection<Vehicle> VehicleHistory { get; protected set; }
       
        //public RoutesHistory RoutesHistoric { get; private set; }
        //public Route CurrentRoute { get; private set; }
        public DriverStatus Status { get; private set; }
        public string UserId { get; protected set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public void SetName(string name)
        {
            Name = name;
        }
        public void SetEmail(string email)
        {
            Email = email;
        }
        public static Driver Create(string userId, string name, string email)
        {
            Driver driver = new Driver
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserId = userId,
                Name = name,
                //RoutesHistoric = RoutesHistory.Create()
            };
            return driver;
        
        }
        //public void SetCurrentRoute(Route route)
        //{
        //    CurrentRoute = route;
        //}
        //public void SetCurrentRouteNull()
        //{
        //    CurrentRoute = null;
        //}
        //public void AddRouteToHistoric(Route route)
        //{
           
        //    RoutesHistoric.AddRoute(route);
        //}

        public void SetStatus(DriverStatus status)
        {
            Status = status;
        }
       
    }
}
