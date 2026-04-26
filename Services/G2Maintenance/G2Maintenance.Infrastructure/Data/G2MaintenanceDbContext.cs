using G2Maintenance.Domain.HistoryAggregate;
using Microsoft.EntityFrameworkCore;

namespace G2Maintenance.Infrastructure.Data;

public class G2MaintenanceDbContext : DbContext
{
    public G2MaintenanceDbContext(DbContextOptions<G2MaintenanceDbContext> options) : base(options)
    {
    }
    
    public DbSet<G2RepairHistory>  RepairHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<G2RepairHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VehicleId).IsRequired();
        });
    }
}