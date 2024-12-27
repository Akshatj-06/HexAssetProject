using HexAsset.Data;
using HexAsset.Models;
using HexAsset.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HexAsset.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AssetController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		public AssetController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}


		[HttpGet]
		[Route("GetAsset")]
		public async Task <IActionResult> GetAllAssets()
		{
			try
			{
				var assets = await dbContext.Assets.ToListAsync();
				return Ok(assets);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		[HttpGet("GetAssetById/{id}")]
		public async Task<IActionResult> GetAssetById(int id)
		{
			try
			{
				var asset = dbContext.Assets.Find(id);
				if (asset == null)
				{
					return NotFound($"Asset with ID {id} not found.");
				}
				await dbContext.SaveChangesAsync();
				return Ok(asset);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("AddAsset")]
		public async Task<IActionResult> AddAsset(AssetDto assetDto)
		{
			try
			{
				var newAsset = new Asset
				{
					AssetName = assetDto.AssetName,
					AssetCategory = assetDto.AssetCategory,
					AssetModel = assetDto.AssetModel,
					AssetValue = assetDto.AssetValue,
					CurrentStatus = assetDto.CurrentStatus
				};
				dbContext.Assets.Add(newAsset);
				await dbContext.SaveChangesAsync();

				return Ok(newAsset);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[Authorize(Roles = "Admin")]
		[HttpPut("UpdateAsset/{id}")]
		public async Task<IActionResult> UpdateAssetById(int id, AssetDto assetDto)
		{
			try
			{
				var asset = dbContext.Assets.Find(id);
				if (asset == null) 
				{
					return NotFound($"Asset with ID {id} not found."); 
				}

				asset.AssetName = assetDto.AssetName;
				asset.AssetCategory = assetDto.AssetCategory;
				asset.AssetModel = assetDto.AssetModel;
				asset.AssetValue = assetDto.AssetValue;
				asset.CurrentStatus = assetDto.CurrentStatus;


				dbContext.Assets.Update(asset);
				await dbContext.SaveChangesAsync();
				return Ok(asset);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
			
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("DeleteAsset/{id}")]
		public async Task<IActionResult> DeleteAssetById(int id)
		{
			try
			{
				var asset = dbContext.Assets.Find(id);
				if (asset == null)
				{
					return NotFound($"Asset with ID {id} not found.");
				}
				dbContext.Assets.Remove(asset);
				await dbContext.SaveChangesAsync();
				return Ok(asset);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
	}
}
