using VotingSystem.Domain.DTOs;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Domain.Interfaces
{
    public interface IVoteRepository
    {
        Task AddAsync(Vote vote);
        Task<int> GetTotalVotesAsync();
        Task<int> GetTotalVotesByParticipantAsync(Guid participantId);
        Task<IEnumerable<VotesPerHourDto>> GetVotesPerHourAsync();
    }
}
