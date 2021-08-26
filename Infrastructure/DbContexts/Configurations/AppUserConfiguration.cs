using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminPanel.Infrastructure.Persistence.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(t => t.FirstName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(t => t.MiddleName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(t => t.LastName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(t => t.ProfilePicture)
				.HasMaxLength(256);

			builder.Property(t => t.Description)
				.HasMaxLength(2000);
		}
	}
}
