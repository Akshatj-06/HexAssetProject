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
	public class AssetRequestTests
	{
		private AppDbContext _context;
		private AssetRequestController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDb")
				.Options;

			_context = new AppDbContext(options);
			_controller = new AssetRequestController(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
		[Test]
		public async Task GetAllAssetRequests_ReturnsOkResult()
		{
			// Arrange
			var assetRequests = new List<AssetRequest>
	{
		new AssetRequest { AssetRequestId = 1, AssetId = 1, UserId = 1, RequestStatus = "Pending" },
		new AssetRequest { AssetRequestId = 2, AssetId = 2, UserId = 2, RequestStatus = "Approved" }
	};

			_context.AssetRequests.AddRange(assetRequests);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.GetAllAssetRequests();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var returnedRequests = okResult.Value as List<AssetRequest>;
			Assert.AreEqual(2, returnedRequests.Count);
		}
		[Test]
		public async Task GetAssetRequestById_AssetNotFound_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetAssetRequestById(1);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
			var notFoundResult = result as NotFoundObjectResult;
			Assert.AreEqual("Asset request with ID 1 not found.", notFoundResult.Value);
		}

		[Test]
		public async Task AddAsset_ReturnsOkResult()
		{
			// Arrange
			var assetRequestDto = new AssetRequestDto
			{
				AssetId = 1,
				UserId = 1,
				RequestStatus = "Pending",
			};

			// Act
			var result = await _controller.AddAsset(assetRequestDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var createdRequest = okResult.Value as AssetRequest;
			Assert.NotNull(createdRequest);
			Assert.AreEqual(assetRequestDto.AssetId, createdRequest.AssetId);
		}
		[Test]
		public async Task UpdateAssetRequestById_ReturnsOkResult()
		{
			// Arrange
			var assetRequest = new AssetRequest
			{
				AssetRequestId = 1,
				AssetId = 1,
				UserId = 1,
				RequestStatus = "Pending"
			};
			_context.AssetRequests.Add(assetRequest);
			await _context.SaveChangesAsync();

			var updatedDto = new AssetRequestDto
			{
				AssetId = 2,
				UserId = 2,
				RequestStatus = "Approved"
			};

			// Act
			var result = await _controller.UpdateAssetRequestById(1, updatedDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updatedRequest = _context.AssetRequests.Find(1);
			Assert.NotNull(updatedRequest);
			Assert.AreEqual(updatedDto.RequestStatus, updatedRequest.RequestStatus);
		}
		[Test]
		public async Task DeleteAssetRequestById_ReturnsOkResult()
		{
			// Arrange
			var assetRequest = new AssetRequest
			{
				AssetRequestId = 1,
				AssetId = 1,
				UserId = 1,
				RequestStatus = "Pending"
			};
			_context.AssetRequests.Add(assetRequest);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.DeleteAssetRequestById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var deletedRequest = _context.AssetRequests.Find(1);
			Assert.IsNull(deletedRequest);
		}

	}
}
