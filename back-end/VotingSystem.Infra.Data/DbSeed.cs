using System;
using System.Collections.Generic;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infra.Data
{
    public class DbSeed
    {
        public static IEnumerable<Participant> GetParticipants()
        {
            return new List<Participant>
            {
                new Participant
                {
                    Id = Guid.Parse("72b0ec70-e394-4feb-a187-28d65e0c3947"),
                    Name = "Manu Gavazzi",
                    PhotoUrl = "https://i.imgur.com/NxoF9Dt.png",
                    IsActive = true,
                },
                new Participant
                {
                    Id = Guid.Parse("ecef9055-34a3-446a-8fca-778920cab9dc"),
                    Name = "Felipe Prior",
                    PhotoUrl = "https://i.imgur.com/yv5n9Rz.png",
                    IsActive = true,
                },
            };
        }
    }
}
