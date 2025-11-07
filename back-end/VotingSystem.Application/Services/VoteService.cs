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

        public VoteService(IVoteRepository voteRepository, 
            IRabbitMqService rabbitMqService)
        {
            _voteRepository = voteRepository;
            _rabbitMqService = rabbitMqService;

        }

        public async Task RegisterVoteAsync(Guid participantId)
        {
            _rabbitMqService.Publish("vote_queue", new { ParticipantId = participantId, Timestamp = DateTime.UtcNow });
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
