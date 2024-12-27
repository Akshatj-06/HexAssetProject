using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class AssetAllocationDto
	{
		[Required(ErrorMessage = "Asset Id is required.")]
		public required int AssetId { get; set; }
		[Required(ErrorMessage = "User Id is required")]
		public required int UserId { get; set; }
		public DateTime AllocationDate { get; set; } = DateTime.UtcNow;
		public DateTime? ReturnDate { get; set; }
		[Required(ErrorMessage = "Please provide Allocation Status")]
		public required string AllocationStatus { get; set; }
	}
}
