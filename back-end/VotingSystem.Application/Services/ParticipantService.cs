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
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<IEnumerable<ParticipantDto>> GetAllActiveAsync()
        {
            var participants = await _participantRepository.GetAllAsync();

            return participants.Select(p => new ParticipantDto
            {
                Id = p.Id,
                Name = p.Name,
                PhotoUrl = p.PhotoUrl
            });
        }

        public async Task<ParticipantDto?> GetByIdAsync(Guid id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null) return null;

            return new ParticipantDto
            {
                Id = participant.Id,
                Name = participant.Name,
                PhotoUrl = participant.PhotoUrl
            };
        }
    }
}
