using AutoMapper;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CORRIGIDO: Mapeamentos de Usuario para UserInfo - GARANTIR TUTORD CORRETO
            CreateMap<Usuario, UserInfo>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome)) // Nome vem sempre do Usuario
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => 
                    src.Funcionario != null ? src.Funcionario.Cargo : src.TipoUsuario))
                .ForMember(dest => dest.TutorId, opt => opt.MapFrom(src => 
                    src.Tutor != null ? (int?)src.Tutor.TutorId : null))
                .ForMember(dest => dest.FuncionarioId, opt => opt.MapFrom(src => 
                    src.Funcionario != null ? (int?)src.Funcionario.FuncionarioId : null));

            // CORRIGIDO: Mapeamentos de Tutor - USUARIO É A RAIZ
            CreateMap<Tutor, TutorDto>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : src.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Email : null))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataCadastro));

            CreateMap<TutorDto, Tutor>();
            CreateMap<CreateTutorDto, Tutor>();
            CreateMap<UpdateTutorDto, Tutor>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore()); // Usuario será atualizado separadamente

            // Mapeamentos de Funcionario
            CreateMap<Funcionario, FuncionarioDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Email : null));
            CreateMap<CreateFuncionarioDto, Funcionario>();
            CreateMap<UpdateFuncionarioDto, Funcionario>();
            
            // NOVO: Mapeamento simplificado para dropdown
            CreateMap<Funcionario, FuncionarioSimplificadoDto>();

            // Mapeamentos de Animal
            CreateMap<Animal, AnimalDto>()
                .ForMember(dest => dest.TutorNome, opt => opt.MapFrom(src => src.Tutor != null ? src.Tutor.Nome : null));
            CreateMap<CreateAnimalDto, Animal>();
            CreateMap<UpdateAnimalDto, Animal>();

            // Mapeamentos de Servico - ATUALIZADO PARA CARGOS
            CreateMap<Servico, ServicoDto>()
                .ForMember(dest => dest.CargosResponsaveis, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.CargosResponsaveis) 
                        ? src.CargosResponsaveis.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.CargosResponsaveisTexto, opt => opt.MapFrom(src => src.CargosResponsaveis))
                .ForMember(dest => dest.FuncionariosAptos, opt => opt.MapFrom(src => 
                    src.ServicoFuncionarios.Select(sf => sf.Funcionario)))
                .ForMember(dest => dest.FuncionarioResponsavelNome, opt => opt.MapFrom(src => 
                    src.FuncionarioResponsavel != null ? src.FuncionarioResponsavel.Nome : null));
            
            CreateMap<CreateServicoDto, Servico>()
                .ForMember(dest => dest.CargosResponsaveis, opt => opt.MapFrom(src =>
                    src.CargosResponsaveis != null && src.CargosResponsaveis.Any()
                        ? string.Join(",", src.CargosResponsaveis)
                        : null))
                .ForMember(dest => dest.ServicoFuncionarios, opt => opt.Ignore());
            
            CreateMap<UpdateServicoDto, Servico>()
                .ForMember(dest => dest.CargosResponsaveis, opt => opt.MapFrom(src =>
                    src.CargosResponsaveis != null && src.CargosResponsaveis.Any()
                        ? string.Join(",", src.CargosResponsaveis)
                        : null))
                .ForMember(dest => dest.ServicoFuncionarios, opt => opt.Ignore());

            // Mapeamentos de Agendamento
            CreateMap<Agendamento, AgendamentoDto>()
                .ForMember(dest => dest.AnimalNome, opt => opt.MapFrom(src => src.Animal != null ? src.Animal.Nome : null))
                .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null))
                .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario != null ? src.Funcionario.Nome : null));
            
            CreateMap<CreateAgendamentoDto, Agendamento>();
            CreateMap<UpdateAgendamentoDto, Agendamento>();

            // Mapeamentos de Produto
            CreateMap<Produto, ProdutoDto>();
            CreateMap<CreateProdutoDto, Produto>();
            CreateMap<UpdateProdutoDto, Produto>();

            // Mapeamentos de Categoria
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<CreateCategoriaDto, Categoria>();

            // Mapeamentos de Fornecedor
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<CreateFornecedorDto, Fornecedor>();
            CreateMap<UpdateFornecedorDto, Fornecedor>();

            // Mapeamentos de Venda
            CreateMap<Venda, VendaDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId)); // ? Mapeia o UsuarioId
            
            CreateMap<CreateVendaDto, Venda>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId)); // ? Mapeia o UsuarioId

            CreateMap<ItemVenda, ItemVendaDto>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : null))
                .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null));
            
            CreateMap<CreateItemVendaDto, ItemVenda>();

            // Mapeamentos de ProdutoImagem
            CreateMap<ProdutoImagem, ProdutoImagemDto>().ReverseMap();
        }
    }
}