using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Domain.LocationAggregate;
using G2VehicleInventory.Domain.VehicleAggregate;

namespace G2VehicleInventory.Infrastructure.Data
{
	public class G2InventoryDbContext : DbContext
	{
		public G2InventoryDbContext(DbContextOptions<G2InventoryDbContext> options) : base(options)
		{
		}
		public DbSet<G2Vehicle> Vehicles { get; set; }
		public DbSet<G2Location> Locations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<G2Vehicle>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Map value obj
				entity.OwnsOne(e => e.VehicleCode, vc =>
				{
					vc.Property(p => p.Value)
					  .HasColumnName("VehicleCode")
					  .IsRequired()
					  .HasMaxLength(10);
				});
				entity.Property(e => e.LocationId).IsRequired();
				entity.Property(e => e.VehicleType).IsRequired();
				entity.Property(e => e.VehicleStatus).IsRequired();
			});

			modelBuilder.Entity<G2Location>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

				// Map value obj
				entity.OwnsOne(e => e.Address, a =>
				{
					a.Property(p => p.Street).IsRequired().HasMaxLength(100);
					a.Property(p => p.City).IsRequired().HasMaxLength(50);
					a.Property(p => p.PostalCode).IsRequired().HasMaxLength(10);
				});
			});
		}
	}
}
