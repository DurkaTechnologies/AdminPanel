using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContexts.Configurations
{
	public class DistrictConfiguration : IEntityTypeConfiguration<District>
	{
		public void Configure(EntityTypeBuilder<District> builder)
		{
			builder.Property(t => t.Name)
				.HasMaxLength(256)
				.IsRequired();

		}
	}
}
