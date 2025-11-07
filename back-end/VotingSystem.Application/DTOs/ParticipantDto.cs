using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Application.DTOs
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public int Votes { get; set; }
        public int PercentageOfVotes { get; set; }
    }
}
