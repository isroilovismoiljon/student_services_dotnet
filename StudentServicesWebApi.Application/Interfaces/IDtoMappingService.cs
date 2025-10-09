using StudentServicesWebApi.Application.DTOs.Design;
using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

public interface IDtoMappingService
{
    UserResponseDto MapToUserResponseDto(User user);
    PaymentDto MapToPaymentDto(Payment payment);
    PaymentSummaryDto MapToPaymentSummaryDto(Payment payment);
    TextSlideDto MapToTextSlideDto(TextSlide textSlide);
    TextSlideSummaryDto MapToTextSlideSummaryDto(TextSlide textSlide);
    PhotoSlideDto MapToPhotoSlideDto(PhotoSlide photoSlide);
    PhotoSlideSummaryDto MapToPhotoSlideSummaryDto(PhotoSlide photoSlide);
    DesignDto MapToDesignDto(Design design);
    DesignSummaryDto MapToDesignSummaryDto(Design design);
}
