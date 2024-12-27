using HexAsset.Models;
using Microsoft.EntityFrameworkCore;

namespace HexAsset.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Asset> Assets { get; set; }
		public DbSet<AssetAllocation> AssetAllocations { get; set; }
		public DbSet<ServiceRequest> ServiceRequests { get; set; }
		public DbSet<AssetRequest> AssetRequests { get; set; }
		public DbSet<AuditRequest> AuditRequests { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// One-to-Many: AssetAllocation -> Asset
			modelBuilder.Entity<AssetAllocation>()
				.HasOne(a => a.Asset)
				.WithMany(b => b.AssetAllocations)
				.HasForeignKey(a => a.AssetId);

			// One-to-Many: AssetAllocation -> User
			modelBuilder.Entity<AssetAllocation>()
				.HasOne(a => a.User)
				.WithMany(b => b.AssetAllocations)
				.HasForeignKey(a => a.UserId);

			// One-to-Many: ServiceRequest -> Asset
			modelBuilder.Entity<ServiceRequest>()
				.HasOne(s => s.Asset)
				.WithMany(a => a.ServiceRequests)
				.HasForeignKey(s => s.AssetId);

			// One-to-Many: AssetRequest -> Asset
			modelBuilder.Entity<AssetRequest>()
				.HasOne(s => s.Asset)
				.WithMany(a => a.AssetRequests)
				.HasForeignKey(s => s.AssetId);

			// One-to-Many: ServiceRequest -> User
			modelBuilder.Entity<ServiceRequest>()
				.HasOne(s => s.User)
				.WithMany(u => u.ServiceRequests)
				.HasForeignKey(s => s.UserId);

			// One-to-Many: AssetRequest -> User
			modelBuilder.Entity<AssetRequest>()
				.HasOne(s => s.User)
				.WithMany(u => u.AssetRequests)
				.HasForeignKey(s => s.UserId);

			// One-to-Many: AuditRequest -> User
			modelBuilder.Entity<AuditRequest>()
				.HasOne(a => a.User)
				.WithMany(u => u.AuditRequests)
				.HasForeignKey(a => a.UserId);
		}

	}

}
