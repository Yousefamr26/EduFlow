using AutoMapper;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Features.Dashboard.Queries.Teacher;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<ApplicationUser, StudentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) 
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}