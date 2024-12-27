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
	public class AuditRequestTests
	{
		private AppDbContext _context;
		private AuditRequestController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDb")
				.Options;

			_context = new AppDbContext(options);
			_context.Database.EnsureCreated();

			_controller = new AuditRequestController(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public async Task GetAllAuditRequests_ReturnsOkResult()
		{
			// Arrange
			var auditRequests = new List<AuditRequest>
			{
				new AuditRequest { AuditId = 1, UserId = 1, AuditStatus = "Pending", AuditDate = DateTime.UtcNow },
				new AuditRequest { AuditId = 2, UserId = 2, AuditStatus = "Completed", AuditDate = DateTime.UtcNow }
			};
			_context.AuditRequests.AddRange(auditRequests);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.GetAllAuditRequests();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;

			Assert.NotNull(okResult);
			var returnedAuditRequests = okResult.Value as List<AuditRequest>;
			Assert.NotNull(returnedAuditRequests);
			Assert.AreEqual(auditRequests.Count, returnedAuditRequests.Count);
		}

		[Test]
		public async Task GetAuditRequestById_AuditRequestNotFound_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetAuditRequestById(1);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
			var notFoundResult = result as NotFoundObjectResult;
			Assert.AreEqual("Audit request with ID 1 not found.", notFoundResult.Value);
		}

		[Test]
		public async Task AddAuditRequest_ReturnsOkResult()
		{
			// Arrange
			var auditRequestDto = new AuditRequestDto
			{
				UserId = 1,
				AuditStatus = "Pending",
				AuditDate = DateTime.UtcNow
			};

			// Act
			var result = await _controller.AddAuditRequest(auditRequestDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var createdAuditRequest = okResult.Value as AuditRequest;
			Assert.NotNull(createdAuditRequest);
			Assert.AreEqual(auditRequestDto.UserId, createdAuditRequest.UserId);
		}

		[Test]
		public async Task UpdateAuditRequestById_ReturnsOkResult()
		{
			// Arrange
			var auditRequest = new AuditRequest
			{
				AuditId = 1,
				UserId = 1,
				AuditStatus = "Pending",
				AuditDate = DateTime.UtcNow
			};
			_context.AuditRequests.Add(auditRequest);
			await _context.SaveChangesAsync();

			var updatedAuditRequestDto = new AuditRequestDto
			{
				UserId = 2,
				AuditStatus = "Completed",
				AuditDate = DateTime.UtcNow
			};

			// Act
			var result = await _controller.UpdateAuditRequestById(1, updatedAuditRequestDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updatedAuditRequest = _context.AuditRequests.Find(1);
			Assert.NotNull(updatedAuditRequest);
			Assert.AreEqual(updatedAuditRequestDto.UserId, updatedAuditRequest.UserId);
			Assert.AreEqual(updatedAuditRequestDto.AuditStatus, updatedAuditRequest.AuditStatus);
		}

		[Test]
		public async Task DeleteAuditRequestById_ReturnsOkResult()
		{
			// Arrange
			var auditRequest = new AuditRequest
			{
				AuditId = 1,
				UserId = 1,
				AuditStatus = "Pending",
				AuditDate = DateTime.UtcNow
			};
			_context.AuditRequests.Add(auditRequest);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.DeleteAuditRequestById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var deletedAuditRequest = _context.AuditRequests.Find(1);
			Assert.IsNull(deletedAuditRequest);
		}
	}
}
