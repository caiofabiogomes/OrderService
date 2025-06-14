using AutoMapper;
using OrderService.Application.ViewModels;
using OrderService.Contracts.Events;

namespace OrderService.Application.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderService.Domain.Entities.Order, CreateOrderEvent>()
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => src.Mode))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems.Select(i => new OrderItemEvent
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity.Value,
                    Description = i.Description.Description,
                    Title = i.Description.Title
                })));

            CreateMap<OrderService.Domain.Entities.Order, OrderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => src.Mode))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus.Status))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity.Value,
                    Description = i.Description.Description,
                    Title = i.Description.Title,
                    Price = i.Price.Amount
                })));
        }
    }
}
