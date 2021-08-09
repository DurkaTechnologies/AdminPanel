using AdminPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminPanel.Infrastructure.Persistence.Configurations
{
	public class CommunityConfiguration : IEntityTypeConfiguration<Community>
	{
		public void Configure(EntityTypeBuilder<Community> builder)
		{
			builder.Ignore(e => e.DomainEvents);


			builder.Property(t => t.Name)
				.HasMaxLength(256)
				.IsRequired();
		}
	}
}
