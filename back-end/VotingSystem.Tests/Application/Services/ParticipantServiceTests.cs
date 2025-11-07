using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Services;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;
using Xunit;

namespace VotingSystem.Tests.Application.Services
{
    public class ParticipantServiceTests
    {
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly ParticipantService _participantService;

        public ParticipantServiceTests()
        {
            _mockParticipantRepository = new Mock<IParticipantRepository>();
            _participantService = new ParticipantService(_mockParticipantRepository.Object);
        }

        [Fact]
        public async Task GetAllActiveAsync_ShouldReturnAllParticipants()
        {
            // Arrange
            var participants = new List<Participant>
            {
                new Participant { Id = Guid.NewGuid(), Name = "Alice", PhotoUrl = "url1" },
                new Participant { Id = Guid.NewGuid(), Name = "Bob", PhotoUrl = "url2" }
            };
            _mockParticipantRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(participants);

            // Act
            var result = (await _participantService.GetAllActiveAsync()).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Alice");
            result[1].Name.Should().Be("Bob");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnParticipant_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var participant = new Participant { Id = id, Name = "Alice", PhotoUrl = "url1" };
            _mockParticipantRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(participant);

            // Act
            var result = await _participantService.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Alice");
            result.PhotoUrl.Should().Be("url1");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockParticipantRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Participant?)null);

            // Act
            var result = await _participantService.GetByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }
    }
}
