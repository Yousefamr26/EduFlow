using AutoMapper;
using EduFlow.Domain.Entities;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<Material, MaterialDto>()
            .ForMember(dest => dest.filePath, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.videoUrl, opt => opt.MapFrom(src => src.VideoUrl));
    }
}