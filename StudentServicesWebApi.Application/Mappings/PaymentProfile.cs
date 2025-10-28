using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Application.Mappings;
public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
            .ForMember(dest => dest.ProcessedByAdmin, opt => opt.MapFrom(src => src.ProcessedByAdmin))
            .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount))
            .ForMember(dest => dest.AmountWasAdjusted, opt => opt.MapFrom(src => src.AmountWasAdjusted));
        CreateMap<Payment, PaymentSummaryDto>()
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
            .ForMember(dest => dest.SenderUsername, opt => opt.MapFrom(src => src.Sender.Username))
            .ForMember(dest => dest.ProcessedByAdminName, opt => opt.MapFrom(src => src.ProcessedByAdmin != null ? $"{src.ProcessedByAdmin.FirstName} {src.ProcessedByAdmin.LastName}" : "Unassigned"))
            .ForMember(dest => dest.FinalAmount, opt => opt.MapFrom(src => src.FinalAmount))
            .ForMember(dest => dest.AmountWasAdjusted, opt => opt.MapFrom(src => src.AmountWasAdjusted));
        CreateMap<CreatePaymentDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SenderId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Photo, opt => opt.Ignore()) 
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => Domain.Enums.PaymentStatus.Waiting))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ProcessedByAdminId, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedByAdmin, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAmount, opt => opt.Ignore())
            .ForMember(dest => dest.RejectReason, opt => opt.Ignore())
            .ForMember(dest => dest.AdminNotes, opt => opt.Ignore())
            .ForMember(dest => dest.Sender, opt => opt.Ignore());
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.TelegramId, opt => opt.MapFrom(src => src.TelegramId))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
            .ForMember(dest => dest.ReferralId, opt => opt.MapFrom(src => src.ReferralId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
