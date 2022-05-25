using System;

namespace Licenta.Model
{
    public enum VehicleStatus { Free, Busy, UnAvailable }
    public class Vehicle : DataEntity
    {
        public string Name { get; protected set; }
        public string Type { get; protected set; }
        public string RegistrationNumber { get; protected set; }
        public int MaximCarryWeightKg { get; protected set; }
        public string VIN { get; protected set; }
        public VehicleStatus Status { get; protected set; }

        public static Vehicle Create(string Name, string Type, string registrationNumber, int MaximCarryWeight, string VIN)
        {
            if (MaximCarryWeight < 0)
            {
                throw new Exception("Maxim carry weight can not be less than 0");
            }

            return new Vehicle
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Type = Type,
                RegistrationNumber = registrationNumber,
                MaximCarryWeightKg = MaximCarryWeight,
                VIN = VIN,
                Status = VehicleStatus.Free
            };
        }

        public Vehicle Update(string name, string type, string registrationNumber, int maximCarryWeight, string vin)
        {
            Name = name;
            Type = type;
            RegistrationNumber = registrationNumber;
            MaximCarryWeightKg = maximCarryWeight;
            VIN = vin;

            return this;
        }

        public VehicleStatus UpdateStatus(VehicleStatus status)
        {
            Status = status;
            return Status;
        }

    }
}
