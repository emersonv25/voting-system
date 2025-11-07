using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infra.Data.EntitiesConfiguration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Votes");

            builder.Property(v => v.ParticipantId).IsRequired();

            builder.HasOne(v => v.Participant)
                   .WithMany(p => p.Votes)
                   .HasForeignKey(v => v.ParticipantId);

            builder.HasIndex(v => v.ParticipantId);
            builder.HasIndex(v => v.CreatedAt);
        }
    }
}
