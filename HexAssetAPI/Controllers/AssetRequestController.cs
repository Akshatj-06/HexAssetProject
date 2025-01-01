using HexAsset.Data;
using HexAsset.Models.Dto;
using HexAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HexAsset.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AssetRequestController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		public AssetRequestController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		[Route("GetAssetRequest")]
		[Authorize]
		public async Task<IActionResult> GetAllAssetRequests()
		{
			try
			{
				var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

				if (userRole == "Admin")
				{
					var assetRequests = await dbContext.AssetRequests.ToListAsync();
					return Ok(assetRequests);
				}
				else
				{
					var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
					if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
					{
						return Unauthorized("User ID not found in token.");
					}

					var userId = Convert.ToInt32(userIdClaim.Value);
					var assetRequests = await dbContext.AssetRequests
													   .Where(ar => ar.UserId == userId)
													   .ToListAsync();
					return Ok(assetRequests);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("GetAssetRequestById/{id}")]
		[Authorize]
		public async Task<IActionResult> GetAssetRequestById(int id)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var assetRequest = await dbContext.AssetRequests.FindAsync(id);

				if (assetRequest == null)
				{
					return NotFound($"Asset request with ID {id} not found.");
				}

				// Restrict users from accessing others' asset requests
				if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin" &&
					assetRequest.UserId != userId)
				{
					return Forbid("You are not authorized to access this asset request.");
				}

				return Ok(assetRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost]
		[Authorize(Roles = "Employee")]
		[Route("AddAssetRequest")]
		public async Task<IActionResult> AddAsset(AssetRequestDto assetRequestDto)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);

				var newAssetRequest = new AssetRequest
				{
					AssetId = assetRequestDto.AssetId,
					UserId = userId,
					UserName = assetRequestDto.UserName,
					Item = assetRequestDto.Item,
					RequestStatus = "Pending",
					RequestDate = assetRequestDto.RequestDate
				};

				dbContext.AssetRequests.Add(newAssetRequest);
				await dbContext.SaveChangesAsync();

				return Ok(newAssetRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPut("UpdateAssetRequest/{id}")]
		[Authorize(Roles = "Employee")]
		public async Task<IActionResult> UpdateAssetRequestById(int id, AssetRequestDto assetRequestDto)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var assetRequest = await dbContext.AssetRequests.FindAsync(id);

				if (assetRequest == null)
				{
					return NotFound($"Asset request with ID {id} not found.");
				}

				if (assetRequest.UserId != userId)
				{
					return Forbid("You can only update your own asset requests.");
				}

				assetRequest.AssetId = assetRequestDto.AssetId;
				assetRequest.Item = assetRequestDto.Item;
				assetRequest.UserName = assetRequestDto.UserName;
				assetRequest.RequestStatus = assetRequestDto.RequestStatus;
				assetRequest.RequestDate = assetRequestDto.RequestDate;

				dbContext.AssetRequests.Update(assetRequest);
				await dbContext.SaveChangesAsync();
				return Ok(assetRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete("DeleteAssetRequest/{id}")]
		[Authorize(Roles = "Employee")]
		public async Task<IActionResult> DeleteAssetRequestById(int id)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var assetRequest = await dbContext.AssetRequests.FindAsync(id);

				if (assetRequest == null)
				{
					return NotFound($"Asset request with ID {id} not found.");
				}

				if (assetRequest.UserId != userId)
				{
					return Forbid("You can only delete your own asset requests.");
				}

				dbContext.AssetRequests.Remove(assetRequest);
				await dbContext.SaveChangesAsync();
				return Ok(assetRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
