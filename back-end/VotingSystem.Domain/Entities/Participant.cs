using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Vote> Votes { get; set; }   
    }
}
