using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data.Repositories;
using VotingSystem.Data;
using VotingSystem.Domain.Entities;
using Xunit;

namespace VotingSystem.Tests.Infra.Data.Repositories
{
    public class VoteRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly VoteRepository _voteRepository;

        public VoteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            _voteRepository = new VoteRepository(_dbContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddVote()
        {
            // Arrange
            var vote = new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };

            // Act
            await _voteRepository.AddAsync(vote);

            // Assert
            var result = await _dbContext.Votes.FindAsync(vote.Id);
            result.Should().NotBeNull();
            result!.ParticipantId.Should().Be(vote.ParticipantId);
        }

        [Fact]
        public async Task GetTotalVotesAsync_ShouldReturnVotesCount()
        {
            // Arrange
            var votes = new[]
            {
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow }
            };
            await _dbContext.Votes.AddRangeAsync(votes);
            await _dbContext.SaveChangesAsync();

            // Act
            var count = await _voteRepository.GetTotalVotesAsync();

            // Assert
            count.Should().Be(2);
        }

        [Fact]
        public async Task GetTotalVotesByParticipantAsync_ShouldReturnVotesCountForParticipant()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            var votes = new[]
            {
                new Vote { Id = Guid.NewGuid(), ParticipantId = participantId, CreatedAt = DateTime.UtcNow },
                new Vote { Id = Guid.NewGuid(), ParticipantId = participantId, CreatedAt = DateTime.UtcNow },
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow }
            };
            await _dbContext.Votes.AddRangeAsync(votes);
            await _dbContext.SaveChangesAsync();

            // Act
            var count = await _voteRepository.GetTotalVotesByParticipantAsync(participantId);

            // Assert
            count.Should().Be(2);
        }

        [Fact]
        public async Task GetVotesPerHourAsync_ShouldReturnVotesGroupedByHour()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var votes = new[]
            {
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0) },
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = new DateTime(now.Year, now.Month, now.Day, 10, 30, 0) },
                new Vote { Id = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), CreatedAt = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0) }
            };
            await _dbContext.Votes.AddRangeAsync(votes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = (await _voteRepository.GetVotesPerHourAsync()).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Hour.Hour.Should().Be(10);
            result[0].Total.Should().Be(2);
            result[1].Hour.Hour.Should().Be(11);
            result[1].Total.Should().Be(1);
        }
    }
}
