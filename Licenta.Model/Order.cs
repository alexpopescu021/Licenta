using System;

namespace Licenta.Model
{
    public enum OrderStatus { Created, Assigned, PickedUp, Delivering, Delivered, Canceled };

    public class Order : DataEntity
    {
        public virtual LocationAddress PickUpAddress { get; private set; }
        public virtual LocationAddress DeliveryAddress { get; private set; }
        public virtual Recipient Recipient { get; private set; }
        public virtual Customer Sender { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime PickUpTime { get; private set; }
        public DateTime DeliveryTime { get; private set; }
        public string AWB { get; private set; }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        public static Order Create(Recipient recipient, Customer sender, LocationAddress pickup,
            LocationAddress delivery, decimal price)

        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                DeliveryAddress = delivery,
                PickUpAddress = pickup,
                Price = price,
                Recipient = recipient,
                Sender = sender,
                Status = OrderStatus.Created,
                CreationTime = DateTime.UtcNow,
                AWB = CreateAWB(8)
            };
            return order;
        }

        static Random rd = new Random();
        internal static string CreateAWB(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public Order Update(LocationAddress pickup, LocationAddress delivery, decimal price)
        {
            PickUpAddress = pickup;
            DeliveryAddress = delivery;
            Price = price;

            return this;
        }

        public void SetPickUpTime()
        {
            PickUpTime = DateTime.UtcNow;
        }

        public void SetDeliveryTime()
        {
            DeliveryTime = DateTime.UtcNow;
        }
    }
}