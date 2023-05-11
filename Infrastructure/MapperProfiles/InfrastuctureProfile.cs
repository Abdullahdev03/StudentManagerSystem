using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.MapperProfiles;

public class InfrastuctureProfile : Profile
{
    public InfrastuctureProfile()
    {
        CreateMap<IdentityUser, StudentDto>();
        CreateMap<IdentityRole, RoleDto>();
        CreateMap<Student ,StudentDto>().ReverseMap();
        CreateMap<Group, StudentDto>().ReverseMap();
        CreateMap<Group, GroupDto>().ReverseMap();
        
    }
}