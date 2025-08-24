namespace AlphaProject.API.Mappings
{
    using AlphaProject.API.Models;
    using AlphaProject.Shared.Dtos;
    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            //.ReverseMap();

            CreateMap<OrderItem, OrderItemDto>();//.ReverseMap();
            CreateMap<Client, ClientDto>();//.ReverseMap();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            //.ReverseMap();

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.OrderItemId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
