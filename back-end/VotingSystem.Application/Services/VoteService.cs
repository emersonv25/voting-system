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
        private readonly IParticipantRepository _participantRepository;

        public VoteService(IVoteRepository voteRepository, IParticipantRepository participantRepository)
        {
            _voteRepository = voteRepository;
            _participantRepository = participantRepository;
        }

        public async Task RegisterVoteAsync(Guid participantId)
        {
            var participant = await _participantRepository.GetByIdAsync(participantId);
            if (participant == null)
                throw new ArgumentException("Participante inválido.");

            var vote = new Vote { ParticipantId = participantId };
            await _voteRepository.AddAsync(vote);
        }

        public async Task<int> GetTotalVotesAsync()
        {
            return await _voteRepository.GetTotalVotesAsync();
        }

        public async Task<IEnumerable<VotesPerHourDto>> GetVotesPerHourAsync()
        {
            return await _voteRepository.GetVotesPerHourAsync();
        }

        public async Task<GetTotalByParticipantDto> GetTotalVotesByParticipantAsync(Guid participantId)
        {
            var total = await _voteRepository.GetTotalVotesByParticipantAsync(participantId);
            return new GetTotalByParticipantDto
            {
                ParticipantId = participantId,
                TotalVotes = total
            };
        }

        public async Task<GetStatsDto> GetStatsAsync()
        {
            var totalVotes = await _voteRepository.GetTotalVotesAsync();
            var votesPerHour = await _voteRepository.GetVotesPerHourAsync();
            return new GetStatsDto
            {
                TotalVotes = totalVotes,
                VotesPerHour = votesPerHour.ToList()
            };
        }
    }
}
