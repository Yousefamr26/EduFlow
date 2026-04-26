using EduFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduFlow.Infrastructure.Persistence.Configurations
{
    public class TeacherRatingConfiguration : IEntityTypeConfiguration<TeacherRating>
    {
        public void Configure(EntityTypeBuilder<TeacherRating> builder)
        {
            builder.HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Teacher)
                .WithMany()
                .HasForeignKey(r => r.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}