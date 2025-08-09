using AutoMapper;
using TicketTracker.DTOs;
using TicketTracker.Models;

namespace TicketTracker.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();

        CreateMap<Ticket, TicketResponseDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo));

        CreateMap<TicketRequestDto, Ticket>();

        CreateMap<Comment, CommentResponseDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
        
        CreateMap<CommentRequestDto, Comment>();

        // CreateMap<AttachmentRequestDto, Attachment>();
        CreateMap<Attachment, AttachmentResponseDto>();
    }
}