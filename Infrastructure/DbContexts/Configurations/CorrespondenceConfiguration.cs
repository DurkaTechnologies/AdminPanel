using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContexts.Configurations
{
	class CorrespondenceConfiguration : IEntityTypeConfiguration<Correspondence>
	{
		public void Configure(EntityTypeBuilder<Correspondence> builder)
		{
			builder.Property(t => t.RequestNumber).IsRequired();

			builder.HasOne(x => x.Worker)
				.WithMany(x => x.Correspondences)
				.HasForeignKey(x => x.WorkerId);
		}
	}
}
