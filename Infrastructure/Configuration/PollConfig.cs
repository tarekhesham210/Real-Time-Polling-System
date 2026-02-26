using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    internal class PollConfig : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasMany(p => p.Options)
                        .WithOne(o => o.Poll)
                        .HasForeignKey(o => o.PollId)
                        .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.User)
                        .WithMany()
                        .HasForeignKey(p => p.CreatedById)
                        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
