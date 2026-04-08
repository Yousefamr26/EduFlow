using EduFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccessCodeConfiguration : IEntityTypeConfiguration<AccessCodes>
{
    public void Configure(EntityTypeBuilder<AccessCodes> builder)
    {
        builder.ToTable("AccessCodes");

        BaseEntityConfiguration.Configure(builder);

        builder.Property(a => a.CodeHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(a => a.CodeHash)
            .IsUnique();

        builder.Property(a => a.ExpiryDate)
            .IsRequired();

        builder.Property(a => a.IsUsed)
            .HasDefaultValue(false);

        builder.Property(a => a.Attempts)
            .HasDefaultValue(0);

        builder.HasCheckConstraint("CK_AccessCode_Attempts",
            "[Attempts] >= 0 AND [Attempts] <= 5");

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.UserId);
    }
}