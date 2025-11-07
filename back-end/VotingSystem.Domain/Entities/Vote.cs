using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.Entities
{
    public class Vote : BaseEntity
    {
        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; }
    }
}
