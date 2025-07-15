using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrdersApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository,
          IPublishEndpoint publishEndpoint,
            IMapper mapper
            )
        {
            _orderRepository = orderRepository;
            this._publishEndpoint = publishEndpoint;
            this.mapper = mapper;
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

            var domainObject = mapper.Map<Order>(model);


            var savedOrder = await this.AddOrderAsync(domainObject);

            var orderReceived = _publishEndpoint.Publish(new OrderReceived()
            {
                CreatedAt = savedOrder.OrderDate,
                OrderId = savedOrder.OrderId
            });

            var notifyOrderCreated = _publishEndpoint.Publish(new OrderCreated()
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

            }

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _orderRepository.SaveChangesAsync();
        }

    }
}
