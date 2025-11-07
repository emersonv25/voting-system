using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Data;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;

namespace VotingSystem.Infra.Data.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDbContext _context;

        public ParticipantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            return await _context.Participants
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Participant?> GetByIdAsync(Guid id)
        {
            return await _context.Participants
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
    }

}
