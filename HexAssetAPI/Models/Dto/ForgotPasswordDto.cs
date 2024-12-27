using System.ComponentModel.DataAnnotations;

namespace HexAsset.Models.Dto
{
	public class ForgotPasswordDto
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public required string Email { get; set; }

		[Required(ErrorMessage = "New password is required.")]
		public required string NewPassword { get; set; }
	}
}
