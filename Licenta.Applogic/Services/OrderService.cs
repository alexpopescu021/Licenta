using System;
using System.Collections.Generic;
using Licenta.DataAccess.Abstractions;
using Licenta.Model;

namespace Licenta.AppLogic.Services
{
    public class OrderService
    {
        private readonly IPersistenceContext PersistenceContext;
        private readonly IOrderRepository OrderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IRouteRepository routeRepository;
        private readonly IRecipientRepository recipientRepository;

        public OrderService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
            OrderRepository = persistenceContext.OrderRepository;
            customerRepository = persistenceContext.CustomerRepository;
            routeRepository = persistenceContext.RouteRepository;
            recipientRepository = persistenceContext.RecipientRepository;
        }

        public void ChangeOrderStatus(Guid orderId, OrderStatus status)
        {

            var order = OrderRepository.GetById(orderId);
            order.SetStatus(status);
            if (status == OrderStatus.PickedUp)
            {
                order.SetPickUpTime();
            }
            if (status == OrderStatus.Delivered)
            {
                order.SetDeliveryTime();
            }
            OrderRepository.Update(order);
            PersistenceContext.SaveChanges();
        }

        public Order CreateOrder(Recipient recipient, Customer sender,
            LocationAddress deliveryAddress, LocationAddress pickUpAddress, decimal price, string awb)
        {
            var order = Order.Create(recipient, sender, pickUpAddress, deliveryAddress, price, awb);
            OrderRepository.Add(order);
            PersistenceContext.SaveChanges();
            return order;
        }

        public IEnumerable<Order> GetOrdersForCurrentCustomer(string senderId)
        {
            Guid.TryParse(senderId, out var customerId);
            return OrderRepository.GetOrdersForCurrentCustomer(customerId);
        }

        public IEnumerable<Order> GetUnfinishedOrders()
        {
            return OrderRepository.GetUnfinishedOrders();
        }

        public Recipient CreateNewRecipient(string name, string phoneNo, string email)
        {
            var recipient = Recipient.Create(name, phoneNo, email);
            recipient = recipientRepository.Add(recipient);
            PersistenceContext.SaveChanges();
            return recipient;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return OrderRepository.GetAll();
        }


        //when a driver starts up a route,all the order with status PickedUp should be changed into Delivering
        public void StartRoute(ICollection<RouteEntry> routeEntries)
        {
            foreach (var routeEntry in routeEntries)
            {
                if (routeEntry.Order.Status == OrderStatus.PickedUp)
                {
                    routeEntry.Order.SetStatus(OrderStatus.Delivering);
                }
            }
        }

        public Order CreateOrder(Recipient recipient, Customer sender, string pickupId, string deliveryId, decimal price, string awb)
        {
            Guid.TryParse(pickupId, out var pickupGuid);
            var pickupLocation = customerRepository.GetLocationAddress(pickupGuid);

            Guid.TryParse(deliveryId, out var deliveryGuid);
            var deliveryLocation = customerRepository.GetLocationAddress(deliveryGuid);

            var order = Order.Create(recipient, sender, pickupLocation, deliveryLocation, price, awb);

            OrderRepository.Add(order);
            PersistenceContext.SaveChanges();
            return order;
        }

        public Order GetById(string id)
        {
            Guid.TryParse(id, out var guid);
            return OrderRepository.GetById(guid);
        }


        public bool Remove(string id)
        {
            var orderToRemove = GetById(id);
            var routes = routeRepository.GetAll();
            foreach (var route in routes)
            {
                foreach (var entry in route.RouteEntries)
                {
                    if (entry.Order.Id == orderToRemove.Id)
                    {
                        routeRepository.Remove(entry, route.Id);
                        break;
                    }
                }
            }
            return OrderRepository.RemoveOrder(orderToRemove.Id);
        }

        public Recipient GetRecipient(string id)
        {
            var order = GetById(id);
            return order.Recipient;
        }

        public Order Update(string id, string pickupId, string deliveryId, decimal price)
        {
            Guid.TryParse(pickupId, out var pickupGuid);
            var pickupLocation = customerRepository.GetLocationAddress(pickupGuid);

            Guid.TryParse(deliveryId, out var deliveryGuid);
            var deliveryLocation = customerRepository.GetLocationAddress(deliveryGuid);

            var order = GetById(id);
            order.Update(pickupLocation, deliveryLocation, price);
            PersistenceContext.SaveChanges();
            return order;
        }

        public Order GetByAwb(string awb)
        {
           return OrderRepository.GetByAwb(awb);
        }
    }
}
