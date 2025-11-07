using System.Collections.Generic;
using VotingSystem.Domain.DTOs;

namespace VotingSystem.Application.DTOs
{
    public class StatsDto
    {
        public int TotalVotes { get; set; }
        public List<VotesPerHourDto> VotesPerHour { get; set; } = new();
    }
}