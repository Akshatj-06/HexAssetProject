using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }
		public required string Role { get; set; }
		public required string Name { get; set; }

		public required string Email { get; set; }
		public required string Password { get; set; }
		public string? ContactNumber { get; set; }
		public string? Address { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;


		public ICollection<AssetAllocation>? AssetAllocations { get; set; }
		public ICollection<ServiceRequest>? ServiceRequests { get; set; }
		public ICollection<AssetRequest>? AssetRequests { get; set; }
		public ICollection<AuditRequest>? AuditRequests { get; set; }
	}
}
