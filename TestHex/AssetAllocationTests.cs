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
using System.Text;
using System.Threading.Tasks;

namespace TestHex
{
	[TestFixture]
	public class AssetAllocationTests
	{
		private AppDbContext _context;
		private AssetAllocationController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDb")
				.Options;

			_context = new AppDbContext(options);
			_context.Database.EnsureCreated();

			_controller = new AssetAllocationController(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
		[Test]
		public async Task GetAllAssetAllocations_ReturnsOkResult()
		{
			// Arrange
			var assetAllocations = new List<AssetAllocation>
			{
				new AssetAllocation { AllocationId = 1, AssetId = 1, UserId = 1, AllocationStatus = "Pending" },
				new AssetAllocation { AllocationId = 2, AssetId = 2, UserId = 2, AllocationStatus = "Allocated" }
			};

			_context.AssetAllocations.AddRange(assetAllocations);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.GetAllAssetAllocations();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;

			Assert.NotNull(okResult);
			var returnedAssetAllocations = okResult.Value as List<AssetAllocation>;
			Assert.NotNull(returnedAssetAllocations);
			Assert.AreEqual(assetAllocations.Count, returnedAssetAllocations.Count);
		}

		[Test]
		public async Task GetAssetAllocationById_AssetNotFound_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetAssetAllocationById(1);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
			var notFoundResult = result as NotFoundObjectResult;
			Assert.AreEqual("AssetAllocation with ID 1 not found.", notFoundResult.Value);
		}

		[Test]
		public async Task AddAssetAllocation_ReturnsOkResult()
		{
			// Arrange
			var assetAllocationDto = new AssetAllocationDto
			{
				AssetId = 1,
				UserId = 1,
				AllocationStatus = "Pending"
			};

			// Act
			var result = await _controller.AddAssetAllocation(assetAllocationDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var createdAssetAllocation = okResult.Value as AssetAllocation;
			Assert.NotNull(createdAssetAllocation);
			Assert.AreEqual(assetAllocationDto.AssetId, createdAssetAllocation.AssetId);
		}

		[Test]
		public async Task UpdateAssetAllocationById_ReturnsOkResult()
		{
			// Arrange
			var assetAllocation = new AssetAllocation
			{
				AllocationId = 1,
				AssetId = 1,
				UserId = 1,
				AllocationStatus = "Pending"
			};
			_context.AssetAllocations.Add(assetAllocation);
			await _context.SaveChangesAsync();

			var updatedAssetAllocationDto = new AssetAllocationDto
			{
				AssetId = 2,
				UserId = 2,
				AllocationStatus = "Allocated"
			};

			// Act
			var result = await _controller.UpdateAssetAllocationById(1, updatedAssetAllocationDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updateAssetAllocation = _context.AssetAllocations.Find(1);
			Assert.NotNull(updateAssetAllocation);
			Assert.AreEqual(updatedAssetAllocationDto.AssetId, updateAssetAllocation.AssetId);
		}
		[Test]
		public async Task DeleteAssetAllocationById_ReturnsOkResult()
		{
			// Arrange
			var assetAllocation = new AssetAllocation
			{
				AllocationId = 1,
				AssetId = 1,
				UserId = 1,
				AllocationStatus = "Pending"

			};
			_context.AssetAllocations.Add(assetAllocation);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.DeleteAssetAllocationById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var deletedAssetAllocation = _context.AssetAllocations.Find(1);
			Assert.IsNull(deletedAssetAllocation);
		}
	}
}

