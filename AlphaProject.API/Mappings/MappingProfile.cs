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
           
            CreateMap<ProductDto, Product>()
               .ForMember(dest => dest.OrderItems, opt => opt.Ignore()); // gestisci la logica se vuoi mappare anche gli OrderItems
                       
            CreateMap<Client, ClientDto>();//.ReverseMap();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            //.ReverseMap();

            // Mapping tra OrderDto e Order. gli orderitems vengono mappati dal DTO al modello OrderItem
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // Mapping tra OrderItem e OrderItemDto
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.OrderItemId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
