using AdminPanel.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContexts.Configurations
{
    class CorrespondenceConfiguration : IEntityTypeConfiguration<Correspondence>
    {
        public void Configure(EntityTypeBuilder<Correspondence> builder)
        {
            builder
                .HasOne(x => x.Worker)
                .WithMany(x => x.Correspondences)
                .HasForeignKey(x => x.WorkerId);
        }
    }
}
