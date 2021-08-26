using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminPanel.Infrastructure.Persistence.Configurations
{
	public class CommunityConfiguration : IEntityTypeConfiguration<Community>
	{
		public void Configure(EntityTypeBuilder<Community> builder)
		{
			builder.Property(t => t.Name)
				.HasMaxLength(256)
				.IsRequired();

			builder.HasOne(x => x.District)
				.WithMany(x => x.Communities)
				.HasForeignKey(x => x.DistrictId);
		}
	}
}
