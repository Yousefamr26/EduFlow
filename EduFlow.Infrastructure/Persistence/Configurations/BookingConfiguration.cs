using EduFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookingTime)
            .IsRequired();

        builder.HasOne(b => b.Student)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Session)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique Booking
        builder.HasIndex(b => new { b.StudentId, b.SessionId })
            .IsUnique();

        builder.HasIndex(b => b.StudentId);
        builder.HasIndex(b => b.SessionId);
    }
}