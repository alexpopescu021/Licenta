using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.DataAccess.Repositories
{
    public class EfEmployeeRepository : IEmployeeRepository
    {
        public EfEmployeeRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;

        }

        private readonly ApplicationDbContext DbContext;

        public void AddEmployee(string userId, string name, string email, string role)
        {
            switch (role)
            {
                case "Driver":
                {
                    var driver = Driver.Create(userId, name, email);
                    DbContext.Drivers.Add(driver);
                    break;
                }
                case "Dispatcher":
                {
                    var dispatcher = Dispatcher.Create(userId, name, email);
                    DbContext.Dispatchers.Add(dispatcher);
                    break;
                }
            }

            DbContext.SaveChanges();
        }

    }
}
