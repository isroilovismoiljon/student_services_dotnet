using AutoMapper;
using StudentServicesWebApi.Application.DTOs.Auth;
using StudentServicesWebApi.Application.DTOs.User;
using StudentServicesWebApi.Domain.Models;
namespace StudentServicesWebApi.Application.Mappings;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>();
        CreateMap<UserResponseDto, User>();
        CreateMap<User, RegisterResponseDto>();
    }
}
