using AutoMapper;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Query;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));
    }
}