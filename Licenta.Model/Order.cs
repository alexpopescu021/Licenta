﻿using System;

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
        public string Awb { get; private set; }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        public static Order Create(Recipient recipient, Customer sender, LocationAddress pickup,
            LocationAddress delivery, decimal price, string awb)

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
                Awb = awb
            };
            return order;
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