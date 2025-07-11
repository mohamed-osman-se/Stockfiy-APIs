using Api.DTOs;
using Api.Models;
using Api.Modles;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Stock, StockDTO>()
             .ForMember(dest => dest.comments, opt => opt.MapFrom(src => src.comments));
        CreateMap<Stock, CreatStockDTO>().ReverseMap();
        CreateMap<Stock, UpdateStockDTO>().ReverseMap();
        CreateMap<Comment, CommentDTO>()
    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.appUser!.UserName))
    .ReverseMap();


        CreateMap<Comment, CreatCommentDTO>().ReverseMap();
        CreateMap<Comment, UPdateCommentDTO>().ReverseMap();
        CreateMap<AppUser, RegisterDTO>().ReverseMap();
        CreateMap<AppUser, AuthResponse>().ReverseMap();
        CreateMap<AppUser, LoginDTO>().ReverseMap();
    }
}
