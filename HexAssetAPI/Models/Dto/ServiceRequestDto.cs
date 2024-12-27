using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class ServiceRequestDto
	{
		[Required(ErrorMessage= "Asset Id is required")]
		public int AssetId { get; set; }
		[Required(ErrorMessage = "User Id is required")]
		public int UserId { get; set; }
		public string? Description { get; set; }
		[Required(ErrorMessage = "Please update status")]
		public required string RequestStatus { get; set; }
		public DateTime RequestDate { get; set; } = DateTime.UtcNow;
	}
}
