namespace StudentServicesWebApi.Domain.Interfaces;

public interface IUrlService
{
    string? GetPaymentImageUrl(string? relativePath);
    string? GetPresentationPhotoUrl(string? relativePath);
    string GetBaseUrl();
    string? GetPaymentFileUrl(string? relativePath);
    string? GetPresentationFileUrl(string? relativePath);
}