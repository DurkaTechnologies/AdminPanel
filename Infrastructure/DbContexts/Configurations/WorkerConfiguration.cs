using AdminPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminPanel.Infrastructure.Persistence.Configurations
{
	public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(t => t.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(t => t.Surname)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(t => t.Lastname)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(t => t.Phone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.Image)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}