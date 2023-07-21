using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order);
        Order GetOrderById(string id);
        void DeleteOrder(Order order);
        void UpdateOrder(Order order);
        List<Order> GetOrders();
    }
}
