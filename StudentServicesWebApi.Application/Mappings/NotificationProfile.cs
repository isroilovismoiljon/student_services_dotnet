using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Notification;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Application.Mappings;
public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationResponseDto>();
        CreateMap<CreateNotificationRequestDto, Notification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.NotificationStatus.Unread))
            .ForMember(dest => dest.ReadAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        CreateMap<UpdateNotificationRequestDto, Notification>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
