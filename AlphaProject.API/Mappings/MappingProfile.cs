namespace AlphaProject.API.Mappings
{
    using AlphaProject.API.Models;
    using AlphaProject.Shared.Dtos;
    using AutoMapper;
    

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            /*CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();*/
        }
    }

}
