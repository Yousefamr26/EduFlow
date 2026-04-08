using EduFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.FileUrl)
            .HasMaxLength(500);

        builder.Property(m => m.VideoUrl)
            .HasMaxLength(500);

        builder.HasOne(m => m.Session)
            .WithMany(s => s.Materials)
            .HasForeignKey(m => m.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Teacher)
            .WithMany()
            .HasForeignKey(m => m.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.SessionId);
    }
}