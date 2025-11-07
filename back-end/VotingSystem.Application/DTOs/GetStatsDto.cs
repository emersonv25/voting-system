using System.Collections.Generic;
using VotingSystem.Domain.DTOs;

namespace VotingSystem.Application.DTOs
{
    public class GetStatsDto
    {
        public int TotalVotes { get; set; }
        public List<VotesPerHourDto> VotesPerHour { get; set; } = new();
    }
}