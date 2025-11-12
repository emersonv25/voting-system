using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Interfaces;
using VotingSystem.Domain.DTOs;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;

namespace VotingSystem.Application.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<VoteService> _logger;

        public VoteService(IVoteRepository voteRepository, 
            IRabbitMqService rabbitMqService, ILogger<VoteService> logger)
        {
            _voteRepository = voteRepository;
            _rabbitMqService = rabbitMqService;
            _logger = logger;

        }

        public async Task RegisterVoteAsync(Guid participantId)
        {
            _rabbitMqService.Publish("vote_queue", new { ParticipantId = participantId, Timestamp = DateTime.UtcNow });
            _logger.LogInformation("Voto registrado para o participante {ParticipantId}", participantId);
            await Task.CompletedTask;
        }


        public async Task<StatsDto> GetStatsAsync()
        {
            var totalVotes = await _voteRepository.GetTotalVotesAsync();
            var votesPerHour = await _voteRepository.GetVotesPerHourAsync();
            return new StatsDto
            {
                TotalVotes = totalVotes,
                VotesPerHour = votesPerHour.ToList()
            };
        }
    }
}
