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
        public string Vin { get; protected set; }
        public VehicleStatus Status { get; protected set; }

        public static Vehicle Create(string name, string type, string registrationNumber, int maximCarryWeight, string vin)
        {
            if (maximCarryWeight < 0)
            {
                throw new Exception("Maxim carry weight can not be less than 0");
            }

            return new Vehicle
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                RegistrationNumber = registrationNumber,
                MaximCarryWeightKg = maximCarryWeight,
                Vin = vin,
                Status = VehicleStatus.Free
            };
        }

        public Vehicle Update(string name, string type, string registrationNumber, int maximCarryWeight, string vin)
        {
            Name = name;
            Type = type;
            RegistrationNumber = registrationNumber;
            MaximCarryWeightKg = maximCarryWeight;
            Vin = vin;

            return this;
        }

        public VehicleStatus UpdateStatus(VehicleStatus status)
        {
            Status = status;
            return Status;
        }

    }
}
