using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;
using VotingSystem.Domain.DTOs;

namespace VotingSystem.Application.Interfaces
{
    public interface IVoteService
    {
        Task RegisterVoteAsync(Guid participantId);
        Task<StatsDto> GetStatsAsync();
    }
}
