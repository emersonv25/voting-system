using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Interfaces;

namespace VotingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantsController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Retorna todos os participantes ativos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetAll()
        {
            var response = await _participantService.GetAllActiveAsync();

            return Ok(response);
        }

        /// <summary>
        /// Retorna um participante específico pelo ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ParticipantDto>> GetById(Guid id)
        {
            var response = await _participantService.GetByIdAsync(id);
            if (response == null)
                return NotFound(new { message = "Participante não encontrado." });

            return Ok(response);
        }
    }
}
