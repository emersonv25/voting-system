using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Interfaces;

namespace VotingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VotesController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        /// <summary>
        /// Registra um voto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostVote(Guid participantId)
        {
            await _voteService.RegisterVoteAsync(participantId);
            return Ok(new { message = "Voto computado com sucesso!" });
        }

        /// <summary>
        /// Estatísticas gerais de votação
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<StatsDto>> GetStats()
        {
            var response = await _voteService.GetStatsAsync();

            return Ok(response);
        }
    }

}
