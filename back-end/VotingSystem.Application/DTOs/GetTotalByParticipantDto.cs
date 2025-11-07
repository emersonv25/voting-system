using System;

namespace VotingSystem.Application.DTOs
{
    public class GetTotalByParticipantDto
    {
        public Guid ParticipantId { get; set; }
        public int TotalVotes { get; set; }
    }
}