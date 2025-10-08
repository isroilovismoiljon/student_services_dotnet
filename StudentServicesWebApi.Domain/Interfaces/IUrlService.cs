namespace StudentServicesWebApi.Domain.Interfaces;

public interface IUrlService
{
    string? GetImageUrl(string? relativePath);
    string GetBaseUrl();
    string? GetFileUrl(string? relativePath);
}