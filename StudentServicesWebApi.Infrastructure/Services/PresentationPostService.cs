using StudentServicesWebApi.Application.DTOs.PresentationPost;
using StudentServicesWebApi.Application.Interfaces;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Infrastructure.Services;
public class PresentationPostService : IPresentationPostService
{
    private readonly IPresentationPostRepository _postRepository;
    public PresentationPostService(IPresentationPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<List<PresentationPostDto>> GetPostsByPresentationPageIdAsync(int presentationPageId, CancellationToken ct = default)
    {
        var posts = await _postRepository.GetByPresentationPageIdAsync(presentationPageId, ct);
        return posts.Select(p => new PresentationPostDto
        {
            Id = p.Id,
            PresentationPageId = p.PresentationPageId,
            TitleId = p.TitleId,
            TextId = p.TextId,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }
    public async Task<PresentationPostDto?> GetPostByIdAsync(int id, CancellationToken ct = default)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);
        if (post == null) return null;
        return new PresentationPostDto
        {
            Id = post.Id,
            PresentationPageId = post.PresentationPageId,
            TitleId = post.TitleId,
            TextId = post.TextId,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }
    public async Task<PresentationPostDto> CreatePostAsync(CreatePresentationPostDto createDto, CancellationToken ct = default)
    {
        var post = new PresentationPost
        {
            PresentationPageId = createDto.PresentationPageId,
            TitleId = createDto.TitleId,
            TextId = createDto.TextId
        };
        var createdPost = await _postRepository.AddAsync(post, ct);
        return new PresentationPostDto
        {
            Id = createdPost.Id,
            PresentationPageId = createdPost.PresentationPageId,
            TitleId = createdPost.TitleId,
            TextId = createdPost.TextId,
            CreatedAt = createdPost.CreatedAt,
            UpdatedAt = createdPost.UpdatedAt
        };
    }
    public async Task<PresentationPostDto?> UpdatePostAsync(int id, UpdatePresentationPostDto updateDto, CancellationToken ct = default)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);
        if (post == null) return null;
        if (updateDto.PresentationPageId.HasValue)
            post.PresentationPageId = updateDto.PresentationPageId.Value;
        if (updateDto.TitleId.HasValue)
            post.TitleId = updateDto.TitleId.Value;
        if (updateDto.TextId.HasValue)
            post.TextId = updateDto.TextId.Value;
        post.UpdatedAt = DateTime.UtcNow;
        var updatedPost = await _postRepository.UpdateAsync(post, ct);
        return new PresentationPostDto
        {
            Id = updatedPost.Id,
            PresentationPageId = updatedPost.PresentationPageId,
            TitleId = updatedPost.TitleId,
            TextId = updatedPost.TextId,
            CreatedAt = updatedPost.CreatedAt,
            UpdatedAt = updatedPost.UpdatedAt
        };
    }
    public async Task<bool> DeletePostAsync(int id, CancellationToken ct = default)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);
        if (post == null) return false;
        await _postRepository.DeleteAsync(post, ct);
        return true;
    }
    public async Task<bool> PostExistsAsync(int id, CancellationToken ct = default)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);
        return post != null;
    }
}
