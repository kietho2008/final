using System;
using System.Collections.Generic;
using G2CarRentalCustomers.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPlatform.Models;

public partial class G2CustomerProfileContext : DbContext
{
	public G2CustomerProfileContext()
	{
	}

	public G2CustomerProfileContext(DbContextOptions<G2CustomerProfileContext> options)
		: base(options)
	{
	}

	public virtual DbSet<G2Customer> Customers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CustomerProfile;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<G2Customer>(entity =>
		{
			entity.ToTable("Customers");
			entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC070DC399EC");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
