using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.AppLogic.Services
{
    public class EmployeeServices
    {
        private readonly IEmployeeRepository EmployeeRepository;
        private readonly IPersistenceContext PersistenceContext;
        private readonly IDriverRepository DriverRepository;
        private readonly IDispatcherRepository DispatcherRepository;
        public EmployeeServices(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
            EmployeeRepository = persistenceContext.EmployeeRepository;
            DriverRepository = persistenceContext.DriverRepository;
            DispatcherRepository = persistenceContext.DispatcherRepository;
        }
        public void AddEmployee(string userId, string name, string email, string role)
        {
            EmployeeRepository.AddEmployee(userId, name, email, role);
        }
        public void DeleteEmployee(string userId)
        {
            var employee = GetEmployee(userId);

            if (!DriverRepository.Remove(employee.Id))
            {
                DispatcherRepository.Remove(employee.Id);
            }
            PersistenceContext.SaveChanges();

        }
        public Employee GetEmployee(string userId)
        {
            _ = new Employee();
            var employee = DriverRepository.GetByUserId(userId) ?? (Employee)DispatcherRepository.GetByUserId(userId);
            return employee;
        }

        public void UpdateEmployee(string name, string email, string role, string userId)
        {
            var employee = GetEmployee(userId);

            switch (role)
            {
                case "Driver":
                {
                    var driver = DriverRepository.GetById(employee.Id);
                    driver.SetName(name);
                    driver.SetEmail(email);
                    DriverRepository.Update(driver);
                    break;
                }
                case "Dispatcher":
                {
                    var dispatcher = DispatcherRepository.GetById(employee.Id);
                    dispatcher.SetName(name);
                    dispatcher.SetEmail(email);
                    DispatcherRepository.Update(dispatcher);
                    break;
                }
            }
        }
    }
}
