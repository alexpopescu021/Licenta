﻿namespace Licenta.Model
{
    public class Employee : DataEntity
    {
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

    }
}
