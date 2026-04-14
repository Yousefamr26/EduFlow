using EduFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduFlow.Infrastructure.Persistence.Configurations
{
    public class WaitingListEntryConfiguration : IEntityTypeConfiguration<WaitingListEntry>
    {
        public void Configure(EntityTypeBuilder<WaitingListEntry> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.StudentId)
                .IsRequired();

            builder.Property(w => w.SessionId)
                .IsRequired();

            builder.Property(w => w.RequestTime)
                .IsRequired();

            builder.Property(w => w.QueuePosition)
                .IsRequired();

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.Property(w => w.UpdatedAt)
                .IsRequired();

            builder.HasIndex(w => new { w.StudentId, w.SessionId })
                .IsUnique()
                .HasName("IX_WaitingListEntry_StudentId_SessionId");

            builder.HasIndex(w => w.SessionId)
                .HasName("IX_WaitingListEntry_SessionId");

            builder.HasIndex(w => w.RequestTime)
                .HasName("IX_WaitingListEntry_RequestTime");

            builder.HasOne(w => w.Student)
                .WithMany()
                .HasForeignKey(w => w.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Session)
                .WithMany()
                .HasForeignKey(w => w.SessionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
