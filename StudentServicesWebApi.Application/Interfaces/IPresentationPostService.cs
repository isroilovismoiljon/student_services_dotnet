using StudentServicesWebApi.Application.DTOs.PresentationPost;
namespace StudentServicesWebApi.Application.Interfaces;
public interface IPresentationPostService
{
    Task<List<PresentationPostDto>> GetPostsByPresentationPageIdAsync(int presentationPageId, CancellationToken ct = default);
    Task<PresentationPostDto?> GetPostByIdAsync(int id, CancellationToken ct = default);
    Task<PresentationPostDto> CreatePostAsync(CreatePresentationPostDto createDto, CancellationToken ct = default);
    Task<PresentationPostDto?> UpdatePostAsync(int id, UpdatePresentationPostDto updateDto, CancellationToken ct = default);
    Task<bool> DeletePostAsync(int id, CancellationToken ct = default);
    Task<bool> PostExistsAsync(int id, CancellationToken ct = default);
}
