// DataAccessLayer/Concrete/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityLayer.EntityLayer.Entities;
using EntityLayer.Entities;

namespace DataAccessLayer.Concrete
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public override int SaveChanges()
        {
            UpdateAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditInformation()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is AuditTrailer && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var baseEntity = (AuditTrailer)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedDateTime = DateTime.UtcNow;
                    baseEntity.CreatedBy = "currentUser"; 
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.ModifiedDateTime = DateTime.UtcNow;
                    baseEntity.ModifiedBy = "currentUser"; 
                }
            }
        }
    }
}