﻿using System;

namespace Licenta.Model
{
    public enum DriverStatus { Free, Driving }
    public class Driver : Employee
    {
        public RoutesHistory RoutesHistoric { get; private set; }
        public Route CurrentRoute { get; private set; }
        public DriverStatus Status { get; private set; }
        public static Driver Create(string userId, string name, string email)
        {
            var driver = new Driver
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserId = userId,
                Name = name,
                //RoutesHistoric = RoutesHistory.Create()
            };
            return driver;

        }
        public void SetCurrentRoute(Route route)
        {
            CurrentRoute = route;
        }
        public void SetCurrentRouteNull()
        {
            CurrentRoute = null;
        }
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
