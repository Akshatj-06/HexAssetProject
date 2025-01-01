using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class AssetRequestDto
	{
		[Required(ErrorMessage = "Asset Id is required")]
		public int AssetId { get; set; }
		[Required(ErrorMessage = "User Id is required")]
		public int UserId { get; set; }
		[Required(ErrorMessage = "Please update status")]

		public string UserName { get; set; }
		public string? Item{ get; set; }
		public required string RequestStatus { get; set; }
		public DateTime RequestDate { get; set; } = DateTime.UtcNow;
	}
}
