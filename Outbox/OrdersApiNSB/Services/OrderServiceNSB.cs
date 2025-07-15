using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using OrdersApiNSB.Data;


namespace OrdersApiNSB.Services
{
    public class OrderServiceNSB : IOrderServiceNSB
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageSession _messageSession;
        private readonly IMapper _mapper;

        public OrderServiceNSB(
            IOrderRepository orderRepository,
            IMessageSession messageSession,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _messageSession = messageSession;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderAsync(orderId);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _orderRepository.GetOrderAsync(id);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            return await _orderRepository.AddOrderAsync(order);
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _orderRepository.OrderExistsAsync(id);
        }
        public async Task AcceptOrder(OrderModel model)
        {
            var domainObject = _mapper.Map<Order>(model);
            var savedOrder = await AddOrderAsync(domainObject);

            // Publish OrderReceived event using NServiceBus
            await _messageSession.Publish(new OrderReceived
            {
                CreatedAt = savedOrder.OrderDate,
                OrderId = savedOrder.OrderId
            });

            // Publish OrderCreated event using NServiceBus
            await _messageSession.Publish(new OrderCreated
            {
                CreatedAt = savedOrder.OrderDate,
                OrderId = savedOrder.OrderId,
                TotalAmount = domainObject.OrderItems.Sum(x => x.Price * x.Quantity)
            });

            try
            {
                await _orderRepository.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // Consider adding appropriate exception handling here
            }
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _orderRepository.SaveChangesAsync();
        }

    }
}
