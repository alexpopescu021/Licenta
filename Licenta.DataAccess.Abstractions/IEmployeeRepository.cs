namespace Licenta.DataAccess.Abstractions
{
    public interface IEmployeeRepository
    {
        void AddEmployee(string userId, string name, string email, string role);

    }
}
