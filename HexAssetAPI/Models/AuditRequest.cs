using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models
{
	public class AuditRequest
	{
		[Key]
		public int AuditId { get; set; }
		public int UserId { get; set; }

		public int? AllocationId { get; set; }
		public required string AuditStatus { get; set; }

		public string? Item { get; set; }
		public DateTime AuditDate { get; set; } = DateTime.UtcNow;

		public User? User { get; set; }
	}
}
