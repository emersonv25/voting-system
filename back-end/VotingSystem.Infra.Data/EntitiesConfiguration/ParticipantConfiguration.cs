using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Data.Mappings
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participants");
            builder.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v4()");
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.IsActive).HasDefaultValue(true).IsRequired();
        }
    }
}
