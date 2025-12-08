using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.Models;

namespace SIGA_PET.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento do banco de dados
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(AppDbContext context, ILogger<DatabaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// ?? RESETAR E POPULAR BANCO DE DADOS COMPLETO
        /// </summary>
        /// <remarks>
        /// ?? **ATENÇÃO: Esta operação irá DELETAR TODOS OS DADOS e recriar o banco do zero!**
        /// 
        /// Este endpoint executa as seguintes operações:
        /// 1. **Deleta o banco de dados existente**
        /// 2. **Cria todas as tabelas do zero**
        /// 3. **Popula com dados de exemplo**
        /// 
        /// **Dados que serão criados:**
        /// - ? 7 Usuários (1 Admin, 3 Funcionários, 3 Tutores)
        /// - ? 3 Funcionários (Veterinário, Tosador, Atendente)
        /// - ? 4 Tutores (3 com login, 1 sem login)
        /// - ? 5 Animais (Cães e Gatos)
        /// - ? 6 Categorias de produtos
        /// - ? 3 Fornecedores
        /// - ? 8 Produtos com imagens
        /// - ? 6 Serviços (Consulta, Banho, Tosa, etc)
        /// - ? 5 Agendamentos
        /// - ? 3 Vendas com itens
        /// 
        /// **Credenciais de teste criadas:**
        /// 
        /// | Tipo | Email | Senha | Descrição |
        /// |------|-------|-------|-----------|
        /// | Admin | admin@sigapet.com | senha123 | Administrador do sistema |
        /// | Veterinário | carlos.vet@sigapet.com | senha123 | Dr. Carlos Veterinário |
        /// | Tosador | ana.tosa@sigapet.com | senha123 | Ana Tosadora |
        /// | Atendente | pedro.atend@sigapet.com | senha123 | Pedro Atendente |
        /// | Cliente/Tutor | maria.silva@email.com | senha123 | Maria Silva (tem pets) |
        /// | Cliente/Tutor | joao.santos@email.com | senha123 | João Santos (tem pets) |
        /// | Cliente/Tutor | paula.oli@email.com | senha123 | Paula Oliveira (tem pets) |
        /// 
        /// **?? Tempo estimado: 5-10 segundos**
        /// 
        /// **Exemplo de resposta bem-sucedida:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Banco de dados resetado e populado com sucesso!",
        ///   "resumo": {
        ///     "usuarios": 7,
        ///     "funcionarios": 3,
        ///     "tutores": 4,
        ///     "animais": 5,
        ///     "categorias": 6,
        ///     "fornecedores": 3,
        ///     "produtos": 8,
        ///     "servicos": 6,
        ///     "agendamentos": 5,
        ///     "vendas": 3
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Banco resetado e populado com sucesso</response>
        /// <response code="500">Erro ao resetar o banco de dados</response>
        [HttpPost("reset-e-popular")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> ResetarEPopular()
        {
            try
            {
                _logger.LogWarning("?? Iniciando RESET COMPLETO do banco de dados...");

                // 1. DELETAR banco existente
                _logger.LogInformation("?? Deletando banco existente...");
                await _context.Database.EnsureDeletedAsync();

                // 2. CRIAR banco do zero
                _logger.LogInformation("??? Criando estrutura do banco...");
                await _context.Database.EnsureCreatedAsync();

                // 3. POPULAR com dados
                _logger.LogInformation("?? Populando dados iniciais...");

                // 3.1. Usuários (Base do sistema)
                var usuarios = new List<Usuario>
                {
                    new Usuario { Nome = "Admin Sistema", Email = "admin@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Admin", Ativo = true },
                    new Usuario { Nome = "Dr. Carlos Veterinário", Email = "carlos.vet@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Ana Tosadora", Email = "ana.tosa@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Pedro Atendente", Email = "pedro.atend@sigapet.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Funcionario", Ativo = true },
                    new Usuario { Nome = "Maria Silva", Email = "maria.silva@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Tutor", Ativo = true },
                    new Usuario { Nome = "João Santos", Email = "joao.santos@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Tutor", Ativo = true },
                    new Usuario { Nome = "Paula Oliveira", Email = "paula.oli@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123"), TipoUsuario = "Tutor", Ativo = true }
                };
                await _context.Usuarios.AddRangeAsync(usuarios);
                await _context.SaveChangesAsync();

                // 3.2. Funcionários
                var funcionarios = new List<Funcionario>
                {
                    new Funcionario { UsuarioId = usuarios[1].UsuarioId, Nome = "Dr. Carlos Veterinário", Cargo = "Veterinário", Telefone = "(11) 98765-1111", DataContratacao = DateTime.Now.AddYears(-1), Ativo = true },
                    new Funcionario { UsuarioId = usuarios[2].UsuarioId, Nome = "Ana Tosadora", Cargo = "Tosador", Telefone = "(11) 98765-2222", DataContratacao = DateTime.Now.AddMonths(-9), Ativo = true },
                    new Funcionario { UsuarioId = usuarios[3].UsuarioId, Nome = "Pedro Atendente", Cargo = "Atendente", Telefone = "(11) 98765-3333", DataContratacao = DateTime.Now.AddMonths(-6), Ativo = true }
                };
                await _context.Funcionarios.AddRangeAsync(funcionarios);
                await _context.SaveChangesAsync();

                // 3.3. Tutores
                var tutores = new List<Tutor>
                {
                    new Tutor { UsuarioId = usuarios[4].UsuarioId, Nome = "Maria Silva", Telefone = "(11) 99999-1111", Endereco = "Rua das Flores, 123", DataCadastro = DateTime.Now.AddMonths(-3) },
                    new Tutor { UsuarioId = usuarios[5].UsuarioId, Nome = "João Santos", Telefone = "(11) 99999-2222", Endereco = "Av. Brasil, 456", DataCadastro = DateTime.Now.AddMonths(-2) },
                    new Tutor { UsuarioId = usuarios[6].UsuarioId, Nome = "Paula Oliveira", Telefone = "(11) 99999-3333", Endereco = "Rua da Paz, 789", DataCadastro = DateTime.Now.AddMonths(-1) },
                    new Tutor { UsuarioId = null, Nome = "Cliente Avulso", Telefone = "(11) 99999-9999", Endereco = "Não informado", DataCadastro = DateTime.Now }
                };
                await _context.Tutores.AddRangeAsync(tutores);
                await _context.SaveChangesAsync();

                // 3.4. Animais
                var animais = new List<Animal>
                {
                    new Animal { TutorId = tutores[0].TutorId, Nome = "Rex", Especie = "Cão", Raca = "Labrador", DataNascimento = new DateTime(2020, 5, 10), Sexo = "Macho", Pelagem = "Curta", Observacoes = "Muito dócil e brincalhão" },
                    new Animal { TutorId = tutores[0].TutorId, Nome = "Mimi", Especie = "Gato", Raca = "Persa", DataNascimento = new DateTime(2021, 3, 15), Sexo = "Fêmea", Pelagem = "Longa", Observacoes = "Gosta de carinho" },
                    new Animal { TutorId = tutores[1].TutorId, Nome = "Bob", Especie = "Cão", Raca = "Bulldog", DataNascimento = new DateTime(2019, 8, 20), Sexo = "Macho", Pelagem = "Curta", Observacoes = "Precisa de dieta especial" },
                    new Animal { TutorId = tutores[2].TutorId, Nome = "Luna", Especie = "Gato", Raca = "Siamês", DataNascimento = new DateTime(2022, 1, 5), Sexo = "Fêmea", Pelagem = "Curta", Observacoes = "Muito ativa" },
                    new Animal { TutorId = tutores[2].TutorId, Nome = "Thor", Especie = "Cão", Raca = "Pastor Alemão", DataNascimento = new DateTime(2020, 11, 30), Sexo = "Macho", Pelagem = "Média", Observacoes = "Treinamento de guarda" }
                };
                await _context.Animais.AddRangeAsync(animais);
                await _context.SaveChangesAsync();

                // 3.5. Categorias
                var categorias = new List<Categoria>
                {
                    new Categoria { Nome = "Ração", Descricao = "Alimentos secos e úmidos para pets" },
                    new Categoria { Nome = "Brinquedos", Descricao = "Brinquedos diversos para entretenimento" },
                    new Categoria { Nome = "Higiene", Descricao = "Produtos de limpeza e higiene" },
                    new Categoria { Nome = "Acessórios", Descricao = "Coleiras, guias, camas e outros" },
                    new Categoria { Nome = "Medicamentos", Descricao = "Medicamentos e suplementos" },
                    new Categoria { Nome = "Petiscos", Descricao = "Snacks e treats para pets" }
                };
                await _context.Categorias.AddRangeAsync(categorias);
                await _context.SaveChangesAsync();

                // 3.6. Fornecedores
                var fornecedores = new List<Fornecedor>
                {
                    new Fornecedor { Nome = "PetFood Distribuidora", Cnpj = "12.345.678/0001-99", Telefone = "(11) 3000-1111", Email = "contato@petfood.com.br", Endereco = "Av. Industrial, 1000" },
                    new Fornecedor { Nome = "BrinquePet Ltda", Cnpj = "98.765.432/0001-11", Telefone = "(11) 3000-2222", Email = "vendas@brinquepet.com.br", Endereco = "Rua das Fábricas, 500" },
                    new Fornecedor { Nome = "HigienePet S.A.", Cnpj = "11.222.333/0001-44", Telefone = "(11) 3000-3333", Email = "comercial@higienepet.com.br", Endereco = "Av. Química, 200" }
                };
                await _context.Fornecedores.AddRangeAsync(fornecedores);
                await _context.SaveChangesAsync();

                // 3.7. Produtos
                var produtos = new List<Produto>
                {
                    new Produto { Nome = "Ração Premium Cães Adultos 15kg", Descricao = "Ração balanceada para cães adultos de todas as raças", Preco = 189.90m, QuantidadeEstoque = 50, Ativo = true, CodigoBarras = "7891234567890", CategoriaId = categorias[0].CategoriaId, FornecedorId = fornecedores[0].FornecedorId },
                    new Produto { Nome = "Ração Premium Gatos Adultos 10kg", Descricao = "Ração especial para gatos adultos", Preco = 149.90m, QuantidadeEstoque = 30, Ativo = true, CodigoBarras = "7891234567891", CategoriaId = categorias[0].CategoriaId, FornecedorId = fornecedores[0].FornecedorId },
                    new Produto { Nome = "Bola de Borracha Grande", Descricao = "Bola resistente para cães de porte grande", Preco = 29.90m, QuantidadeEstoque = 100, Ativo = true, CodigoBarras = "7891234567892", CategoriaId = categorias[1].CategoriaId, FornecedorId = fornecedores[1].FornecedorId },
                    new Produto { Nome = "Arranhador para Gatos", Descricao = "Arranhador com brinquedo suspenso", Preco = 89.90m, QuantidadeEstoque = 20, Ativo = true, CodigoBarras = "7891234567893", CategoriaId = categorias[1].CategoriaId, FornecedorId = fornecedores[1].FornecedorId },
                    new Produto { Nome = "Shampoo Neutro 500ml", Descricao = "Shampoo hipoalergênico para pets", Preco = 39.90m, QuantidadeEstoque = 80, Ativo = true, CodigoBarras = "7891234567894", CategoriaId = categorias[2].CategoriaId, FornecedorId = fornecedores[2].FornecedorId },
                    new Produto { Nome = "Coleira Ajustável Nylon", Descricao = "Coleira resistente tamanho M", Preco = 24.90m, QuantidadeEstoque = 150, Ativo = true, CodigoBarras = "7891234567895", CategoriaId = categorias[3].CategoriaId, FornecedorId = fornecedores[1].FornecedorId },
                    new Produto { Nome = "Cama Ortopédica Média", Descricao = "Cama confortável para pets de médio porte", Preco = 159.90m, QuantidadeEstoque = 25, Ativo = true, CodigoBarras = "7891234567896", CategoriaId = categorias[3].CategoriaId, FornecedorId = fornecedores[1].FornecedorId },
                    new Produto { Nome = "Petisco Natural Frango 200g", Descricao = "Petisco desidratado 100% natural", Preco = 19.90m, QuantidadeEstoque = 200, Ativo = true, CodigoBarras = "7891234567897", CategoriaId = categorias[5].CategoriaId, FornecedorId = fornecedores[0].FornecedorId }
                };
                await _context.Produtos.AddRangeAsync(produtos);
                await _context.SaveChangesAsync();

                // 3.8. Imagens dos Produtos
                var imagens = new List<ProdutoImagem>
                {
                    new ProdutoImagem { ProdutoId = produtos[0].ProdutoId, Url = "assets/images/products/racao-caes.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[1].ProdutoId, Url = "assets/images/products/racao-gatos.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[2].ProdutoId, Url = "assets/images/products/bola-borracha.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[3].ProdutoId, Url = "assets/images/products/arranhador.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[4].ProdutoId, Url = "assets/images/products/shampoo.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[5].ProdutoId, Url = "assets/images/products/coleira.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[6].ProdutoId, Url = "assets/images/products/cama.jpg" },
                    new ProdutoImagem { ProdutoId = produtos[7].ProdutoId, Url = "assets/images/products/petisco.jpg" }
                };
                await _context.ProdutoImagens.AddRangeAsync(imagens);
                await _context.SaveChangesAsync();

                // 3.9. Serviços
                var servicos = new List<Servico>
                {
                    new Servico { Nome = "Consulta Veterinária", Descricao = "Consulta geral com veterinário", Preco = 120.00m, DuracaoMinutos = 30, Ativo = true, FuncionarioResponsavelId = funcionarios[0].FuncionarioId, CargosResponsaveis = "Veterinário" },
                    new Servico { Nome = "Banho e Tosa Pequeno Porte", Descricao = "Banho completo e tosa para pets pequenos", Preco = 80.00m, DuracaoMinutos = 60, Ativo = true, FuncionarioResponsavelId = funcionarios[1].FuncionarioId, CargosResponsaveis = "Tosador" },
                    new Servico { Nome = "Banho e Tosa Grande Porte", Descricao = "Banho completo e tosa para pets grandes", Preco = 120.00m, DuracaoMinutos = 90, Ativo = true, FuncionarioResponsavelId = funcionarios[1].FuncionarioId, CargosResponsaveis = "Tosador" },
                    new Servico { Nome = "Vacinação", Descricao = "Aplicação de vacinas", Preco = 60.00m, DuracaoMinutos = 15, Ativo = true, FuncionarioResponsavelId = funcionarios[0].FuncionarioId, CargosResponsaveis = "Veterinário" },
                    new Servico { Nome = "Exame de Sangue", Descricao = "Coleta e análise de exames laboratoriais", Preco = 150.00m, DuracaoMinutos = 20, Ativo = true, FuncionarioResponsavelId = funcionarios[0].FuncionarioId, CargosResponsaveis = "Veterinário" },
                    new Servico { Nome = "Tosa Higiênica", Descricao = "Tosa apenas em regiões específicas", Preco = 50.00m, DuracaoMinutos = 30, Ativo = true, FuncionarioResponsavelId = funcionarios[1].FuncionarioId, CargosResponsaveis = "Tosador" }
                };
                await _context.Servicos.AddRangeAsync(servicos);
                await _context.SaveChangesAsync();

                // 3.10. Relacionamento Servico-Funcionario
                var servicoFuncionarios = new List<ServicoFuncionario>
                {
                    new ServicoFuncionario { ServicoId = servicos[0].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId },
                    new ServicoFuncionario { ServicoId = servicos[1].ServicoId, FuncionarioId = funcionarios[1].FuncionarioId },
                    new ServicoFuncionario { ServicoId = servicos[2].ServicoId, FuncionarioId = funcionarios[1].FuncionarioId },
                    new ServicoFuncionario { ServicoId = servicos[3].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId },
                    new ServicoFuncionario { ServicoId = servicos[4].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId },
                    new ServicoFuncionario { ServicoId = servicos[5].ServicoId, FuncionarioId = funcionarios[1].FuncionarioId }
                };
                await _context.ServicoFuncionarios.AddRangeAsync(servicoFuncionarios);
                await _context.SaveChangesAsync();

                // 3.11. Agendamentos
                var agendamentos = new List<Agendamento>
                {
                    new Agendamento { AnimalId = animais[0].AnimalId, ServicoId = servicos[0].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId, DataHora = DateTime.Now.AddDays(2), Status = "Confirmado", Observacoes = "Primeira consulta do Rex" },
                    new Agendamento { AnimalId = animais[1].AnimalId, ServicoId = servicos[1].ServicoId, FuncionarioId = funcionarios[1].FuncionarioId, DataHora = DateTime.Now.AddDays(3), Status = "Pendente", Observacoes = "Banho e tosa completa" },
                    new Agendamento { AnimalId = animais[2].AnimalId, ServicoId = servicos[0].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId, DataHora = DateTime.Now.AddDays(5), Status = "Pendente", Observacoes = "Check-up de rotina" },
                    new Agendamento { AnimalId = animais[3].AnimalId, ServicoId = servicos[5].ServicoId, FuncionarioId = funcionarios[1].FuncionarioId, DataHora = DateTime.Now.AddDays(1), Status = "Confirmado", Observacoes = null },
                    new Agendamento { AnimalId = animais[4].AnimalId, ServicoId = servicos[3].ServicoId, FuncionarioId = funcionarios[0].FuncionarioId, DataHora = DateTime.Now.AddDays(7), Status = "Pendente", Observacoes = "Reforço de vacina antirrábica" }
                };
                await _context.Agendamentos.AddRangeAsync(agendamentos);
                await _context.SaveChangesAsync();

                // 3.12. Vendas
                var vendas = new List<Venda>
                {
                    new Venda { UsuarioId = usuarios[4].UsuarioId, TutorId = tutores[0].TutorId, FuncionarioId = funcionarios[2].FuncionarioId, DataVenda = DateTime.Now.AddDays(-10), ValorTotal = 209.80m, FormaPagamento = "Cartão de Crédito", Observacoes = "Compra de ração e petisco" },
                    new Venda { UsuarioId = usuarios[5].UsuarioId, TutorId = tutores[1].TutorId, FuncionarioId = funcionarios[2].FuncionarioId, DataVenda = DateTime.Now.AddDays(-5), ValorTotal = 119.80m, FormaPagamento = "Dinheiro", Observacoes = "Compra de brinquedos" },
                    new Venda { UsuarioId = usuarios[6].UsuarioId, TutorId = tutores[2].TutorId, FuncionarioId = null, DataVenda = DateTime.Now.AddDays(-2), ValorTotal = 199.80m, FormaPagamento = "PIX", Observacoes = "Compra de cama e shampoo" }
                };
                await _context.Vendas.AddRangeAsync(vendas);
                await _context.SaveChangesAsync();

                // 3.13. Itens de Venda
                var itensVenda = new List<ItemVenda>
                {
                    // Venda 1: Ração + Petisco
                    new ItemVenda { VendaId = vendas[0].VendaId, ProdutoId = produtos[0].ProdutoId, Quantidade = 1, PrecoUnitario = 189.90m },
                    new ItemVenda { VendaId = vendas[0].VendaId, ProdutoId = produtos[7].ProdutoId, Quantidade = 1, PrecoUnitario = 19.90m },
                    // Venda 2: Bola + Arranhador
                    new ItemVenda { VendaId = vendas[1].VendaId, ProdutoId = produtos[2].ProdutoId, Quantidade = 1, PrecoUnitario = 29.90m },
                    new ItemVenda { VendaId = vendas[1].VendaId, ProdutoId = produtos[3].ProdutoId, Quantidade = 1, PrecoUnitario = 89.90m },
                    // Venda 3: Cama + Shampoo
                    new ItemVenda { VendaId = vendas[2].VendaId, ProdutoId = produtos[6].ProdutoId, Quantidade = 1, PrecoUnitario = 159.90m },
                    new ItemVenda { VendaId = vendas[2].VendaId, ProdutoId = produtos[4].ProdutoId, Quantidade = 1, PrecoUnitario = 39.90m }
                };
                await _context.ItensVenda.AddRangeAsync(itensVenda);
                await _context.SaveChangesAsync();

                _logger.LogInformation("? Banco de dados resetado e populado com sucesso!");

                return Ok(new
                {
                    success = true,
                    message = "? Banco de dados resetado e populado com sucesso!",
                    resumo = new
                    {
                        usuarios = usuarios.Count,
                        funcionarios = funcionarios.Count,
                        tutores = tutores.Count,
                        animais = animais.Count,
                        categorias = categorias.Count,
                        fornecedores = fornecedores.Count,
                        produtos = produtos.Count,
                        servicos = servicos.Count,
                        agendamentos = agendamentos.Count,
                        vendas = vendas.Count
                    },
                    credenciais = new
                    {
                        admin = new { email = "admin@sigapet.com", senha = "senha123" },
                        veterinario = new { email = "carlos.vet@sigapet.com", senha = "senha123" },
                        tosador = new { email = "ana.tosa@sigapet.com", senha = "senha123" },
                        cliente = new { email = "maria.silva@email.com", senha = "senha123" }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Erro ao resetar e popular banco de dados");
                return StatusCode(500, new
                {
                    success = false,
                    message = "? Erro ao resetar banco de dados",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// ?? Verificar status do banco de dados
        /// </summary>
        /// <remarks>
        /// Retorna informações sobre o estado atual do banco de dados, incluindo:
        /// - Quantidade de registros em cada tabela
        /// - Status da conexão
        /// - Versão do banco
        /// 
        /// **Exemplo de resposta:**
        /// ```json
        /// {
        ///   "status": "Online",
        ///   "versao": "Microsoft SQL Server 2019",
        ///   "tabelas": {
        ///     "Usuarios": 7,
        ///     "Tutores": 4,
        ///     "Produtos": 8,
        ///     ...
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Informações do banco retornadas com sucesso</response>
        /// <response code="500">Erro ao consultar o banco de dados</response>
        [HttpGet("status")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> VerificarStatus()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();

                if (!canConnect)
                {
                    return StatusCode(500, new
                    {
                        status = "Offline",
                        message = "Não foi possível conectar ao banco de dados"
                    });
                }

                return Ok(new
                {
                    status = "Online",
                    message = "Banco de dados conectado com sucesso",
                    tabelas = new
                    {
                        Usuarios = await _context.Usuarios.CountAsync(),
                        Funcionarios = await _context.Funcionarios.CountAsync(),
                        Tutores = await _context.Tutores.CountAsync(),
                        Animais = await _context.Animais.CountAsync(),
                        Categorias = await _context.Categorias.CountAsync(),
                        Fornecedores = await _context.Fornecedores.CountAsync(),
                        Produtos = await _context.Produtos.CountAsync(),
                        Servicos = await _context.Servicos.CountAsync(),
                        Agendamentos = await _context.Agendamentos.CountAsync(),
                        Vendas = await _context.Vendas.CountAsync()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar status do banco");
                return StatusCode(500, new
                {
                    status = "Erro",
                    message = "Erro ao verificar status do banco de dados",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// ??? Limpar todos os dados (manter estrutura)
        /// </summary>
        /// <remarks>
        /// Remove todos os dados das tabelas, mas mantém a estrutura do banco.
        /// 
        /// ?? **ATENÇÃO: Esta operação deleta todos os dados mas mantém as tabelas!**
        /// 
        /// Use este endpoint quando quiser começar do zero mas já tem a estrutura criada.
        /// </remarks>
        /// <response code="200">Dados removidos com sucesso</response>
        /// <response code="500">Erro ao limpar dados</response>
        [HttpDelete("limpar-dados")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> LimparDados()
        {
            try
            {
                _logger.LogWarning("??? Limpando todos os dados do banco...");

                // Ordem correta para evitar conflitos de FK
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ItensVenda");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Vendas");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Agendamentos");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ServicoFuncionarios");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Servicos");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ProdutoImagens");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Produtos");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Fornecedores");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Categorias");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM RegistrosProntuario");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Animais");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Tutores");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Funcionarios");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Usuarios");

                // Resetar IDENTITY
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Usuarios', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Funcionarios', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Tutores', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Animais', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Categorias', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Fornecedores', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Produtos', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Servicos', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Agendamentos', RESEED, 0)");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Vendas', RESEED, 0)");

                _logger.LogInformation("? Dados limpos com sucesso!");

                return Ok(new
                {
                    success = true,
                    message = "? Todos os dados foram removidos com sucesso. A estrutura do banco foi mantida."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Erro ao limpar dados do banco");
                return StatusCode(500, new
                {
                    success = false,
                    message = "? Erro ao limpar dados",
                    error = ex.Message
                });
            }
        }
    }
}
