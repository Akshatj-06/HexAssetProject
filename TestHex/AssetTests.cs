using HexAsset.Controllers;
using HexAsset.Data;
using HexAsset.Models;
using HexAsset.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHex
{
	[TestFixture]
	public class AssetTests
	{
		private AppDbContext _context;
		private AssetController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDb")
			.Options;

			_context = new AppDbContext(options);
			_context.Database.EnsureCreated();

			_controller = new AssetController(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public async Task GetAllAssets_ReturnsOkResult()
		{
			// Arrange
			var assets = new List<Asset>
			{
				new Asset { AssetId = 1, AssetName = "Asset 1", AssetCategory = "Category 1", AssetValue = 1000, CurrentStatus = "Available" },
				new Asset { AssetId = 2, AssetName = "Asset 2", AssetCategory = "Category 2", AssetValue = 1500, CurrentStatus = "Available" }
			};
			_context.Assets.AddRange(assets);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.GetAllAssets();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result); // Check if result is OkObjectResult.
			var okResult = result as OkObjectResult;

			Assert.NotNull(okResult);
			var returnedAssets = okResult.Value as List<Asset>;
			Assert.NotNull(returnedAssets);
			Assert.AreEqual(assets.Count, returnedAssets.Count);
		}

		[Test]
		public async Task GetAssetById_AssetNotFound_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetAssetById(1);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
			var notFoundResult = result as NotFoundObjectResult;
			Assert.AreEqual("Asset with ID 1 not found.", notFoundResult.Value);
		}

		[Test]
		public async Task AddAsset_ReturnsOkResult()
		{
			// Arrange
			var assetDto = new AssetDto
			{
				AssetName = "New Asset",
				AssetCategory = "Category 1",
				AssetValue = 2000,
				CurrentStatus = "Available"
			};

			// Act
			var result = await _controller.AddAsset(assetDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var createdAsset = okResult.Value as Asset;
			Assert.NotNull(createdAsset);
			Assert.AreEqual(assetDto.AssetName, createdAsset.AssetName);
		}

		[Test]
		public async Task UpdateAssetById_ReturnsOkResult()
		{
			// Arrange
			var asset = new Asset
			{
				AssetId = 1,
				AssetName = "Old Asset",
				AssetCategory = "Old Category",
				AssetValue = 1000,
				CurrentStatus = "Available"
			};
			_context.Assets.Add(asset);
			await _context.SaveChangesAsync();

			var updatedAssetDto = new AssetDto
			{
				AssetName = "Updated Asset",
				AssetCategory = "Updated Category",
				AssetValue = 2500,
				CurrentStatus = "Unavailable"
			};

			// Act
			var result = await _controller.UpdateAssetById(1, updatedAssetDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updatedAsset = _context.Assets.Find(1);
			Assert.NotNull(updatedAsset);
			Assert.AreEqual(updatedAssetDto.AssetName, updatedAsset.AssetName);
		}

		[Test]
		public async Task DeleteAssetById_ReturnsOkResult()
		{
			// Arrange
			var asset = new Asset
			{
				AssetId = 1,
				AssetName = "Asset to Delete",
				AssetCategory = "Category 1",
				AssetValue = 3000,
				CurrentStatus = "Unavailable"
			};
			_context.Assets.Add(asset);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.DeleteAssetById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var deletedAsset = _context.Assets.Find(1);
			Assert.IsNull(deletedAsset);
		}
	}
}
