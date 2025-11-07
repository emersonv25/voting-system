using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Interfaces;

namespace VotingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _participantService;

        public ResultsController(IResultService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Resultado da votação com a lista de participantes, votos e porcentagens.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ResultDto>> GetResultAsync()
        {
            var response = await _participantService.GetResultAsync();

            return Ok(response);
        }

    }
}
