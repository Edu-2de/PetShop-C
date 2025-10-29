using AutoMapper;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tutor mappings
            CreateMap<Tutor, TutorDto>();
            CreateMap<CreateTutorDto, Tutor>()
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateTutorDto, Tutor>()
                .ForMember(dest => dest.TutorId, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore());

            // Aqui você pode adicionar mappings para outras entidades depois
        }
    }
}