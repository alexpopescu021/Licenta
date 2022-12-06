using System;

namespace Licenta.Model
{
    public class Dispatcher : Employee
    {
        public static Dispatcher Create(string userId, string name, string email)
        {
            var dispatcher = new Dispatcher
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserId = userId,
                Name = name

            };
            return dispatcher;
        }
    }
}
