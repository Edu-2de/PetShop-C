using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;
using SIGA_PET.Enums;

namespace SIGA_PET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AgendamentoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Agendamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentos()
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                        .ThenInclude(animal => animal.Tutor)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Erro ao carregar agendamentos", 
                    details = ex.Message,
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        // GET: api/Agendamento/tutor/5
        [HttpGet("tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByTutor(int tutorId)
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                        .ThenInclude(animal => animal.Tutor)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.Animal != null && a.Animal.TutorId == tutorId)
                    .OrderByDescending(a => a.DataHora)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByUsuario(int usuarioId)
        {
            try
            {
                var tutor = await _context.Tutores.FirstOrDefaultAsync(t => t.UsuarioId == usuarioId);

                if (tutor == null)
                {
                    // If a user is not a tutor, they won't have appointments. Return an empty list.
                    return Ok(new List<AgendamentoDto>());
                }

                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.Animal.TutorId == tutor.TutorId)
                    .OrderByDescending(a => a.DataHora)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar agendamentos: {ex.Message}");
            }
        }

        // GET: api/Agendamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgendamentoDto>> GetAgendamento(int id)
        {
            try
            {
                var agendamento = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .FirstOrDefaultAsync(a => a.AgendamentoId == id);

                if (agendamento == null)
                {
                    return NotFound($"Agendamento com ID {id} n�o encontrado.");
                }

                var agendamentoDto = _mapper.Map<AgendamentoDto>(agendamento);
                return Ok(agendamentoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Agendamento/animal/5
        [HttpGet("animal/{animalId}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByAnimal(int animalId)
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.AnimalId == animalId)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Agendamento/data/2024-01-01
        [HttpGet("data/{data}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByData(DateTime data)
        {
            try
            {
                var dataInicio = data.Date;
                var dataFim = dataInicio.AddDays(1);

                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.DataHora >= dataInicio && a.DataHora < dataFim)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// <summary>
        /// Criar novo agendamento
        /// </summary>
        /// <param name="createAgendamentoDto">Dados do agendamento</param>
        /// <remarks>
        /// Cria um novo agendamento no sistema.
        /// 
        /// **Validações aplicadas:**
        /// - Data e hora não podem ser no passado
        /// - Funcionário deve estar disponível no horário
        /// - Animal deve existir e estar ativo
        /// - Serviço deve estar ativo
        /// 
        /// **Exemplo de requisição:**
        /// ```json
        /// {
        ///   "animalId": 1,
        ///   "servicoId": 2,
        ///   "funcionarioId": 1,
        ///   "dataHora": "2024-12-15T14:30:00",
        ///   "observacoes": "Primeira consulta do Rex"
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Agendamento criado com sucesso</response>
        /// <response code="400">Dados inválidos ou conflito de horário</response>
        /// <response code="404">Animal, serviço ou funcionário não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(AgendamentoDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<AgendamentoDto>> CreateAgendamento([FromBody] CreateAgendamentoDto createAgendamentoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // VALIDAÇÃO: Verificar se a data não é no passado
                var agora = DateTime.Now;
                if (createAgendamentoDto.DataHora <= agora)
                {
                    return BadRequest($"❌ Não é possível agendar para uma data/hora que já passou. Data/hora atual: {agora:dd/MM/yyyy HH:mm}");
                }

                // VALIDAÇÃO: Verificar se a data não é muito no futuro (opcional - máximo 6 meses)
                if (createAgendamentoDto.DataHora > agora.AddMonths(6))
                {
                    return BadRequest("❌ Não é possível agendar com mais de 6 meses de antecedência.");
                }

                // VALIDAÇÃO: Verificar horário de funcionamento (8h às 18h INCLUSIVE)
                var horaAgendamento = createAgendamentoDto.DataHora.TimeOfDay;
                var horaAbertura = new TimeSpan(8, 0, 0);  // 8:00
                var horaFechamento = new TimeSpan(18, 0, 0); // 18:00

                // Corrigido: permite até 18:00 (antes do fechamento às 18:01)
                if (horaAgendamento < horaAbertura || horaAgendamento > horaFechamento)
                {
                    return BadRequest($"❌ Agendamentos só podem ser feitos entre 8:00 e 18:00. Hora recebida: {createAgendamentoDto.DataHora:HH:mm}");
                }

                // VALIDA��O: Verificar se � domingo (opcional - n�o atendemos domingos)
                if (createAgendamentoDto.DataHora.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest("? N�o atendemos aos domingos. Escolha outro dia da semana.");
                }

                // Verificar se animal existe
                var animal = await _context.Animais.FindAsync(createAgendamentoDto.AnimalId);
                if (animal == null)
                    return NotFound($"Animal com ID {createAgendamentoDto.AnimalId} n�o encontrado.");

                // Verificar se servi�o existe e est� ativo
                var servico = await _context.Servicos
                    .Include(s => s.ServicoFuncionarios)
                        .ThenInclude(sf => sf.Funcionario)
                    .FirstOrDefaultAsync(s => s.ServicoId == createAgendamentoDto.ServicoId);

                if (servico == null)
                    return NotFound($"Servi�o com ID {createAgendamentoDto.ServicoId} n�o encontrado.");

                if (!servico.Ativo)
                    return BadRequest("? Este servi�o n�o est� mais dispon�vel.");

                var agendamento = _mapper.Map<Agendamento>(createAgendamentoDto);

                // Auto-atribuir funcion�rio se n�o especificado
                if (!createAgendamentoDto.FuncionarioId.HasValue)
                {
                    var funcionariosAptos = servico.ServicoFuncionarios
                        .Where(sf => sf.Funcionario.Ativo)
                        .Select(sf => sf.Funcionario)
                        .ToList();

                    if (!funcionariosAptos.Any())
                        return BadRequest("? Este servi�o n�o possui funcion�rios dispon�veis no momento.");

                    // Verificar qual funcion�rio tem menos agendamentos no dia
                    var dataAgendamento = createAgendamentoDto.DataHora.Date;
                    var funcionarioMenosOcupado = funcionariosAptos
                        .OrderBy(f => _context.Agendamentos.Count(a => 
                            a.FuncionarioId == f.FuncionarioId && 
                            a.DataHora.Date == dataAgendamento))
                        .First();

                    agendamento.FuncionarioId = funcionarioMenosOcupado.FuncionarioId;
                }
                else
                {
                    // Verificar se funcion�rio especificado existe e est� ativo
                    var funcionario = await _context.Funcionarios.FindAsync(createAgendamentoDto.FuncionarioId);
                    if (funcionario == null)
                        return NotFound($"Funcion�rio com ID {createAgendamentoDto.FuncionarioId} n�o encontrado.");

                    if (!funcionario.Ativo)
                        return BadRequest("? Este funcion�rio n�o est� dispon�vel no momento.");

                    // Verificar se funcion�rio � apto para o servi�o
                    var funcionarioApto = servico.ServicoFuncionarios
                        .Any(sf => sf.FuncionarioId == createAgendamentoDto.FuncionarioId.Value);

                    if (!funcionarioApto)
                        return BadRequest($"? O funcion�rio {funcionario.Nome} n�o est� habilitado para realizar este servi�o.");
                }

                // Verificar conflitos de hor�rio
                var dataInicio = agendamento.DataHora;
                var dataFim = agendamento.DataHora.AddMinutes(servico.DuracaoMinutos);

                var conflito = await _context.Agendamentos
                    .Where(a => a.FuncionarioId == agendamento.FuncionarioId &&
                               a.Status != "Cancelado" &&
                               ((a.DataHora <= dataInicio && a.DataHora.AddMinutes(a.Servico.DuracaoMinutos) > dataInicio) ||
                                (a.DataHora < dataFim && a.DataHora >= dataInicio)))
                    .Include(a => a.Servico)
                    .FirstOrDefaultAsync();

                if (conflito != null)
                {
                    var funcionarioNome = await _context.Funcionarios
                        .Where(f => f.FuncionarioId == agendamento.FuncionarioId)
                        .Select(f => f.Nome)
                        .FirstOrDefaultAsync();

                    return BadRequest($"? Conflito de hor�rio! {funcionarioNome} j� possui um agendamento de {conflito.DataHora:dd/MM/yyyy HH:mm} �s {conflito.DataHora.AddMinutes(conflito.Servico.DuracaoMinutos):HH:mm}.");
                }

                // Definir status padr�o
                if (string.IsNullOrEmpty(agendamento.Status))
                {
                    agendamento.Status = "Pendente";
                }

                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();

                // Recarregar com relacionamentos para retorno
                await _context.Entry(agendamento)
                    .Reference(a => a.Animal)
                    .LoadAsync();
                await _context.Entry(agendamento.Animal)
                    .Reference(a => a.Tutor)
                    .LoadAsync();
                await _context.Entry(agendamento)
                    .Reference(a => a.Servico)
                    .LoadAsync();
                await _context.Entry(agendamento)
                    .Reference(a => a.Funcionario)
                    .LoadAsync();

                var agendamentoDto = _mapper.Map<AgendamentoDto>(agendamento);
                return CreatedAtAction(nameof(GetAgendamento), new { id = agendamento.AgendamentoId }, agendamentoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// ?? Atualizar agendamento existente
        /// </summary>
        /// <param name="id">ID do agendamento a ser atualizado</param>
        /// <param name="updateAgendamentoDto">Dados atualizados</param>
        /// <remarks>
        /// Atualiza um agendamento existente.
        /// 
        /// **Valida��es aplicadas:**
        /// - Data e hora n�o podem ser no passado
        /// - Funcion�rio deve estar dispon�vel no hor�rio
        /// - N�o pode alterar agendamentos j� conclu�dos
        /// 
        /// **Exemplo de requisi��o:**
        /// ```json
        /// {
        ///   "animalId": 1,
        ///   "servicoId": 2,
        ///   "funcionarioId": 1,
        ///   "dataHora": "2024-12-20T15:00:00",
        ///   "status": "Confirmado",
        ///   "observacoes": "Reagendamento - cliente pediu mudan�a de hor�rio"
        /// }
        /// ```
        /// </remarks>
        /// <response code="204">Agendamento atualizado com sucesso</response>
        /// <response code="400">Dados inv�lidos ou conflito de hor�rio</response>
        /// <response code="404">Agendamento n�o encontrado</response>
        /// <response code="409">Agendamento n�o pode ser alterado (j� conclu�do/cancelado)</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> UpdateAgendamento(int id, [FromBody] UpdateAgendamentoDto updateAgendamentoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var agendamento = await _context.Agendamentos
                    .Include(a => a.Servico)
                    .FirstOrDefaultAsync(a => a.AgendamentoId == id);

                if (agendamento == null)
                    return NotFound($"Agendamento com ID {id} n�o encontrado.");

                // Verificar se pode ser alterado
                if (agendamento.Status == "Concluido")
                    return Conflict("? N�o � poss�vel alterar um agendamento j� conclu�do.");

                // VALIDA��O: Verificar se a nova data n�o � no passado
                var agora = DateTime.Now;
                if (updateAgendamentoDto.DataHora <= agora && updateAgendamentoDto.DataHora != agendamento.DataHora)
                {
                    return BadRequest($"? N�o � poss�vel reagendar para uma data/hora que j� passou. Data/hora atual: {agora:dd/MM/yyyy HH:mm}");
                }

                // VALIDAÇÃO: Verificar horário de funcionamento se a data mudou
                if (updateAgendamentoDto.DataHora != agendamento.DataHora)
                {
                    var horaAgendamento = updateAgendamentoDto.DataHora.TimeOfDay;
                    var horaAbertura = new TimeSpan(8, 0, 0);
                    var horaFechamento = new TimeSpan(18, 0, 0);

                    if (horaAgendamento < horaAbertura || horaAgendamento > horaFechamento)
                    {
                        return BadRequest("❌ Agendamentos só podem ser feitos entre 8:00 e 18:00.");
                    }

                    // Verificar se é domingo
                    if (updateAgendamentoDto.DataHora.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return BadRequest("❌ Não atendemos aos domingos. Escolha outro dia da semana.");
                    }
                }

                // Verificar conflitos se hor�rio ou funcion�rio mudaram
                if (updateAgendamentoDto.DataHora != agendamento.DataHora || 
                    updateAgendamentoDto.FuncionarioId != agendamento.FuncionarioId)
                {
                    var servico = await _context.Servicos.FindAsync(updateAgendamentoDto.ServicoId);
                    if (servico == null)
                        return BadRequest("? Servi�o n�o encontrado.");

                    var dataInicio = updateAgendamentoDto.DataHora;
                    var dataFim = updateAgendamentoDto.DataHora.AddMinutes(servico.DuracaoMinutos);

                    var conflito = await _context.Agendamentos
                        .Where(a => a.AgendamentoId != id && // Excluir o pr�prio agendamento
                                   a.FuncionarioId == updateAgendamentoDto.FuncionarioId &&
                                   a.Status != "Cancelado" &&
                                   ((a.DataHora <= dataInicio && a.DataHora.AddMinutes(a.Servico.DuracaoMinutos) > dataInicio) ||
                                    (a.DataHora < dataFim && a.DataHora >= dataInicio)))
                        .Include(a => a.Servico)
                        .FirstOrDefaultAsync();

                    if (conflito != null)
                    {
                        var funcionarioNome = await _context.Funcionarios
                            .Where(f => f.FuncionarioId == updateAgendamentoDto.FuncionarioId)
                            .Select(f => f.Nome)
                            .FirstOrDefaultAsync();

                        return BadRequest($"? Conflito de hor�rio! {funcionarioNome} j� possui um agendamento de {conflito.DataHora:dd/MM/yyyy HH:mm} �s {conflito.DataHora.AddMinutes(conflito.Servico.DuracaoMinutos):HH:mm}.");
                    }
                }

                _mapper.Map(updateAgendamentoDto, agendamento);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // DELETE: api/Agendamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgendamento(int id)
        {
            try
            {
                var agendamento = await _context.Agendamentos.FindAsync(id);
                if (agendamento == null) return NotFound($"Agendamento com ID {id} n�o encontrado.");

                _context.Agendamentos.Remove(agendamento);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao deletar: O agendamento pode ter refer�ncias que impedem a exclus�o. Detalhes: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // NOVO ENDPOINT: Verificar disponibilidade
        [HttpGet("disponibilidade")]
        public async Task<ActionResult<object>> VerificarDisponibilidade([FromQuery] int servicoId, [FromQuery] DateTime dataHora, [FromQuery] int? funcionarioId = null)
        {
            try
            {
                var servico = await _context.Servicos
                    .Include(s => s.FuncionarioResponsavel)
                    .FirstOrDefaultAsync(s => s.ServicoId == servicoId);

                if (servico == null)
                    return BadRequest("Servi�o n�o encontrado.");

                var funcionarioResponsavel = funcionarioId ?? servico.FuncionarioResponsavelId;
                
                var conflitos = new List<string>();

                // Verificar conflito de funcion�rio
                if (funcionarioResponsavel.HasValue)
                {
                    var conflitoFunc = await _context.Agendamentos
                        .AnyAsync(a => a.FuncionarioId == funcionarioResponsavel
                                       && a.DataHora == dataHora
                                       && a.Status != "Cancelado");
                    
                    if (conflitoFunc)
                        conflitos.Add("Funcion�rio j� possui agendamento neste hor�rio");
                }

                // Verificar conflito de servi�o (se tem funcion�rio respons�vel espec�fico)
                if (servico.FuncionarioResponsavelId.HasValue)
                {
                    var conflitoServ = await _context.Agendamentos
                        .AnyAsync(a => a.ServicoId == servicoId
                                       && a.DataHora == dataHora
                                       && a.Status != "Cancelado");
                    
                    if (conflitoServ)
                        conflitos.Add("Servi�o j� agendado para este hor�rio");
                }

                return Ok(new {
                    disponivel = !conflitos.Any(),
                    conflitos = conflitos,
                    funcionarioResponsavel = servico.FuncionarioResponsavel?.Nome,
                    funcionarioResponsavelId = servico.FuncionarioResponsavelId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// ?? Criar agendamento completo (cria tutor e animal automaticamente)
        /// </summary>
        /// <param name="createCompletoDto">Dados completos do agendamento</param>
        /// <remarks>
        /// Cria um agendamento completo no sistema, incluindo:
        /// - Tutor (se n�o existir baseado no email/telefone)
        /// - Animal (sempre novo)
        /// - Agendamento
        /// 
        /// **Ideal para:** Agendamentos r�pidos onde o cliente n�o est� cadastrado
        /// 
        /// **Valida��es aplicadas:**
        /// - Verifica se tutor j� existe (por email ou telefone)
        /// - Cria tutor automaticamente se n�o existir
        /// - Cria animal sempre novo (vinculado ao tutor)
        /// - Aplica todas as valida��es de agendamento
        /// 
        /// **Exemplo de requisi��o:**
        /// ```json
        /// {
        ///   "servicoId": 1,
        ///   "dataHora": "2024-12-20T14:30:00",
        ///   "nomeTutor": "Maria Silva",
        ///   "emailTutor": "maria@email.com",
        ///   "telefoneTutor": "(11) 98765-4321",
        ///   "enderecoTutor": "Rua Exemplo, 123",
        ///   "nomeAnimal": "Rex",
        ///   "especieAnimal": "C�o",
        ///   "racaAnimal": "Labrador",
        ///   "sexoAnimal": "Macho",
        ///   "pelagemAnimal": "Curta",
        ///   "observacoes": "Primeira consulta"
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Agendamento criado com sucesso (tutor e animal criados)</response>
        /// <response code="400">Dados inv�lidos ou conflito de hor�rio</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("completo")]
        [ProducesResponseType(typeof(AgendamentoDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<AgendamentoDto>> CreateAgendamentoCompleto([FromBody] CreateAgendamentoCompletoDto createCompletoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // VALIDA��ES DE DATA/HORA (mesmas do endpoint principal)
                var agora = DateTime.Now;
                if (createCompletoDto.DataHora <= agora)
                {
                    return BadRequest($"? N�o � poss�vel agendar para uma data/hora que j� passou. Data/hora atual: {agora:dd/MM/yyyy HH:mm}");
                }

                if (createCompletoDto.DataHora > agora.AddMonths(6))
                {
                    return BadRequest("❌ Não é possível agendar com mais de 6 meses de antecedência.");
                }

                var horaAgendamento = createCompletoDto.DataHora.TimeOfDay;
                var horaAbertura = new TimeSpan(8, 0, 0);
                var horaFechamento = new TimeSpan(18, 0, 0);

                if (horaAgendamento < horaAbertura || horaAgendamento > horaFechamento)
                {
                    return BadRequest("❌ Agendamentos só podem ser feitos entre 8:00 e 18:00.");
                }

                if (createCompletoDto.DataHora.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest("❌ Não atendemos aos domingos. Escolha outro dia da semana.");
                }

                // Verificar se servi�o existe
                var servico = await _context.Servicos
                    .Include(s => s.ServicoFuncionarios)
                        .ThenInclude(sf => sf.Funcionario)
                    .FirstOrDefaultAsync(s => s.ServicoId == createCompletoDto.ServicoId);

                if (servico == null)
                    return NotFound($"Servi�o com ID {createCompletoDto.ServicoId} n�o encontrado.");

                if (!servico.Ativo)
                    return BadRequest("? Este servi�o n�o est� mais dispon�vel.");

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // 1. CRIAR OU ENCONTRAR TUTOR
                    Tutor tutor;
                    
                    // Verificar se tutor j� existe (por email ou telefone)
                    Tutor? tutorExistente = null;
                    
                    if (!string.IsNullOrEmpty(createCompletoDto.EmailTutor))
                    {
                        var usuarioExistente = await _context.Usuarios
                            .FirstOrDefaultAsync(u => u.Email == createCompletoDto.EmailTutor);
                        if (usuarioExistente != null)
                        {
                            tutorExistente = await _context.Tutores
                                .FirstOrDefaultAsync(t => t.UsuarioId == usuarioExistente.UsuarioId);
                        }
                    }

                    if (tutorExistente == null && !string.IsNullOrEmpty(createCompletoDto.TelefoneTutor))
                    {
                        tutorExistente = await _context.Tutores
                            .FirstOrDefaultAsync(t => t.Telefone == createCompletoDto.TelefoneTutor);
                    }

                    if (tutorExistente != null)
                    {
                        tutor = tutorExistente;
                    }
                    else
                    {
                        // Criar novo tutor
                        tutor = new Tutor
                        {
                            Nome = createCompletoDto.NomeTutor ?? "Cliente N�o Informado",
                            Telefone = createCompletoDto.TelefoneTutor ?? "",
                            Endereco = createCompletoDto.EnderecoTutor ?? "N�o informado",
                            DataCadastro = DateTime.Now,
                            UsuarioId = null // Tutor sem usu�rio de login
                        };

                        _context.Tutores.Add(tutor);
                        await _context.SaveChangesAsync();
                    }

                    // 2. CRIAR ANIMAL
                    var animal = new Animal
                    {
                        Nome = createCompletoDto.NomeAnimal,
                        Especie = createCompletoDto.EspecieAnimal,
                        Raca = createCompletoDto.RacaAnimal ?? "SRD",
                        Sexo = createCompletoDto.SexoAnimal,
                        DataNascimento = createCompletoDto.DataNascimentoAnimal,
                        Pelagem = createCompletoDto.PelagemAnimal,
                        Observacoes = createCompletoDto.ObservacoesAnimal,
                        TutorId = tutor.TutorId
                    };

                    _context.Animais.Add(animal);
                    await _context.SaveChangesAsync();

                    // 3. AUTO-ATRIBUIR FUNCION�RIO se n�o especificado
                    int? funcionarioId = createCompletoDto.FuncionarioId;
                    
                    if (!funcionarioId.HasValue)
                    {
                        var funcionariosAptos = servico.ServicoFuncionarios
                            .Where(sf => sf.Funcionario.Ativo)
                            .Select(sf => sf.Funcionario)
                            .ToList();

                        if (!funcionariosAptos.Any())
                            return BadRequest("? Este servi�o n�o possui funcion�rios dispon�veis no momento.");

                        // Escolher funcion�rio com menos agendamentos no dia
                        var dataAgendamento = createCompletoDto.DataHora.Date;
                        var funcionarioMenosOcupado = funcionariosAptos
                            .OrderBy(f => _context.Agendamentos.Count(a => 
                                a.FuncionarioId == f.FuncionarioId && 
                                a.DataHora.Date == dataAgendamento))
                            .First();

                        funcionarioId = funcionarioMenosOcupado.FuncionarioId;
                    }

                    // 4. VERIFICAR CONFLITOS DE HOR�RIO
                    var dataInicio = createCompletoDto.DataHora;
                    var dataFim = createCompletoDto.DataHora.AddMinutes(servico.DuracaoMinutos);

                    var conflito = await _context.Agendamentos
                        .Where(a => a.FuncionarioId == funcionarioId &&
                                   a.Status != "Cancelado" &&
                                   ((a.DataHora <= dataInicio && a.DataHora.AddMinutes(a.Servico.DuracaoMinutos) > dataInicio) ||
                                    (a.DataHora < dataFim && a.DataHora >= dataInicio)))
                        .Include(a => a.Servico)
                        .FirstOrDefaultAsync();

                    if (conflito != null)
                    {
                        var funcionarioNome = await _context.Funcionarios
                            .Where(f => f.FuncionarioId == funcionarioId)
                            .Select(f => f.Nome)
                            .FirstOrDefaultAsync();

                        return BadRequest($"? Conflito de hor�rio! {funcionarioNome} j� possui um agendamento de {conflito.DataHora:dd/MM/yyyy HH:mm} �s {conflito.DataHora.AddMinutes(conflito.Servico.DuracaoMinutos):HH:mm}.");
                    }

                    // 5. CRIAR AGENDAMENTO
                    var agendamento = new Agendamento
                    {
                        AnimalId = animal.AnimalId,
                        ServicoId = createCompletoDto.ServicoId,
                        FuncionarioId = funcionarioId,
                        DataHora = createCompletoDto.DataHora,
                        Status = createCompletoDto.Status,
                        Observacoes = createCompletoDto.Observacoes
                    };

                    _context.Agendamentos.Add(agendamento);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // Recarregar com relacionamentos
                    await _context.Entry(agendamento)
                        .Reference(a => a.Animal)
                        .LoadAsync();
                    await _context.Entry(agendamento.Animal)
                        .Reference(a => a.Tutor)
                        .LoadAsync();
                    await _context.Entry(agendamento)
                        .Reference(a => a.Servico)
                        .LoadAsync();
                    await _context.Entry(agendamento)
                        .Reference(a => a.Funcionario)
                        .LoadAsync();

                    var agendamentoDto = _mapper.Map<AgendamentoDto>(agendamento);
                    return CreatedAtAction(nameof(GetAgendamento), new { id = agendamento.AgendamentoId }, agendamentoDto);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Erro ao criar agendamento completo: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}