using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.DTOs;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;


namespace VotingSystem.Data.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalVotesAsync()
        {
            return await _context.Votes.CountAsync();
        }

        public async Task<int> GetTotalVotesByParticipantAsync(Guid participantId)
        {
            return await _context.Votes
                .CountAsync(v => v.ParticipantId == participantId);
        }

        public async Task<IEnumerable<VotesPerHourDto>> GetVotesPerHourAsync()
        {
            return await _context.Votes
                .GroupBy(v => new DateTime(v.CreatedAt.Year, v.CreatedAt.Month, v.CreatedAt.Day, v.CreatedAt.Hour, 0, 0))
                .Select(g => new VotesPerHourDto
                {
                    Hour = g.Key,
                    Total = g.Count()
                })
                .OrderBy(x => x.Hour)
                .ToListAsync();
        }
    }
}
