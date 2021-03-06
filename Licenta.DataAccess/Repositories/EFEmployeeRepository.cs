using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        public EFEmployeeRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;

        }

        private readonly ApplicationDbContext DbContext;

        public void AddEmployee(string userId, string name, string email, string role)
        {
            if (role == "Driver")
            {
                var driver = Driver.Create(userId, name, email);
                DbContext.Drivers.Add(driver);
            }
            else if (role == "Dispatcher")
            {
                var dispatcher = Dispatcher.Create(userId, name, email);
                DbContext.Dispatchers.Add(dispatcher);
            }
            DbContext.SaveChanges();
        }

    }
}
