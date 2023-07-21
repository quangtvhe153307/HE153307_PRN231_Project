using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void SaveOrder(Order order) => OrderDAO.SaveOrder(order);
        public void UpdateOrder(Order order) => OrderDAO.UpdateOrder(order);
        public List<Order> GetOrders() => OrderDAO.GetOrders();
        public Order GetOrderById(string id) => OrderDAO.FindOrderById(id);

        public void DeleteOrder(Order order) => OrderDAO.DeleteOrder(order);
    }
}
