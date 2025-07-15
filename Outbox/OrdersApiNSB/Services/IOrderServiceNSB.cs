using Contracts.Models;
using Orders.Domain.Entities;

namespace OrdersApiNSB.Services
{
    public interface IOrderServiceNSB
    {
        Task<Order> AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> OrderExistsAsync(int id);
        Task AcceptOrder(OrderModel model);
        Task<Order> GetOrderAsync(Guid orderId);
    }
}