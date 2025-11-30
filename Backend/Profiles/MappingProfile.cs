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

            CreateMap<CreateFuncionarioDto, Funcionario>();

            // CORREÇÃO: Adicionado UpdateFuncionarioDto
            // Ignoramos o Email aqui para tratar manualmente no Controller, pois envolve outra tabela (Usuario)
            CreateMap<UpdateFuncionarioDto, Funcionario>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

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

            // CORREÇÃO: Adicionado UpdateProdutoDto
            CreateMap<UpdateProdutoDto, Produto>();

            CreateMap<ProdutoImagem, ProdutoImagemDto>().ReverseMap();

            // --- Outros ---
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<CreateFornecedorDto, Fornecedor>();
            CreateMap<UpdateFornecedorDto, Fornecedor>();

            CreateMap<Tutor, TutorDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ReverseMap();

            CreateMap<CreateTutorDto, Tutor>();

            // CORREÇÃO: Adicionado UpdateTutorDto
            CreateMap<UpdateTutorDto, Tutor>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            CreateMap<Animal, AnimalDto>().ReverseMap();
            CreateMap<CreateAnimalDto, Animal>();
            CreateMap<UpdateAnimalDto, Animal>();

            CreateMap<Servico, ServicoDto>().ReverseMap();
            CreateMap<CreateServicoDto, Servico>();

            // CORREÇÃO: Adicionado UpdateServicoDto (Correção do Erro 500 ao editar serviço)
            CreateMap<UpdateServicoDto, Servico>();
        }
    }
}