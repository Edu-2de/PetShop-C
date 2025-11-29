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

            // Animal mappings
            CreateMap<Animal, AnimalDto>()
                .ForMember(dest => dest.TutorNome, opt => opt.MapFrom(src => src.Tutor != null ? src.Tutor.Nome : null));
            CreateMap<CreateAnimalDto, Animal>();
            CreateMap<UpdateAnimalDto, Animal>();

            // Produto Mappings
            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor != null ? src.Fornecedor.Nome : null));
            CreateMap<CreateProdutoDto, Produto>();
            CreateMap<UpdateProdutoDto, Produto>();

            // ImagemProduto Mappings
            CreateMap<ProdutoImagem, ProdutoImagemDto>();

            // Servico Mappings
            CreateMap<Servico, ServicoDto>();
            CreateMap<CreateServicoDto, Servico>();
            CreateMap<UpdateServicoDto, Servico>()
                .ForMember(dest => dest.ServicoId, opt => opt.Ignore());

            // Agendamento mappings
            CreateMap<Agendamento, AgendamentoDto>()
                .ForMember(dest => dest.AnimalNome, opt => opt.MapFrom(src => src.Animal != null ? src.Animal.Nome : null))
                .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null))
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario != null ? src.Funcionario.Nome : null));
            CreateMap<CreateAgendamentoDto, Agendamento>();
            CreateMap<UpdateAgendamentoDto, Agendamento>()
                .ForMember(dest => dest.AgendamentoId, opt => opt.Ignore());

            // Fornecedor mappings
            CreateMap<Fornecedor, FornecedorDto>();
            CreateMap<CreateFornecedorDto, Fornecedor>();
            CreateMap<UpdateFornecedorDto, Fornecedor>()
                .ForMember(dest => dest.FornecedorId, opt => opt.Ignore());
        }
    }
}