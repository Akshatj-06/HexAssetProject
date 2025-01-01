using HexAsset.Data;
using HexAsset.Models;
using HexAsset.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HexAsset.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AssetAllocationController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		public AssetAllocationController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		[Route("GetAssetAllocation")]
		[Authorize]
		public async Task<IActionResult> GetAllAssetAllocations()
		{
			try
			{
				// Retrieve the user's role from the claims
				var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

				if (userRole == "Admin")
				{
					// Admin can view all asset allocations
					var assetAllocations = await dbContext.AssetAllocations.ToListAsync();
					return Ok(assetAllocations);
				}
				else
				{
					// Employee can only view their allocated assets
					var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
					if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
					{
						return Unauthorized("User ID not found in token.");
					}

					var userId = Convert.ToInt32(userIdClaim.Value);
					var assetAllocations = await dbContext.AssetAllocations
														  .Where(aa => aa.UserId == userId)
														  .ToListAsync();
					return Ok(assetAllocations);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}



		[HttpGet("GetAssetAllocationById/{id}")]
		public async Task<IActionResult> GetAssetAllocationById(int id)
		{
			var assetAllocation = dbContext.AssetAllocations.Find(id);
			if (assetAllocation == null)
			{
				return NotFound($"AssetAllocation with ID {id} not found.");
			}
			await dbContext.SaveChangesAsync();
			return Ok(assetAllocation);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("AddAssetAllocation")]

		public async Task<IActionResult> AddAssetAllocation(AssetAllocationDto assetAllocationDto)
		{
			try 
			{
				var newAssetAllocation = new AssetAllocation
				{
					AssetId = assetAllocationDto.AssetId,
					UserId = assetAllocationDto.UserId,
					AllocationDate = assetAllocationDto.AllocationDate,
					ReturnDate = assetAllocationDto.ReturnDate,
					AllocationStatus = assetAllocationDto.AllocationStatus,
				};
				dbContext.AssetAllocations.Add(newAssetAllocation);
				await dbContext.SaveChangesAsync();

				return Ok(newAssetAllocation);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[Authorize(Roles = "Admin")]
		[HttpPut("UpdateAssetAllocation/{id}")]
		public async Task<IActionResult> UpdateAssetAllocationById(int id, AssetAllocationDto assetAllocationDto)
		{
			try
			{
				var assetAllocation = dbContext.AssetAllocations.Find(id);
				if (assetAllocation == null)
				{ 
					return NotFound($"AssetAllocation with ID {id} not found."); 
				}

				assetAllocation.AssetId = assetAllocationDto.AssetId;
				assetAllocation.UserId = assetAllocationDto.UserId;
				assetAllocation.AllocationDate = assetAllocationDto.AllocationDate;
				assetAllocation.ReturnDate = assetAllocationDto.ReturnDate;
				assetAllocation.AllocationStatus = assetAllocationDto.AllocationStatus;


				dbContext.AssetAllocations.Update(assetAllocation);
				await dbContext.SaveChangesAsync();
				return Ok(assetAllocation);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("DeleteAssetAllocation/{id}")]
		public async Task<IActionResult> DeleteAssetAllocationById(int id)
		{
			try
			{
				var assetAllocation = dbContext.AssetAllocations.Find(id);
				if (assetAllocation == null)
				{
					return NotFound($"AssetAllocation with ID {id} not found.");
				}
				dbContext.AssetAllocations.Remove(assetAllocation);
				await dbContext.SaveChangesAsync();
				return Ok(assetAllocation);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
