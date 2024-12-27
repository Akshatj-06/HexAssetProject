using HexAsset.Data;
using HexAsset.Models.Dto;
using HexAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HexAsset.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceRequestController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		public ServiceRequestController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		[Route("GetServiceRequest")]
		public async Task<IActionResult> GetAllServiceRequests()
		{
			try
			{
				var serviceRequests = await dbContext.ServiceRequests.ToListAsync();
				return Ok(serviceRequests);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[HttpGet("GetServiceRequestById/{id}")]
		public async Task<IActionResult> GetServiceRequestById(int id)
		{
			try
			{
				var serviceRequest = dbContext.ServiceRequests.Find(id);
				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found.");
				}
				await dbContext.SaveChangesAsync();
				return Ok(serviceRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[HttpPost]
		[Route("AddServiceRequest")]
		public async Task<IActionResult> AddAsset(ServiceRequestDto serviceRequestDto)
		{
			try
			{
				var newServiceRequest = new ServiceRequest
				{
					AssetId = serviceRequestDto.AssetId,
					UserId = serviceRequestDto.UserId,
					Description = serviceRequestDto.Description,
					RequestStatus = serviceRequestDto.RequestStatus,
					RequestDate = serviceRequestDto.RequestDate

				};
				dbContext.ServiceRequests.Add(newServiceRequest);
				await dbContext.SaveChangesAsync();

				return Ok(newServiceRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[Authorize(Roles = "Employee")]
		[HttpPut("UpdateServiceRequest/{id}")]
		public async Task<IActionResult> UpdateServiceRequestById(int id, ServiceRequestDto serviceRequestDto)
		{
			try
			{
				var serviceRequest = dbContext.ServiceRequests.Find(id);
				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found."); 
				}

				serviceRequest.AssetId = serviceRequestDto.AssetId;
				serviceRequest.UserId = serviceRequestDto.UserId;
				serviceRequest.Description = serviceRequestDto.Description;
				serviceRequest.RequestStatus = serviceRequestDto.RequestStatus;
				serviceRequest.RequestDate = serviceRequestDto.RequestDate;

				dbContext.ServiceRequests.Update(serviceRequest);
				await dbContext.SaveChangesAsync();
				return Ok(serviceRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[Authorize(Roles = "Employee")]
		[HttpDelete("DeleteServiceRequest/{id}")]
		public async Task<IActionResult> DeleteServiceRequestById(int id)
		{
			try
			{
				var serviceRequest = dbContext.ServiceRequests.Find(id);
				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found.");
				}
				dbContext.ServiceRequests.Remove(serviceRequest);
				await dbContext.SaveChangesAsync();
				return Ok(serviceRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
	}
}
