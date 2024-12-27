using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models
{
	public class ServiceRequest
	{
		[Key]
		public int ServiceRequestId { get; set; }
		public int AssetId { get; set; }
		public int UserId { get; set; }
		public string? Description { get; set; }
		public required string RequestStatus { get; set; }
		public DateTime RequestDate { get; set; } = DateTime.UtcNow;


		public Asset? Asset { get; set; }
		public User? User { get; set; }
	}
}
