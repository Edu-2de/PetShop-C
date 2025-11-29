using AutoMapper;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Mapeamentos de Funcionario (ESSENCIAL PARA O LOGIN) ---
            CreateMap<Funcionario, FuncionarioDto>().ReverseMap();

            // --- Mapeamentos de Venda ---
            CreateMap<Venda, VendaDto>();
            CreateMap<ItemVenda, ItemVendaDto>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : null))
                .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null));

            CreateMap<CreateVendaDto, Venda>()
                .ForMember(dest => dest.DataVenda, opt => opt.Ignore())
                .ForMember(dest => dest.ValorTotal, opt => opt.Ignore());

            CreateMap<CreateItemVendaDto, ItemVenda>();

            // --- Mapeamentos de Agendamento ---
            CreateMap<Agendamento, AgendamentoDto>()
                .ForMember(dest => dest.AnimalNome, opt => opt.MapFrom(src => src.Animal != null ? src.Animal.Nome : null))
                .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null))
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario != null ? src.Funcionario.Nome : null));

            CreateMap<CreateAgendamentoDto, Agendamento>();
            CreateMap<UpdateAgendamentoDto, Agendamento>();

            // --- Outros Mapeamentos ---
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<CreateProdutoDto, Produto>();

            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<CreateFornecedorDto, Fornecedor>();
            CreateMap<UpdateFornecedorDto, Fornecedor>();

            CreateMap<Tutor, TutorDto>().ReverseMap();
            CreateMap<CreateTutorDto, Tutor>();

            CreateMap<Animal, AnimalDto>().ReverseMap();

            CreateMap<Servico, ServicoDto>().ReverseMap();
            CreateMap<CreateServicoDto, Servico>();
        }
    }
}