using EduFlow.Domain.Common;
using EduFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Infrastructure.Persistence.Context
{
    public class EduDbContext : IdentityDbContext<ApplicationUser>
    {
        public EduDbContext(DbContextOptions<EduDbContext> options)
            : base(options)
        {
        }

        // 🧱 DbSets
        public DbSet<AccessCodes> AccessCodes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WaitingListEntry> WaitingListEntries { get; set; }

        // 🔥 Apply Configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🌟 Apply all IEntityTypeConfiguration in the assembly
            builder.ApplyConfigurationsFromAssembly(typeof(EduDbContext).Assembly);

            // ⚡ Configure RowVersion for all entities that inherit BaseEntity
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var prop = entityType.FindProperty("RowVersion");
                    if (prop != null)
                    {
                        prop.IsConcurrencyToken = true;
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAddOrUpdate;
                    }
                }
            }
        }
    }
}