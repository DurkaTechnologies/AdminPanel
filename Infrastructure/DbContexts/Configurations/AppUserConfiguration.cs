using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(user => user.FirstName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(user => user.MiddleName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(user => user.LastName)
				.HasMaxLength(30)
				.IsRequired();

			builder.Property(user => user.ProfilePicture)
				.HasMaxLength(256);

			builder.Property(user => user.Description)
				.HasMaxLength(5000);

			builder.HasMany(user => user.Communities);
		}
	}
}
