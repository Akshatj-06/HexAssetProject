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
	public class ServiceRequestTests
	{
		private AppDbContext _context;
		private ServiceRequestController _controller;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDb")
				.Options;

			_context = new AppDbContext(options);
			_context.Database.EnsureCreated();

			_controller = new ServiceRequestController(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public async Task GetAllServiceRequests_ReturnsOkResult()
		{
			// Arrange
			var serviceRequests = new List<ServiceRequest>
			{
				new ServiceRequest { ServiceRequestId = 1, AssetId = 1, UserId = 1, RequestStatus = "Open", RequestDate = DateTime.UtcNow },
				new ServiceRequest { ServiceRequestId = 2, AssetId = 2, UserId = 2, RequestStatus = "InProgress", RequestDate = DateTime.UtcNow }
			};
			_context.ServiceRequests.AddRange(serviceRequests);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.GetAllServiceRequests();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;

			Assert.NotNull(okResult);
			var returnedRequests = okResult.Value as List<ServiceRequest>;
			Assert.NotNull(returnedRequests);
			Assert.AreEqual(serviceRequests.Count, returnedRequests.Count);
		}

		[Test]
		public async Task GetServiceRequestById_ServiceRequestNotFound_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetServiceRequestById(1);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
			var notFoundResult = result as NotFoundObjectResult;
			Assert.AreEqual("Service request with ID 1 not found.", notFoundResult.Value);
		}

		[Test]
		public async Task AddServiceRequest_ReturnsOkResult()
		{
			// Arrange
			var serviceRequestDto = new ServiceRequestDto
			{
				AssetId = 1,
				UserId = 1,
				Description = "Repair needed",
				RequestStatus = "Open",
				RequestDate = DateTime.UtcNow
			};

			// Act
			var result = await _controller.AddAsset(serviceRequestDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			var createdServiceRequest = okResult.Value as ServiceRequest;
			Assert.NotNull(createdServiceRequest);
			Assert.AreEqual(serviceRequestDto.AssetId, createdServiceRequest.AssetId);
		}

		[Test]
		public async Task UpdateServiceRequestById_ReturnsOkResult()
		{
			// Arrange
			var serviceRequest = new ServiceRequest
			{
				ServiceRequestId = 1,
				AssetId = 1,
				UserId = 1,
				Description = "Old description",
				RequestStatus = "Open",
				RequestDate = DateTime.UtcNow
			};
			_context.ServiceRequests.Add(serviceRequest);
			await _context.SaveChangesAsync();

			var updatedServiceRequestDto = new ServiceRequestDto
			{
				AssetId = 1,
				UserId = 1,
				Description = "Updated description",
				RequestStatus = "InProgress",
				RequestDate = DateTime.UtcNow
			};

			// Act
			var result = await _controller.UpdateServiceRequestById(1, updatedServiceRequestDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updatedServiceRequest = _context.ServiceRequests.Find(1);
			Assert.NotNull(updatedServiceRequest);
			Assert.AreEqual(updatedServiceRequestDto.Description, updatedServiceRequest.Description);
					}

		[Test]
		public async Task DeleteServiceRequestById_ReturnsOkResult()
		{
			// Arrange
			var serviceRequest = new ServiceRequest
			{
				ServiceRequestId = 1,
				AssetId = 1,
				UserId = 1,
				RequestStatus = "Open",
				RequestDate = DateTime.UtcNow
			};
			_context.ServiceRequests.Add(serviceRequest);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.DeleteServiceRequestById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var deletedServiceRequest = _context.ServiceRequests.Find(1);
			Assert.IsNull(deletedServiceRequest);
		}
	}
}
