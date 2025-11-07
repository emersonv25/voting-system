using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;
using VotingSystem.Application.Services;
using VotingSystem.Domain.DTOs;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;
using Xunit;

namespace VotingSystem.Tests.Application.Services
{
    public class VoteServiceTests
    {
        private readonly Mock<IVoteRepository> _mockVoteRepository;
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly VoteService _voteService;

        public VoteServiceTests()
        {
            _mockVoteRepository = new Mock<IVoteRepository>();
            _mockParticipantRepository = new Mock<IParticipantRepository>();
            _voteService = new VoteService(_mockVoteRepository.Object, _mockParticipantRepository.Object);
        }

        [Fact]
        public async Task RegisterVoteAsync_ShouldAddVote_WhenParticipantExists()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            var participant = new Participant { Id = participantId };
            _mockParticipantRepository.Setup(r => r.GetByIdAsync(participantId)).ReturnsAsync(participant);
            _mockVoteRepository.Setup(r => r.AddAsync(It.IsAny<Vote>())).Returns(Task.CompletedTask);

            // Act
            await _voteService.RegisterVoteAsync(participantId);

            // Assert
            _mockVoteRepository.Verify(r => r.AddAsync(It.Is<Vote>(v => v.ParticipantId == participantId)), Times.Once);
        }

        [Fact]
        public async Task RegisterVoteAsync_ShouldThrowArgumentException_WhenParticipantDoesNotExist()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            _mockParticipantRepository.Setup(r => r.GetByIdAsync(participantId)).ReturnsAsync((Participant?)null);

            // Act
            Func<Task> act = async () => await _voteService.RegisterVoteAsync(participantId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Participante inválido.");
        }

        [Fact]
        public async Task GetTotalVotesAsync_ShouldReturnTotalVotes()
        {
            // Arrange
            _mockVoteRepository.Setup(r => r.GetTotalVotesAsync()).ReturnsAsync(42);

            // Act
            var result = await _voteService.GetTotalVotesAsync();

            // Assert
            result.Should().Be(42);
        }

        [Fact]
        public async Task GetVotesPerHourAsync_ShouldReturnVotesPerHour()
        {
            // Arrange
            var votesPerHour = new List<VotesPerHourDto> { new VotesPerHourDto { Hour = DateTime.UtcNow, Total = 5 } };
            _mockVoteRepository.Setup(r => r.GetVotesPerHourAsync()).ReturnsAsync(votesPerHour);

            // Act
            var result = await _voteService.GetVotesPerHourAsync();

            // Assert
            result.Should().BeEquivalentTo(votesPerHour);
        }

        [Fact]
        public async Task GetTotalVotesByParticipantAsync_ShouldReturnVotesCount()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            _mockVoteRepository.Setup(r => r.GetTotalVotesByParticipantAsync(participantId)).ReturnsAsync(7);

            // Act
            var result = await _voteService.GetTotalVotesByParticipantAsync(participantId);

            // Assert
            result.Should().Be(7);
        }
    }
}
