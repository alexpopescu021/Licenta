﻿using System;
using System.Collections.Generic;

namespace Licenta.Model
{
    public enum RouteStatus { NotAssigned, Assigned, PartiallyCompleted, Completed }
    public class Route : DataEntity
    {
        public ICollection<RouteEntry> RouteEntries { get; set; }
        public Vehicle Vehicle { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime FinishTime { get; private set; }
        public RouteStatus Status { get; private set; }
        public static Route Create()
        {
            var route = new Route()
            {
                Id = Guid.NewGuid()
            };
            return route;
        }

        public static Route Create(Vehicle vehicle)
        {
            var route = new Route()
            {
                Id = Guid.NewGuid(),
                Vehicle = vehicle,
                Status = RouteStatus.NotAssigned

            };
            return route;
        }

        public void SetStatus(RouteStatus status)
        {
            this.Status = status;
        }
        public void SetRouteEntries(ICollection<RouteEntry> routeEntries)
        {
            RouteEntries = routeEntries;
        }

        public void SetRouteEntry(RouteEntry routeEntrie)
        {
            RouteEntries.Add(routeEntrie);
        }

        public void SetStartTime()
        {
            StartTime = DateTime.UtcNow;
        }
        public void SetFinishTime()
        {
            FinishTime = DateTime.UtcNow;

        }
        public void DeleteRouteEntry(RouteEntry routeEntry)
        {
            RouteEntries.Remove(routeEntry);
        }
        public void DeleteRouteEntries()
        {
            RouteEntries.Clear();
        }

        public void SetVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }
    }
}
