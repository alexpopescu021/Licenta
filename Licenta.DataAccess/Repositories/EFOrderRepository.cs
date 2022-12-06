using Licenta.DataAccess.Abstractions;
using Licenta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Licenta.DataAccess.Repositories
{
    public class EfOrderRepository : EfBaseRepository<Order>, IOrderRepository
    {
        public EfOrderRepository(ApplicationDbContext context) : base(context)
        {

        }


        public new Order GetById(Guid orderId)
        {
            return DbContext.Orders
                .Include(o => o.PickUpAddress)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.Recipient)
                .Include(o => o.Recipient.ContactDetails)
                .Include(o => o.Sender)
                .ThenInclude(o => o.LocationAddresses)
                .FirstOrDefault(o => o.Id == orderId);
        }


        public new IEnumerable<Order> GetAll()
        {
            return DbContext.Orders
                        .Include(o => o.PickUpAddress)
                        .Include(o => o.DeliveryAddress)
                        .Include(o => o.Sender)
                        .Include(o => o.Sender.ContactDetails)
                        .Include(o => o.Recipient)
                        .Include(o => o.Recipient.ContactDetails)
                        .OrderByDescending(o => o.CreationTime)
                        .AsEnumerable();
        }

        public IEnumerable<Order> GetOrdersForCurrentCustomer(Guid senderId)
        {
            return DbContext.Orders
                        .Where(s => s.Sender.Id == senderId)
                        .Include(o => o.PickUpAddress)
                        .Include(o => o.DeliveryAddress)
                        .Include(o => o.Sender)
                        .Include(o => o.Sender.ContactDetails)
                        .Include(o => o.Recipient)
                        .Include(o => o.Recipient.ContactDetails)
                        .OrderByDescending(o => o.CreationTime)
                        .AsEnumerable();
        }
        public IEnumerable<Order> GetUnfinishedOrders()
        {
            return DbContext.Orders
                        .Where(o => o.Status != OrderStatus.Delivered)
                        .Include(o => o.PickUpAddress)
                        .Include(o => o.DeliveryAddress)
                        .Include(o => o.Sender)
                        .Include(o => o.Sender.ContactDetails)
                        .Include(o => o.Recipient)
                        .Include(o => o.Recipient.ContactDetails)
                        .OrderByDescending(o => o.CreationTime)
                        .AsEnumerable();
        }
        public bool RemoveOrdersFromCustomer(Guid customerId)
        {
            var orders = DbContext.Orders
                .Where(o => o.Sender.Id == customerId)
                .AsEnumerable();

            if (!orders.Any())
            {
                return false;
            }

            foreach (var order in orders)
            {
                RemoveOrder(order.Id);
            }

            DbContext.SaveChanges();
            return true;
        }

        public bool RemoveOrder(Guid orderId)
        {
            var orderToRemove = GetById(orderId);

            if (orderToRemove != null)
            {
                DbContext.Remove(orderToRemove.Recipient.ContactDetails);
                DbContext.Remove(orderToRemove.Recipient);
                DbContext.Remove(orderToRemove);
                DbContext.SaveChanges();

                return true;
            }
            return false;
        }

        public Order GetByAwb(string awb)
        {
            return DbContext.Orders.FirstOrDefault(o => o.Awb == awb);
        }
    }
}
