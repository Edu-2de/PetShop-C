using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.Models;

namespace SIGA_PET.Controllers
{
    /// <summary>
    /// Controller para gerenciar dados de teste do sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        #region Limpeza e Reset

        /// <summary>
        /// Limpar completamente o banco de dados
        /// </summary>
        /// <remarks>
        /// ATENCAO: Esta operacao remove TODOS os dados do banco!
        /// 
        /// Ordem de exclusao (respeitando foreign keys):
        /// - ItensVenda
        /// - Vendas
        /// - Agendamentos
        /// - ServicoFuncionarios
        /// - RegistroProntuario
        /// - Servicos
        /// - Animais
        /// - ProdutoImagens
        /// - Produtos
        /// - Categorias
        /// - Fornecedores
        /// - Funcionarios
        /// - Tutores
        /// - Usuarios
        /// </remarks>
        [HttpDelete("limpar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LimparBanco()
        {
            try
            {
                // Desabilitar constraints temporariamente para facilitar deleção
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

                // Deletar dados na ordem correta
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [ItensVenda]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Vendas]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Agendamentos]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [ServicoFuncionarios]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [RegistroProntuario]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Servicos]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Animais]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [ProdutoImagens]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Produtos]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Categorias]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Fornecedores]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Funcionarios]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Tutores]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Usuarios]");

                // Reabilitar constraints
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

                // Resetar identity seeds
                var tabelas = new[] { "ItensVenda", "Vendas", "Agendamentos", "RegistroProntuario", 
                    "Servicos", "Animais", "ProdutoImagens", "Produtos", "Categorias", 
                    "Fornecedores", "Funcionarios", "Tutores", "Usuarios" };

                foreach (var tabela in tabelas)
                {
                    await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{tabela}', RESEED, 0)");
                }

                return Ok(new { 
                    success = true,
                    message = "Banco de dados limpo com sucesso",
                    tabelas_limpas = 14,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false,
                    message = "Erro ao limpar banco de dados", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        #endregion

        #region Criação Individual

        /// <summary>
        /// Criar usuarios do sistema (Passo 1 de 10)
        /// </summary>
        /// <remarks>
        /// Cria 5 usuarios:
        /// - 1 Admin (admin@sigapet.com) - senha: admin2024
        /// - 3 Funcionarios (vet, tosador, atendente) - senhas diferentes
        /// - 1 Cliente (cliente@example.com) - senha: cliente123
        /// 
        /// CADA USUÁRIO TEM SUA PRÓPRIA SENHA para facilitar testes
        /// </remarks>
        [HttpPost("usuarios")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CriarUsuarios()
        {
            try
            {
                if (await _context.Usuarios.AnyAsync())
                {
                    return BadRequest(new { message = "Usuarios ja existem. Execute DELETE /api/Seed/limpar primeiro" });
                }

                var usuarios = new List<Usuario>
                {
                    new Usuario { Nome = "Admin Sistema", Email = "admin@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin2024"), TipoUsuario = "Admin", Ativo = true },
                    new Usuario { Nome = "Dr. João Silva", Email = "vet@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("vet123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Maria Santos", Email = "tosador@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("tosador123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Pedro Oliveira", Email = "atendente@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("atendente123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Carlos Silva", Email = "cliente@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("cliente123"), TipoUsuario = "Tutor", Ativo = true }
                };

                _context.Usuarios.AddRange(usuarios);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    success = true,
                    message = "Usuarios criados com sucesso",
                    total = usuarios.Count,
                    senhas_diferentes = new {
                        admin = "admin2024",
                        vet = "vet123", 
                        tosador = "tosador123",
                        atendente = "atendente123",
                        cliente = "cliente123"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao criar usuarios", error = ex.Message });
            }
        }

        /// <summary>
        /// Criar funcionarios (Passo 2 de 10)
        /// </summary>
        [HttpPost("funcionarios")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarFuncionarios()
        {
            try
            {
                if (await _context.Funcionarios.AnyAsync())
                {
                    return BadRequest(new { message = "Funcionarios ja existem" });
                }

                var funcionarios = new List<Funcionario>
                {
                    new Funcionario { Nome = "Dr. João Silva", Cargo = "Veterinario", Telefone = "(11) 98765-4321", DataContratacao = DateTime.Now, Ativo = true, UsuarioId = 2 },
                    new Funcionario { Nome = "Maria Santos", Cargo = "Tosador", Telefone = "(11) 98765-4322", DataContratacao = DateTime.Now, Ativo = true, UsuarioId = 3 },
                    new Funcionario { Nome = "Pedro Oliveira", Cargo = "Atendente", Telefone = "(11) 98765-4323", DataContratacao = DateTime.Now, Ativo = true, UsuarioId = 4 }
                };

                _context.Funcionarios.AddRange(funcionarios);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Funcionarios criados", total = funcionarios.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar tutores (Passo 3 de 10)
        /// </summary>
        [HttpPost("tutores")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarTutores()
        {
            try
            {
                if (await _context.Tutores.AnyAsync())
                {
                    return BadRequest(new { message = "Tutores ja existem" });
                }

                var tutores = new List<Tutor>
                {
                    new Tutor { Nome = "Admin Sistema", Telefone = "(11) 3333-3333", Endereco = "Rua Admin, 100", UsuarioId = 1, DataCadastro = DateTime.Now },
                    new Tutor { Nome = "Carlos Silva", Telefone = "(11) 99999-9999", Endereco = "Rua das Flores, 200", UsuarioId = 5, DataCadastro = DateTime.Now },
                    new Tutor { Nome = "Ana Paula Costa", Telefone = "(11) 88888-8888", Endereco = "Av. Principal, 300", UsuarioId = null, DataCadastro = DateTime.Now }
                };

                _context.Tutores.AddRange(tutores);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Tutores criados", total = tutores.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar categorias (Passo 4 de 10)
        /// </summary>
        [HttpPost("categorias")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarCategorias()
        {
            try
            {
                if (await _context.Categorias.AnyAsync())
                {
                    return BadRequest(new { message = "Categorias ja existem" });
                }

                var categorias = new List<Categoria>
                {
                    new Categoria { Nome = "Alimentos", Descricao = "Racao e alimentos para animais" },
                    new Categoria { Nome = "Higiene", Descricao = "Produtos de higiene e limpeza" },
                    new Categoria { Nome = "Brinquedos", Descricao = "Brinquedos e entretenimento" },
                    new Categoria { Nome = "Medicamentos", Descricao = "Medicamentos e suplementos" },
                    new Categoria { Nome = "Acessorios", Descricao = "Coleiras, guias e acessorios" },
                    new Categoria { Nome = "Camas e Casinhas", Descricao = "Locais de descanso" }
                };

                _context.Categorias.AddRange(categorias);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Categorias criadas", total = categorias.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar fornecedores (Passo 5 de 10)
        /// </summary>
        [HttpPost("fornecedores")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarFornecedores()
        {
            try
            {
                if (await _context.Fornecedores.AnyAsync())
                {
                    return BadRequest(new { message = "Fornecedores ja existem" });
                }

                var fornecedores = new List<Fornecedor>
                {
                    new Fornecedor { Nome = "PetFood Distribuidora", Cnpj = "12.345.678/0001-90", Email = "contato@petfood.com.br", Telefone = "(11) 3000-1000", Endereco = "Av. Industrial, 1000", RazaoSocial = "PetFood LTDA", Contato = "Joao Almeida" },
                    new Fornecedor { Nome = "Higiene Pet Brasil", Cnpj = "98.765.432/0001-10", Email = "vendas@higienepet.com.br", Telefone = "(11) 3000-2000", Endereco = "Rua Comercial, 500", RazaoSocial = "Higiene Pet S/A", Contato = "Maria Silva" },
                    new Fornecedor { Nome = "Brinquedos e Cia", Cnpj = "45.678.901/0001-23", Email = "comercial@brinquedos.com.br", Telefone = "(11) 3000-3000", Endereco = "Av. dos Brinquedos, 234", RazaoSocial = "Brinquedos LTDA", Contato = "Pedro Santos" },
                    new Fornecedor { Nome = "VetMed Suprimentos", Cnpj = "78.901.234/0001-56", Email = "atendimento@vetmed.com.br", Telefone = "(11) 3000-4000", Endereco = "Rua Farmaceutica, 789", RazaoSocial = "VetMed LTDA", Contato = "Ana Costa" }
                };

                _context.Fornecedores.AddRange(fornecedores);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Fornecedores criados", total = fornecedores.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar produtos (Passo 6 de 10)
        /// </summary>
        [HttpPost("produtos")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarProdutos()
        {
            try
            {
                if (await _context.Produtos.AnyAsync())
                {
                    return BadRequest(new { message = "Produtos ja existem" });
                }

                var produtos = new List<Produto>
                {
                    new Produto { Nome = "Racao Premium Caes 15kg", Descricao = "Racao super premium para caes adultos", Preco = 189.90m, QuantidadeEstoque = 45, CategoriaId = 1, FornecedorId = 1, Ativo = true, CodigoBarras = "7891234567890" },
                    new Produto { Nome = "Racao Premium Gatos 5kg", Descricao = "Racao especial para gatos adultos", Preco = 95.90m, QuantidadeEstoque = 60, CategoriaId = 1, FornecedorId = 1, Ativo = true, CodigoBarras = "7891234567891" },
                    new Produto { Nome = "Racao Filhotes 3kg", Descricao = "Racao para filhotes ate 12 meses", Preco = 78.50m, QuantidadeEstoque = 35, CategoriaId = 1, FornecedorId = 1, Ativo = true, CodigoBarras = "7891234567892" },
                    new Produto { Nome = "Shampoo Neutro 500ml", Descricao = "Shampoo hipoalergenico pH balanceado", Preco = 42.90m, QuantidadeEstoque = 80, CategoriaId = 2, FornecedorId = 2, Ativo = true, CodigoBarras = "7891234567893" },
                    new Produto { Nome = "Condicionador Pelos Longos 500ml", Descricao = "Condicionador para pelos longos", Preco = 48.90m, QuantidadeEstoque = 55, CategoriaId = 2, FornecedorId = 2, Ativo = true, CodigoBarras = "7891234567894" },
                    new Produto { Nome = "Kit Escova + Pente", Descricao = "Kit completo para escovacao", Preco = 35.90m, QuantidadeEstoque = 40, CategoriaId = 2, FornecedorId = 2, Ativo = true, CodigoBarras = "7891234567895" },
                    new Produto { Nome = "Bola de Borracha Resistente", Descricao = "Bola super resistente", Preco = 29.90m, QuantidadeEstoque = 120, CategoriaId = 3, FornecedorId = 3, Ativo = true, CodigoBarras = "7891234567896" },
                    new Produto { Nome = "Corda para Morder 3 Nos", Descricao = "Corda resistente com 3 nos", Preco = 24.90m, QuantidadeEstoque = 95, CategoriaId = 3, FornecedorId = 3, Ativo = true, CodigoBarras = "7891234567897" },
                    new Produto { Nome = "Arranhador para Gatos 60cm", Descricao = "Arranhador em sisal", Preco = 149.90m, QuantidadeEstoque = 25, CategoriaId = 3, FornecedorId = 3, Ativo = true, CodigoBarras = "7891234567898" },
                    new Produto { Nome = "Antipulgas e Carrapatos", Descricao = "Protecao por 30 dias", Preco = 68.90m, QuantidadeEstoque = 70, CategoriaId = 4, FornecedorId = 4, Ativo = true, CodigoBarras = "7891234567899" },
                    new Produto { Nome = "Vermifugo Comprimido", Descricao = "Vermifugo amplo espectro", Preco = 45.90m, QuantidadeEstoque = 85, CategoriaId = 4, FornecedorId = 4, Ativo = true, CodigoBarras = "7891234567800" },
                    new Produto { Nome = "Suplemento Vitaminico", Descricao = "Suplemento completo", Preco = 89.90m, QuantidadeEstoque = 50, CategoriaId = 4, FornecedorId = 4, Ativo = true, CodigoBarras = "7891234567801" },
                    new Produto { Nome = "Coleira Ajustavel Nylon", Descricao = "Coleira resistente", Preco = 38.90m, QuantidadeEstoque = 65, CategoriaId = 5, FornecedorId = 2, Ativo = true, CodigoBarras = "7891234567802" },
                    new Produto { Nome = "Guia Retratil 5m", Descricao = "Guia retratil automatica", Preco = 79.90m, QuantidadeEstoque = 40, CategoriaId = 5, FornecedorId = 2, Ativo = true, CodigoBarras = "7891234567803" },
                    new Produto { Nome = "Cama Ortopedica Grande", Descricao = "Cama com espuma especial", Preco = 259.90m, QuantidadeEstoque = 15, CategoriaId = 6, FornecedorId = 3, Ativo = true, CodigoBarras = "7891234567804" }
                };

                _context.Produtos.AddRange(produtos);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Produtos criados", total = produtos.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar imagens de produtos (Passo 7 de 10)
        /// </summary>
        [HttpPost("imagens")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarImagens()
        {
            try
            {
                if (await _context.ProdutoImagens.AnyAsync())
                {
                    return BadRequest(new { message = "Imagens ja existem" });
                }

                var imagens = new List<ProdutoImagem>
                {
                    new ProdutoImagem { ProdutoId = 1, Url = "assets/images/products/racao-caes.jpg" },
                    new ProdutoImagem { ProdutoId = 2, Url = "assets/images/products/racao-gatos.jpg" },
                    new ProdutoImagem { ProdutoId = 3, Url = "assets/images/products/racao-filhotes.jpg" },
                    new ProdutoImagem { ProdutoId = 4, Url = "assets/images/products/shampoo.jpg" },
                    new ProdutoImagem { ProdutoId = 5, Url = "assets/images/products/condicionador.jpg" },
                    new ProdutoImagem { ProdutoId = 7, Url = "assets/images/products/brinquedo-bola.jpg" },
                    new ProdutoImagem { ProdutoId = 9, Url = "assets/images/products/arranhador.jpg" },
                    new ProdutoImagem { ProdutoId = 13, Url = "assets/images/products/coleira.jpg" },
                    new ProdutoImagem { ProdutoId = 15, Url = "assets/images/products/cama.jpg" }
                };

                _context.ProdutoImagens.AddRange(imagens);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Imagens criadas", total = imagens.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar servicos (Passo 8 de 10)
        /// </summary>
        [HttpPost("servicos")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarServicos()
        {
            try
            {
                if (await _context.Servicos.AnyAsync())
                {
                    return BadRequest(new { message = "Servicos ja existem" });
                }

                var servicos = new List<Servico>
                {
                    new Servico { Nome = "Consulta Veterinaria", Descricao = "Consulta clinica geral", Preco = 180.00m, DuracaoMinutos = 60, Ativo = true, CargosResponsaveis = "Veterinario", FuncionarioResponsavelId = 1 },
                    new Servico { Nome = "Banho Simples", Descricao = "Banho com shampoo neutro", Preco = 65.00m, DuracaoMinutos = 40, Ativo = true, CargosResponsaveis = "Tosador,Atendente", FuncionarioResponsavelId = null },
                    new Servico { Nome = "Banho e Tosa", Descricao = "Banho completo + tosa", Preco = 120.00m, DuracaoMinutos = 90, Ativo = true, CargosResponsaveis = "Tosador", FuncionarioResponsavelId = 2 },
                    new Servico { Nome = "Tosa Higienica", Descricao = "Tosa focada em higiene", Preco = 80.00m, DuracaoMinutos = 50, Ativo = true, CargosResponsaveis = "Tosador", FuncionarioResponsavelId = 2 },
                    new Servico { Nome = "Vacinacao Multipla", Descricao = "Aplicacao de vacina V10 ou V8", Preco = 120.00m, DuracaoMinutos = 30, Ativo = true, CargosResponsaveis = "Veterinario", FuncionarioResponsavelId = 1 },
                    new Servico { Nome = "Limpeza de Orelhas", Descricao = "Limpeza completa", Preco = 35.00m, DuracaoMinutos = 20, Ativo = true, CargosResponsaveis = "Tosador,Atendente", FuncionarioResponsavelId = null },
                    new Servico { Nome = "Corte de Unhas", Descricao = "Corte e lixamento", Preco = 30.00m, DuracaoMinutos = 15, Ativo = true, CargosResponsaveis = "Tosador,Veterinario,Atendente", FuncionarioResponsavelId = null },
                    new Servico { Nome = "Aplicacao Antipulgas", Descricao = "Aplicacao topica", Preco = 50.00m, DuracaoMinutos = 20, Ativo = true, CargosResponsaveis = "Veterinario,Tosador", FuncionarioResponsavelId = null }
                };

                _context.Servicos.AddRange(servicos);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Servicos criados", total = servicos.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Vincular servicos aos funcionarios (Passo 9 de 10)
        /// </summary>
        [HttpPost("vinculos")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarVinculos()
        {
            try
            {
                var vinculos = new List<ServicoFuncionario>
                {
                    new ServicoFuncionario { ServicoId = 1, FuncionarioId = 1 },
                    new ServicoFuncionario { ServicoId = 2, FuncionarioId = 2 },
                    new ServicoFuncionario { ServicoId = 2, FuncionarioId = 3 },
                    new ServicoFuncionario { ServicoId = 3, FuncionarioId = 2 },
                    new ServicoFuncionario { ServicoId = 4, FuncionarioId = 2 },
                    new ServicoFuncionario { ServicoId = 5, FuncionarioId = 1 },
                    new ServicoFuncionario { ServicoId = 6, FuncionarioId = 2 },
                    new ServicoFuncionario { ServicoId = 6, FuncionarioId = 3 },
                    new ServicoFuncionario { ServicoId = 7, FuncionarioId = 1 },
                    new ServicoFuncionario { ServicoId = 7, FuncionarioId = 2 },
                    new ServicoFuncionario { ServicoId = 7, FuncionarioId = 3 },
                    new ServicoFuncionario { ServicoId = 8, FuncionarioId = 1 },
                    new ServicoFuncionario { ServicoId = 8, FuncionarioId = 2 }
                };

                _context.ServicoFuncionarios.AddRange(vinculos);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Vinculos criados", total = vinculos.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar animais/pets (Passo 10 de 10)
        /// </summary>
        [HttpPost("animais")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarAnimais()
        {
            try
            {
                if (await _context.Animais.AnyAsync())
                {
                    return BadRequest(new { message = "Animais ja existem" });
                }

                var animais = new List<Animal>
                {
                    new Animal { Nome = "Rex", Especie = "Cao", Raca = "Labrador", Sexo = "Macho", DataNascimento = new DateTime(2020, 5, 15), Pelagem = "Curta", Observacoes = "Muito amigavel e obediente", TutorId = 2 },
                    new Animal { Nome = "Mimi", Especie = "Gato", Raca = "Siames", Sexo = "Femea", DataNascimento = new DateTime(2019, 8, 20), Pelagem = "Curta", Observacoes = "Um pouco arisca", TutorId = 2 },
                    new Animal { Nome = "Thor", Especie = "Cao", Raca = "Rottweiler", Sexo = "Macho", DataNascimento = new DateTime(2021, 3, 10), Pelagem = "Curta", Observacoes = "Protetor e leal", TutorId = 2 },
                    new Animal { Nome = "Luna", Especie = "Gato", Raca = "Persa", Sexo = "Femea", DataNascimento = new DateTime(2022, 1, 5), Pelagem = "Longa", Observacoes = "Calma e carinhosa", TutorId = 3 },
                    new Animal { Nome = "Spike", Especie = "Cao", Raca = "Pug", Sexo = "Macho", DataNascimento = new DateTime(2021, 11, 22), Pelagem = "Curta", Observacoes = "Muito brincalhao", TutorId = 3 },
                    new Animal { Nome = "Mel", Especie = "Gato", Raca = "SRD", Sexo = "Femea", DataNascimento = new DateTime(2020, 7, 8), Pelagem = "Media", Observacoes = "Resgatada da rua", TutorId = 3 }
                };

                _context.Animais.AddRange(animais);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Animais criados", total = animais.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar vendas de teste (Passo 11 de 12)
        /// </summary>
        [HttpPost("vendas")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarVendas()
        {
            try
            {
                if (await _context.Vendas.AnyAsync())
                {
                    return BadRequest(new { message = "Vendas ja existem" });
                }

                // Venda 1: Carlos Silva (cliente@example.com) compra produtos
                var venda1 = new Venda
                {
                    TutorId = 2, // Carlos Silva
                    FuncionarioId = 3, // Pedro Oliveira (Atendente)
                    DataVenda = DateTime.Now.AddDays(-5),
                    FormaPagamento = "Cartao",
                    ValorTotal = 0, // Será calculado
                    Observacoes = "Compra de produtos para o Rex"
                };

                var itensVenda1 = new List<ItemVenda>
                {
                    new ItemVenda { Venda = venda1, ProdutoId = 1, Quantidade = 1, PrecoUnitario = 189.90m }, // Ração Cães 15kg
                    new ItemVenda { Venda = venda1, ProdutoId = 4, Quantidade = 2, PrecoUnitario = 42.90m },  // Shampoo
                    new ItemVenda { Venda = venda1, ProdutoId = 7, Quantidade = 1, PrecoUnitario = 29.90m }   // Bola
                };

                venda1.ValorTotal = itensVenda1.Sum(i => i.Quantidade * i.PrecoUnitario);
                venda1.Itens = itensVenda1;

                // Venda 2: Carlos Silva compra serviço
                var venda2 = new Venda
                {
                    TutorId = 2, // Carlos Silva
                    FuncionarioId = 2, // Maria Santos (Tosador)
                    DataVenda = DateTime.Now.AddDays(-2),
                    FormaPagamento = "Dinheiro",
                    ValorTotal = 0,
                    Observacoes = "Banho e tosa do Rex"
                };

                var itensVenda2 = new List<ItemVenda>
                {
                    new ItemVenda { Venda = venda2, ServicoId = 3, Quantidade = 1, PrecoUnitario = 120.00m } // Banho e Tosa
                };

                venda2.ValorTotal = itensVenda2.Sum(i => i.Quantidade * i.PrecoUnitario);
                venda2.Itens = itensVenda2;

                // Venda 3: Admin Sistema (para teste)
                var venda3 = new Venda
                {
                    TutorId = 1, // Admin Sistema
                    FuncionarioId = 3, // Pedro Oliveira
                    DataVenda = DateTime.Now.AddDays(-1),
                    FormaPagamento = "PIX",
                    ValorTotal = 0,
                    Observacoes = "Compra teste"
                };

                var itensVenda3 = new List<ItemVenda>
                {
                    new ItemVenda { Venda = venda3, ProdutoId = 2, Quantidade = 1, PrecoUnitario = 95.90m },  // Ração Gatos
                    new ItemVenda { Venda = venda3, ServicoId = 1, Quantidade = 1, PrecoUnitario = 180.00m } // Consulta
                };

                venda3.ValorTotal = itensVenda3.Sum(i => i.Quantidade * i.PrecoUnitario);
                venda3.Itens = itensVenda3;

                var vendas = new List<Venda> { venda1, venda2, venda3 };

                _context.Vendas.AddRange(vendas);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Vendas criadas", total = vendas.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Criar agendamentos de teste (Passo 12 de 12)
        /// </summary>
        [HttpPost("agendamentos")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CriarAgendamentos()
        {
            try
            {
                if (await _context.Agendamentos.AnyAsync())
                {
                    return BadRequest(new { message = "Agendamentos ja existem" });
                }

                var agendamentos = new List<Agendamento>
                {
                    // Agendamento passado (concluído)
                    new Agendamento 
                    { 
                        AnimalId = 1, // Rex
                        ServicoId = 1, // Consulta Veterinária
                        FuncionarioId = 1, // Dr. João Silva
                        DataHora = DateTime.Now.AddDays(-3).AddHours(10), 
                        Status = "Concluido",
                        Observacoes = "Consulta de rotina - Rex estava bem"
                    },
                    
                    // Agendamento futuro (confirmado)
                    new Agendamento 
                    { 
                        AnimalId = 2, // Mimi
                        ServicoId = 3, // Banho e Tosa
                        FuncionarioId = 2, // Maria Santos
                        DataHora = DateTime.Now.AddDays(2).AddHours(14), 
                        Status = "Confirmado",
                        Observacoes = "Primeira tosa da Mimi"
                    },
                    
                    // Agendamento futuro (pendente)
                    new Agendamento 
                    { 
                        AnimalId = 4, // Luna
                        ServicoId = 2, // Banho Simples
                        FuncionarioId = 2, // Maria Santos
                        DataHora = DateTime.Now.AddDays(5).AddHours(16), 
                        Status = "Pendente",
                        Observacoes = "Banho mensal da Luna"
                    }
                };

                _context.Agendamentos.AddRange(agendamentos);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Agendamentos criados", total = agendamentos.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        #endregion

        #region Popular Completo

        /// <summary>
        /// Popular banco de dados completo
        /// </summary>
        /// <remarks>
        /// Executa TODOS os passos de 1 a 12 em sequencia:
        /// 
        /// 1. Usuarios (5 usuarios)
        /// 2. Funcionarios (3 funcionarios)
        /// 3. Tutores (3 tutores)
        /// 4. Categorias (6 categorias)
        /// 5. Fornecedores (4 fornecedores)
        /// 6. Produtos (15 produtos)
        /// 7. Imagens (9 imagens)
        /// 8. Servicos (8 servicos)
        /// 9. Vinculos (13 vinculos)
        /// 10. Animais (6 animais)
        /// 11. Vendas (3 vendas de teste)
        /// 12. Agendamentos (3 agendamentos de teste)
        /// 
        /// Tempo estimado: 5-10 segundos
        /// 
        /// IMPORTANTE: Execute DELETE /api/Seed/limpar antes se o banco tiver dados
        /// 
        /// CREDENCIAIS CRIADAS (SENHAS DIFERENTES):
        /// - admin@sigapet.com / admin2024
        /// - vet@sigapet.com / vet123
        /// - tosador@sigapet.com / tosador123
        /// - atendente@sigapet.com / atendente123
        /// - cliente@example.com / cliente123
        /// </remarks>
        [HttpPost("popular-completo")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PopularCompleto()
        {
            var inicio = DateTime.Now;
            var passos = new Dictionary<string, string>();

            try
            {
                // Verificar se já tem dados
                if (await _context.Usuarios.AnyAsync())
                {
                    return BadRequest(new { 
                        success = false,
                        message = "Banco ja possui dados. Execute DELETE /api/Seed/limpar primeiro" 
                    });
                }

                // Executar todos os passos
                await CriarUsuarios();
                passos.Add("1_usuarios", "OK");

                await CriarFuncionarios();
                passos.Add("2_funcionarios", "OK");

                await CriarTutores();
                passos.Add("3_tutores", "OK");

                await CriarCategorias();
                passos.Add("4_categorias", "OK");

                await CriarFornecedores();
                passos.Add("5_fornecedores", "OK");

                await CriarProdutos();
                passos.Add("6_produtos", "OK");

                await CriarImagens();
                passos.Add("7_imagens", "OK");

                await CriarServicos();
                passos.Add("8_servicos", "OK");

                await CriarVinculos();
                passos.Add("9_vinculos", "OK");

                await CriarAnimais();
                passos.Add("10_animais", "OK");

                await CriarVendas();
                passos.Add("11_vendas", "OK");

                await CriarAgendamentos();
                passos.Add("12_agendamentos", "OK");

                var fim = DateTime.Now;
                var duracao = (fim - inicio).TotalSeconds;

                return Ok(new {
                    success = true,
                    message = "Banco de dados populado com sucesso",
                    tempo_segundos = Math.Round(duracao, 2),
                    passos_executados = passos,
                    resumo = new {
                        usuarios = 5,
                        funcionarios = 3,
                        tutores = 3,
                        categorias = 6,
                        fornecedores = 4,
                        produtos = 15,
                        imagens = 9,
                        servicos = 8,
                        vinculos = 13,
                        animais = 6,
                        vendas = 3,
                        agendamentos = 3
                    },
                    credenciais = new {
                        admin = new { email = "admin@sigapet.com", senha = "admin2024" },
                        veterinario = new { email = "vet@sigapet.com", senha = "vet123" },
                        tosador = new { email = "tosador@sigapet.com", senha = "tosador123" },
                        atendente = new { email = "atendente@sigapet.com", senha = "atendente123" },
                        cliente = new { email = "cliente@example.com", senha = "cliente123" }
                    },
                    dados_teste = new {
                        vendas_criadas = "Carlos Silva possui 2 vendas para teste",
                        agendamentos_criados = "3 agendamentos (1 passado, 2 futuros) para teste",
                        animais_cadastrados = "6 pets distribuídos entre Carlos Silva e Admin"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false,
                    message = "Erro ao popular banco", 
                    passo_com_erro = passos.Keys.LastOrDefault(),
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        #endregion

        #region Status

        /// <summary>
        /// Verificar status do banco de dados
        /// </summary>
        /// <remarks>
        /// Retorna informacoes sobre a quantidade de registros em cada tabela
        /// </remarks>
        [HttpGet("status")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> Status()
        {
            try
            {
                var contadores = new
                {
                    usuarios = await _context.Usuarios.CountAsync(),
                    funcionarios = await _context.Funcionarios.CountAsync(),
                    tutores = await _context.Tutores.CountAsync(),
                    categorias = await _context.Categorias.CountAsync(),
                    fornecedores = await _context.Fornecedores.CountAsync(),
                    produtos = await _context.Produtos.CountAsync(),
                    imagens_produtos = await _context.ProdutoImagens.CountAsync(),
                    servicos = await _context.Servicos.CountAsync(),
                    vinculos_servicos = await _context.ServicoFuncionarios.CountAsync(),
                    animais = await _context.Animais.CountAsync(),
                    vendas = await _context.Vendas.CountAsync(),
                    itens_vendas = await _context.ItensVenda.CountAsync(),
                    agendamentos = await _context.Agendamentos.CountAsync()
                };

                var total = contadores.usuarios + contadores.funcionarios + contadores.tutores + 
                           contadores.categorias + contadores.fornecedores + contadores.produtos + 
                           contadores.imagens_produtos + contadores.servicos + contadores.vinculos_servicos + 
                           contadores.animais + contadores.vendas + contadores.itens_vendas + 
                           contadores.agendamentos;

                var banco_vazio = total == 0;

                return Ok(new
                {
                    banco = "SIGA-PET",
                    timestamp = DateTime.Now,
                    banco_vazio = banco_vazio,
                    total_registros = total,
                    tabelas = contadores,
                    detalhes = new {
                        dados_principais = contadores.usuarios + contadores.tutores + contadores.animais,
                        dados_produtos = contadores.produtos + contadores.categorias + contadores.fornecedores,
                        dados_servicos = contadores.servicos + contadores.funcionarios,
                        dados_transacionais = contadores.vendas + contadores.agendamentos
                    },
                    recomendacao = banco_vazio 
                        ? "Execute POST /api/Seed/popular-completo para criar dados de teste" 
                        : "Banco possui dados. Execute DELETE /api/Seed/limpar para resetar"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        #endregion
    }
}
