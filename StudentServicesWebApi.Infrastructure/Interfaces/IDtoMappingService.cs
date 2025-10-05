using StudentServicesWebApi.Application.DTOs.Payment;
using StudentServicesWebApi.Application.DTOs.PhotoSlide;
using StudentServicesWebApi.Application.DTOs.TextSlide;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure.Interfaces;

/// <summary>
/// Service for mapping domain models to DTOs with proper URL transformation for image fields
/// </summary>
public interface IDtoMappingService
{
    /// <summary>
    /// Maps User entity to UserResponseDto with image URL transformation
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>UserResponseDto with full image URLs</returns>
    UserResponseDto MapToUserResponseDto(User user);
    
    /// <summary>
    /// Maps Payment entity to PaymentDto with image URL transformation
    /// </summary>
    /// <param name="payment">Payment entity</param>
    /// <returns>PaymentDto with full image URLs</returns>
    PaymentDto MapToPaymentDto(Payment payment);
    
    /// <summary>
    /// Maps Payment entity to PaymentSummaryDto
    /// </summary>
    /// <param name="payment">Payment entity</param>
    /// <returns>PaymentSummaryDto</returns>
    PaymentSummaryDto MapToPaymentSummaryDto(Payment payment);
    
    /// <summary>
    /// Maps TextSlide entity to TextSlideDto
    /// </summary>
    /// <param name="textSlide">TextSlide entity</param>
    /// <returns>TextSlideDto</returns>
    TextSlideDto MapToTextSlideDto(TextSlide textSlide);
    
    /// <summary>
    /// Maps TextSlide entity to TextSlideSummaryDto
    /// </summary>
    /// <param name="textSlide">TextSlide entity</param>
    /// <returns>TextSlideSummaryDto</returns>
    TextSlideSummaryDto MapToTextSlideSummaryDto(TextSlide textSlide);
    
    /// <summary>
    /// Maps PhotoSlide entity to PhotoSlideDto with image URL transformation
    /// </summary>
    /// <param name="photoSlide">PhotoSlide entity</param>
    /// <returns>PhotoSlideDto</returns>
    PhotoSlideDto MapToPhotoSlideDto(PhotoSlide photoSlide);
    
    /// <summary>
    /// Maps PhotoSlide entity to PhotoSlideSummaryDto with image URL transformation
    /// </summary>
    /// <param name="photoSlide">PhotoSlide entity</param>
    /// <returns>PhotoSlideSummaryDto</returns>
    PhotoSlideSummaryDto MapToPhotoSlideSummaryDto(PhotoSlide photoSlide);
}
