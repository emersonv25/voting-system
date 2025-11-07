using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Interfaces;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;

namespace VotingSystem.Application.Services
{
    public class ResultService : IResultService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IVoteRepository _voteRepository;

        public ResultService(IParticipantRepository participantRepository, IVoteRepository voteRepository)
        {
            _participantRepository = participantRepository;
            _voteRepository = voteRepository;
        }
        public async Task<ResultDto> GetResultAsync()
        {
            var participants = await _participantRepository.GetAllAsync();

            var participantDtos = new List<ParticipantDto>();
            int totalVotes = 0;
            var votesByParticipant = new Dictionary<Guid, int>();


            foreach (var participant in participants)
            {
                var votes = await _voteRepository.GetTotalVotesByParticipantAsync(participant.Id);
                votesByParticipant[participant.Id] = votes;
                totalVotes += votes;
            }

            foreach (var participant in participants)
            {
                var votes = votesByParticipant[participant.Id];
                int percentage = totalVotes > 0 ? (int)Math.Round((votes * 100.0) / totalVotes) : 0;

                participantDtos.Add(new ParticipantDto
                {
                    Id = participant.Id,
                    Name = participant.Name,
                    PhotoUrl = participant.PhotoUrl,
                    Votes = votes,
                    PercentageOfVotes = percentage
                });
            }

            return new ResultDto
            {
                Participants = participantDtos,
                TotalVotes = totalVotes
            };
        }
    }
}
