using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.Entities;
using VotingSystem.Infra.Data.Repositories;
using VotingSystem.Data;
using Xunit;

namespace VotingSystem.Tests.Infra.Data.Repositories
{
    public class ParticipantRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly ParticipantRepository _participantRepository;

        public ParticipantRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            _participantRepository = new ParticipantRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveParticipants()
        {
            // Arrange
            var active = new Participant { Id = Guid.NewGuid(), Name = "Active", IsActive = true };
            var inactive = new Participant { Id = Guid.NewGuid(), Name = "Inactive", IsActive = false };
            await _dbContext.Participants.AddRangeAsync(active, inactive);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = (await _participantRepository.GetAllAsync()).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Active");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnActiveParticipant_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var participant = new Participant { Id = id, Name = "Active", IsActive = true };
            await _dbContext.Participants.AddAsync(participant);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _participantRepository.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Active");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenParticipantIsInactive()
        {
            // Arrange
            var id = Guid.NewGuid();
            var participant = new Participant { Id = id, Name = "Inactive", IsActive = false };
            await _dbContext.Participants.AddAsync(participant);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _participantRepository.GetByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }
    }
}
