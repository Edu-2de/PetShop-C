using AutoMapper;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<CreateCategoriaDto, Categoria>();

            // --- Mapeamentos de Funcionario ---
            CreateMap<Funcionario, FuncionarioDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ReverseMap();

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

            // --- Produtos ---
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<CreateProdutoDto, Produto>();

            // ====================================================================
            // ADICIONE ESTA LINHA PARA CORRIGIR O ERRO DE UPLOAD
            // ====================================================================
            CreateMap<ProdutoImagem, ProdutoImagemDto>().ReverseMap();

            // --- Outros ---
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<CreateFornecedorDto, Fornecedor>();
            CreateMap<UpdateFornecedorDto, Fornecedor>();

            CreateMap<Tutor, TutorDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ReverseMap();

            CreateMap<Animal, AnimalDto>().ReverseMap();

            CreateMap<Servico, ServicoDto>().ReverseMap();
            CreateMap<CreateServicoDto, Servico>();
        }
    }
}