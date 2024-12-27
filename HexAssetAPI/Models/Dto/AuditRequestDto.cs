using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class AuditRequestDto
	{
		[Required(ErrorMessage ="User Id is required")]
		public int UserId { get; set; }
		[Required(ErrorMessage = "User Id is status")]

		public int? AllocationId { get; set; }
		public required string AuditStatus { get; set; }
		public string Item { get; set; }
		public DateTime AuditDate { get; set; } = DateTime.UtcNow;
	}
}
