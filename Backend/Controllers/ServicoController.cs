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
    public class ServicoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ServicoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos os servi�os
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicoDto>>> GetServicos()
        {
            var servicos = await _context.Servicos
                .Include(s => s.ServicoFuncionarios)
                    .ThenInclude(sf => sf.Funcionario)
                .Include(s => s.FuncionarioResponsavel)
                .AsNoTracking()
                .ToListAsync();

            var servicosDto = servicos.Select(s =>
            {
                var dto = _mapper.Map<ServicoDto>(s);
                
                // NOVO: Buscar funcion�rios aptos baseado nos cargos
                if (!string.IsNullOrEmpty(s.CargosResponsaveis))
                {
                    var cargos = s.CargosResponsaveis.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(c => c.Trim())
                                                   .ToList();
                    
                    var funcionariosAptos = _context.Funcionarios
                        .Where(f => f.Ativo && cargos.Contains(f.Cargo))
                        .ToList();
                        
                    dto.FuncionariosAptos = _mapper.Map<List<FuncionarioSimplificadoDto>>(funcionariosAptos);
                }
                
                return dto;
            }).ToList();

            return Ok(servicosDto);
        }

        /// <summary>
        /// Lista apenas servi�os ativos
        /// </summary>
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<ServicoDto>>> GetServicosAtivos()
        {
            var servicos = await _context.Servicos
                .Where(s => s.Ativo)
                .Include(s => s.ServicoFuncionarios)
                    .ThenInclude(sf => sf.Funcionario)
                .Include(s => s.FuncionarioResponsavel)
                .AsNoTracking()
                .ToListAsync();

            var servicosDto = servicos.Select(s =>
            {
                var dto = _mapper.Map<ServicoDto>(s);
                
                // NOVO: Buscar funcion�rios aptos baseado nos cargos
                if (!string.IsNullOrEmpty(s.CargosResponsaveis))
                {
                    var cargos = s.CargosResponsaveis.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(c => c.Trim())
                                                   .ToList();
                    
                    var funcionariosAptos = _context.Funcionarios
                        .Where(f => f.Ativo && cargos.Contains(f.Cargo))
                        .ToList();
                        
                    dto.FuncionariosAptos = _mapper.Map<List<FuncionarioSimplificadoDto>>(funcionariosAptos);
                }
                
                return dto;
            }).ToList();

            return Ok(servicosDto);
        }

        /// <summary>
        /// NOVO: Busca funcion�rios aptos para um servi�o espec�fico baseado nos cargos
        /// </summary>
        [HttpGet("{id}/funcionarios-aptos")]
        public async Task<ActionResult<IEnumerable<FuncionarioSimplificadoDto>>> GetFuncionariosAptos(int id)
        {
            var servico = await _context.Servicos
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ServicoId == id);

            if (servico == null)
            {
                return NotFound("Servi�o n�o encontrado.");
            }

            List<Funcionario> funcionariosAptos = new List<Funcionario>();

            if (!string.IsNullOrEmpty(servico.CargosResponsaveis))
            {
                var cargos = servico.CargosResponsaveis.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(c => c.Trim())
                                                     .ToList();
                
                funcionariosAptos = await _context.Funcionarios
                    .Where(f => f.Ativo && cargos.Contains(f.Cargo))
                    .ToListAsync();
            }

            return Ok(_mapper.Map<IEnumerable<FuncionarioSimplificadoDto>>(funcionariosAptos));
        }

        /// <summary>
        /// NOVO: Lista todos os cargos dispon�veis
        /// </summary>
        [HttpGet("cargos-disponiveis")]
        public async Task<ActionResult<IEnumerable<string>>> GetCargosDisponiveis()
        {
            var cargos = await _context.Funcionarios
                .Where(f => f.Ativo)
                .Select(f => f.Cargo)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(cargos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicoDto>> GetServico(int id)
        {
            var servico = await _context.Servicos
                .Include(s => s.ServicoFuncionarios)
                    .ThenInclude(sf => sf.Funcionario)
                .Include(s => s.FuncionarioResponsavel)
                .FirstOrDefaultAsync(s => s.ServicoId == id);

            if (servico == null)
            {
                return NotFound("Servi�o n�o encontrado.");
            }

            var dto = _mapper.Map<ServicoDto>(servico);
            
            // NOVO: Buscar funcion�rios aptos baseado nos cargos
            if (!string.IsNullOrEmpty(servico.CargosResponsaveis))
            {
                var cargos = servico.CargosResponsaveis.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(c => c.Trim())
                                                     .ToList();
                
                var funcionariosAptos = await _context.Funcionarios
                    .Where(f => f.Ativo && cargos.Contains(f.Cargo))
                    .ToListAsync();
                    
                dto.FuncionariosAptos = _mapper.Map<List<FuncionarioSimplificadoDto>>(funcionariosAptos);
            }

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ServicoDto>> CreateServico([FromBody] CreateServicoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var servico = _mapper.Map<Servico>(dto);

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            var servicoDto = _mapper.Map<ServicoDto>(servico);
            return CreatedAtAction(nameof(GetServico), new { id = servico.ServicoId }, servicoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServico(int id, [FromBody] UpdateServicoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var servico = await _context.Servicos
                .Include(s => s.ServicoFuncionarios)
                .FirstOrDefaultAsync(s => s.ServicoId == id);

            if (servico == null)
            {
                return NotFound("Servi�o n�o encontrado.");
            }

            _mapper.Map(dto, servico);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Servicos.Any(e => e.ServicoId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {
            try
            {
                var servico = await _context.Servicos
                    .Include(s => s.Agendamentos)
                    .Include(s => s.ItemVendas)
                    .Include(s => s.ServicoFuncionarios)
                    .FirstOrDefaultAsync(s => s.ServicoId == id);

                if (servico == null)
                {
                    return NotFound("Serviço não encontrado.");
                }

                // Verificar se há vendas associadas (não pode deletar)
                if (servico.ItemVendas != null && servico.ItemVendas.Any())
                {
                    return BadRequest($"Não é possível excluir o serviço pois existem {servico.ItemVendas.Count} venda(s) associada(s).");
                }

                // Verificar se há agendamentos associados (não pode deletar)
                if (servico.Agendamentos != null && servico.Agendamentos.Any())
                {
                    return BadRequest($"Não é possível excluir o serviço pois existem {servico.Agendamentos.Count} agendamento(s) associado(s).");
                }

                // Deletar relacionamentos ServicoFuncionario (cascade já configurado, mas garantindo)
                if (servico.ServicoFuncionarios != null && servico.ServicoFuncionarios.Any())
                {
                    _context.ServicoFuncionarios.RemoveRange(servico.ServicoFuncionarios);
                }

                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao deletar serviço: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
