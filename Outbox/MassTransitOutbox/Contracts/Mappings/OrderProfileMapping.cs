using AutoMapper;
using Contracts.Models;
using Orders.Domain.Entities;


namespace OrdersApi.Infrastructure.Mappings
{
    public class OrderProfileMapping : Profile
    {
        public OrderProfileMapping()
        {
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<CustomerModel, Customer>();
            CreateMap<OrderModel, Order>().
                ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new Customer()
                {
                    Email = src.Email,
                    Phone = src.Phone,
                    Name = src.Name
                }))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            //Domain ->Models
            CreateMap<OrderItem, OrderItemModel>();
            CreateMap<Customer, CustomerModel>();

            CreateMap<Order, OrderModel>().
              ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer.Email)).
              ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Customer.Phone)).
              ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Customer.Name));
        }
    }
}
