using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models
{
	public class AssetRequest
	{
		[Key]
		public int AssetRequestId { get; set; }
		public int AssetId { get; set; }
		public int UserId { get; set; }

		public string? Item {  get; set; } 
		public required string RequestStatus { get; set; }
		public DateTime RequestDate { get; set; } = DateTime.UtcNow;


		public Asset? Asset { get; set; }
		public User? User { get; set; }
	}
}
