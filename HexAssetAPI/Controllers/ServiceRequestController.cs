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
	public class ServiceRequestController : ControllerBase
	{
		private readonly AppDbContext dbContext;

		public ServiceRequestController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		[Route("GetServiceRequests")]
		[Authorize]
		public async Task<IActionResult> GetAllServiceRequests()
		{
			try
			{
				var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

				if (userRole == "Admin")
				{
					var serviceRequests = await dbContext.ServiceRequests.ToListAsync();
					return Ok(serviceRequests);
				}
				else
				{
					var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
					if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
					{
						return Unauthorized("User ID not found in token.");
					}

					var userId = Convert.ToInt32(userIdClaim.Value);
					var serviceRequests = await dbContext.ServiceRequests
														 .Where(sr => sr.UserId == userId)
														 .ToListAsync();
					return Ok(serviceRequests);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("GetServiceRequestById/{id}")]
		[Authorize]
		public async Task<IActionResult> GetServiceRequestById(int id)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var serviceRequest = await dbContext.ServiceRequests.FindAsync(id);

				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found.");
				}

				if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin" &&
					serviceRequest.UserId != userId)
				{
					return Forbid("You are not authorized to access this service request.");
				}

				return Ok(serviceRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost]
		[Authorize(Roles = "Employee")]
		[Route("AddServiceRequest")]
		public async Task<IActionResult> AddServiceRequest(ServiceRequestDto serviceRequestDto)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);

				var newServiceRequest = new ServiceRequest
				{
					AssetId = serviceRequestDto.AssetId,
					UserId = userId,
					UserName = serviceRequestDto.UserName,
					Description = serviceRequestDto.Description,
					RequestStatus = "Pending",
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

		[HttpPut("UpdateServiceRequest/{id}")]
		[Authorize(Roles = "Employee")]
		public async Task<IActionResult> UpdateServiceRequestById(int id, ServiceRequestDto serviceRequestDto)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var serviceRequest = await dbContext.ServiceRequests.FindAsync(id);

				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found.");
				}

				if (serviceRequest.UserId != userId)
				{
					return Forbid("You can only update your own service requests.");
				}

				serviceRequest.AssetId = serviceRequestDto.AssetId;
				serviceRequest.UserName = serviceRequestDto.UserName;
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

		[HttpDelete("DeleteServiceRequest/{id}")]
		[Authorize(Roles = "Employee")]
		public async Task<IActionResult> DeleteServiceRequestById(int id)
		{
			try
			{
				var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
				if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Unauthorized("User ID not found in token.");
				}

				var userId = Convert.ToInt32(userIdClaim.Value);
				var serviceRequest = await dbContext.ServiceRequests.FindAsync(id);

				if (serviceRequest == null)
				{
					return NotFound($"Service request with ID {id} not found.");
				}

				if (serviceRequest.UserId != userId)
				{
					return Forbid("You can only delete your own service requests.");
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
