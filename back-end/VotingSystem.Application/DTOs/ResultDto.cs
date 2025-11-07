using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Application.DTOs
{
    public class ResultDto
    {
        public List<ParticipantDto> Participants { get; set; }
        public int TotalVotes { get; set; }
    }
}
