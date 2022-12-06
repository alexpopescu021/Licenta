using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.AppLogic.Services
{
    public class DispatcherService
    {
        private readonly IDispatcherRepository DispatcherRepository;
        private readonly IPersistenceContext PersistenceContext;
        private readonly IDriverRepository driverRepository;
        private readonly IRouteRepository routeRepository;

        public DispatcherService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
            DispatcherRepository = persistenceContext.DispatcherRepository;
            driverRepository = persistenceContext.DriverRepository;
            routeRepository = persistenceContext.RouteRepository;
        }
        public Dispatcher GetByUserId(string userId)
        {
            return DispatcherRepository.GetByUserId(userId);
        }

        public void ConnectDriverToRoute(Route route, Driver driver)
        {
            driver.SetCurrentRoute(route);
            route.SetStatus(RouteStatus.Assigned);
            routeRepository.Update(route);
            driverRepository.Update(driver);
            PersistenceContext.SaveChanges();
        }

/*
        public void DisconnectDriverToRoute(Route route, Driver driver)
        {
            driver.SetCurrentRouteNull();
            route.SetStatus(RouteStatus.NotAssigned);
            routeRepository.Update(route);
            driverRepository.Update(driver);
            PersistenceContext.SaveChanges();
        }
*/
    }
}
