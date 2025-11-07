using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.DTOs
{
    public class VotesPerHourDto
    {
        public DateTime Hour { get; set; }
        public int Total { get; set; }
    }
}
