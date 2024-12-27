using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class AssetDto
	{
		[Required(ErrorMessage = "Name is required."), MaxLength(20)]
		public required string AssetName { get; set; }
		public string? AssetCategory { get; set; }
		public string? AssetModel { get; set; }
		public int AssetValue { get; set; }

		[Required(ErrorMessage = "Set Status as Available or Unavailable")]
		public required string CurrentStatus { get; set; }
	}
}
