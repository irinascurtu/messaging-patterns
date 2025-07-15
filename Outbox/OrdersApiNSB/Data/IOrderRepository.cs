using Orders.Domain.Entities;

namespace OrdersApiNSB.Data
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderAsync(int id);
        Task<Order> GetOrderAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<bool> OrderExistsAsync(int id);
        Task<int> SaveChangesAsync();
        Task<Order> UpdateOrderAsync(Order order);
    }
}