using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VendaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // NOVO: GET todas as vendas (para buscar por usuário no frontend)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendaDto>>> GetVendas()
        {
            try
            {
                var vendas = await _context.Vendas
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Produto)
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Servico)
                    .Include(v => v.Tutor)
                    .Include(v => v.Funcionario)
                    .OrderByDescending(v => v.DataVenda)
                    .AsNoTracking()
                    .ToListAsync();

                var vendasDto = _mapper.Map<IEnumerable<VendaDto>>(vendas);
                return Ok(vendasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Venda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VendaDto>> GetVenda(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Servico)
                .FirstOrDefaultAsync(v => v.VendaId == id);

            if (venda == null) return NotFound();

            return Ok(_mapper.Map<VendaDto>(venda));
        }

        /// <summary>
        /// ?? Buscar vendas por tutor
        /// </summary>
        /// <param name="tutorId">ID do tutor</param>
        /// <remarks>
        /// Retorna todas as vendas realizadas por um tutor específico.
        /// 
        /// **Exemplo de uso:** `/api/Venda/tutor/1`
        /// </remarks>
        /// <response code="200">Lista de vendas do tutor retornada com sucesso</response>
        /// <response code="404">Tutor não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("tutor/{tutorId}")]
        [ProducesResponseType(typeof(IEnumerable<VendaDto>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<IEnumerable<VendaDto>>> GetVendasByTutor(int tutorId)
        {
            try
            {
                // Verificar se o tutor existe
                var tutorExiste = await _context.Tutores.AnyAsync(t => t.TutorId == tutorId);
                if (!tutorExiste)
                    return NotFound($"Tutor com ID {tutorId} não encontrado.");

                var vendas = await _context.Vendas
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Produto)
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Servico)
                    .Include(v => v.Tutor)
                    .Include(v => v.Funcionario)
                    .Where(v => v.TutorId == tutorId)
                    .OrderByDescending(v => v.DataVenda)
                    .AsNoTracking()
                    .ToListAsync();

                var vendasDto = _mapper.Map<IEnumerable<VendaDto>>(vendas);
                return Ok(vendasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// ?? Buscar vendas por ID do usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <remarks>
        /// Retorna todas as vendas associadas a um usuário, seja diretamente 
        /// ou através do seu perfil de tutor.
        /// 
        /// **Exemplo de uso:** `/api/Venda/usuario/5`
        /// </remarks>
        /// <response code="200">Lista de vendas do usuário retornada com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(IEnumerable<VendaDto>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<IEnumerable<VendaDto>>> GetVendasByUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return NotFound($"Usuário com ID {usuarioId} não encontrado.");

                // Encontrar o tutorId associado a este usuário, se houver
                var tutor = await _context.Tutores.FirstOrDefaultAsync(t => t.UsuarioId == usuarioId);

                // Buscar vendas onde o TutorId corresponde ao tutor do usuário
                // OU onde o UsuarioId da venda corresponde diretamente.
                var vendas = await _context.Vendas
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Produto)
                    .Include(v => v.Itens)
                        .ThenInclude(i => i.Servico)
                    .Include(v => v.Tutor)
                    .Include(v => v.Funcionario)
                    .Where(v => (tutor != null && v.TutorId == tutor.TutorId) || v.UsuarioId == usuarioId)
                    .OrderByDescending(v => v.DataVenda)
                    .AsNoTracking()
                    .ToListAsync();

                var vendasDto = _mapper.Map<IEnumerable<VendaDto>>(vendas);
                return Ok(vendasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// ? Criar nova venda (permite compra sem ser tutor)
        /// </summary>
        /// <param name="createVendaDto">Dados da venda</param>
        /// <remarks>
        /// Cria uma nova venda no sistema.
        /// 
        /// **NOVIDADE:** Permite compras sem ser tutor registrado!
        /// - Se não informar tutorId, a venda será criada como "Cliente Avulso"
        /// - Se informar dados de cliente (nome, email, telefone), cria tutor automaticamente
        /// 
        /// **Validações aplicadas:**
        /// - Pelo menos um item deve ser fornecido
        /// - Produtos devem ter estoque suficiente
        /// - Serviços devem estar ativos
        /// 
        /// **Exemplo de requisição (cliente avulso):**
        /// ```json
        /// {
        ///   "itens": [
        ///     {
        ///       "produtoId": 1,
        ///       "quantidade": 2
        ///     }
        ///   ],
        ///   "formaPagamento": "Dinheiro",
        ///   "observacoes": "Venda balcão"
        /// }
        /// ```
        /// 
        /// **Exemplo de requisição (criar tutor na hora):**
        /// ```json
        /// {
        ///   "nomeCliente": "João Silva",
        ///   "emailCliente": "joao@email.com", 
        ///   "telefoneCliente": "(11) 99999-9999",
        ///   "enderecoCliente": "Rua Exemplo, 123",
        ///   "itens": [
        ///     {
        ///       "produtoId": 1,
        ///       "quantidade": 2
        ///     }
        ///   ],
        ///   "formaPagamento": "Cartão"
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Venda criada com sucesso</response>
        /// <response code="400">Dados inválidos ou estoque insuficiente</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(VendaDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<VendaDto>> CreateVenda([FromBody] CreateVendaDto createVendaDto)
        {
            // Validação preliminar dos itens para evitar estado inconsistente
            if (createVendaDto.Itens == null || !createVendaDto.Itens.Any())
                return BadRequest("? A venda deve conter pelo menos um item.");

            foreach (var itemDto in createVendaDto.Itens)
            {
                if (itemDto.ProdutoId == null && itemDto.ServicoId == null)
                    return BadRequest("? Cada item da venda deve ter um Produto OU um Serviço vinculado.");

                if (itemDto.ProdutoId != null && itemDto.ServicoId != null)
                    return BadRequest("? Um item não pode ser Produto e Serviço ao mesmo tempo.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venda = _mapper.Map<Venda>(createVendaDto);
                venda.DataVenda = DateTime.Now;

                // Corrigido: Garante que o UsuarioId do DTO seja atribuído à venda.
                venda.UsuarioId = createVendaDto.UsuarioId;

                // ?? NOVO: Lógica para criar tutor automaticamente se dados foram fornecidos
                if (createVendaDto.TutorId == null && !string.IsNullOrEmpty(createVendaDto.NomeCliente))
                {
                    // Verificar se já existe tutor com mesmo email ou telefone
                    Tutor? tutorExistente = null;
                    
                    if (!string.IsNullOrEmpty(createVendaDto.EmailCliente))
                    {
                        // Buscar por email no usuário associado
                        var usuarioExistente = await _context.Usuarios
                            .FirstOrDefaultAsync(u => u.Email == createVendaDto.EmailCliente);
                        if (usuarioExistente != null)
                        {
                            tutorExistente = await _context.Tutores
                                .FirstOrDefaultAsync(t => t.UsuarioId == usuarioExistente.UsuarioId);
                        }
                    }

                    if (tutorExistente == null && !string.IsNullOrEmpty(createVendaDto.TelefoneCliente))
                    {
                        // Buscar por telefone
                        tutorExistente = await _context.Tutores
                            .FirstOrDefaultAsync(t => t.Telefone == createVendaDto.TelefoneCliente);
                    }

                    if (tutorExistente != null)
                    {
                        // Usar tutor existente
                        venda.TutorId = tutorExistente.TutorId;
                    }
                    else
                    {
                        // Criar novo tutor simplificado (sem usuário/senha)
                        var novoTutor = new Tutor
                        {
                            Nome = createVendaDto.NomeCliente,
                            Telefone = createVendaDto.TelefoneCliente ?? "",
                            Endereco = createVendaDto.EnderecoCliente ?? "Não informado",
                            DataCadastro = DateTime.Now,
                            UsuarioId = null // Tutor sem login, apenas para compras
                        };

                        _context.Tutores.Add(novoTutor);
                        await _context.SaveChangesAsync();
                        
                        venda.TutorId = novoTutor.TutorId;
                    }
                }
                // Se o usuário estiver logado e tiver um tutorId, mas não foi passado no DTO,
                // vamos tentar associá-lo.
                else if (venda.TutorId == null && venda.UsuarioId.HasValue)
                {
                    var tutorDoUsuario = await _context.Tutores.FirstOrDefaultAsync(t => t.UsuarioId == venda.UsuarioId.Value);
                    if (tutorDoUsuario != null)
                    {
                        venda.TutorId = tutorDoUsuario.TutorId;
                    }
                }

                decimal totalCalculado = 0;

                foreach (var item in venda.Itens)
                {
                    // Lógica para Produtos
                    if (item.ProdutoId.HasValue)
                    {
                        var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                        if (produto == null) return BadRequest($"? Produto {item.ProdutoId} não encontrado.");

                        if (produto.QuantidadeEstoque < item.Quantidade)
                            return BadRequest($"? Estoque insuficiente para o produto: {produto.Nome}. Disponível: {produto.QuantidadeEstoque}");

                        // BAIXA NO ESTOQUE
                        produto.QuantidadeEstoque -= item.Quantidade;
                        item.PrecoUnitario = produto.Preco; // Garante preço atual
                        totalCalculado += (produto.Preco * item.Quantidade);
                    }
                    // Lógica para Serviços
                    else if (item.ServicoId.HasValue)
                    {
                        var servico = await _context.Servicos.FindAsync(item.ServicoId);
                        if (servico == null) return BadRequest($"? Serviço {item.ServicoId} não encontrado.");

                        if (!servico.Ativo)
                            return BadRequest($"? O serviço {servico.Nome} não está mais disponível.");

                        item.PrecoUnitario = servico.Preco;
                        totalCalculado += (servico.Preco * item.Quantidade);
                    }
                }

                venda.ValorTotal = totalCalculado;

                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Recarregar com relacionamentos para retorno
                await _context.Entry(venda)
                    .Collection(v => v.Itens)
                    .Query()
                    .Include(i => i.Produto)
                    .Include(i => i.Servico)
                    .LoadAsync();

                if (venda.TutorId.HasValue)
                {
                    await _context.Entry(venda)
                        .Reference(v => v.Tutor)
                        .LoadAsync();
                }

                var vendaDto = _mapper.Map<VendaDto>(venda);
                return CreatedAtAction(nameof(GetVenda), new { id = venda.VendaId }, vendaDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao processar venda: {ex.Message}");
            }
        }
    }
}