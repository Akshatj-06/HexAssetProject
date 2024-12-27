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
	public class AuditRequestController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		public AuditRequestController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		[Route("GetAuditRequest")]
		public async Task<IActionResult> GetAllAuditRequests()
		{
			try
			{
				var auditRequests= await dbContext.AuditRequests.ToListAsync();
				return Ok(auditRequests);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}


		[HttpGet("GetAuditRequestById/{id}")]
		public async Task<IActionResult> GetAuditRequestById(int id)
		{
			try
			{
				var auditRequest = dbContext.AuditRequests.Find(id);
				if (auditRequest == null)
				{
					return NotFound($"Audit request with ID {id} not found.");
				}
				await dbContext.SaveChangesAsync();
				return Ok(auditRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}


		}



		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("AddAuditRequest")]
		public async Task<IActionResult> AddAuditRequest(AuditRequestDto auditassetRequestDto)
		{
			try
			{
				var newAuditRequest = new AuditRequest
				{
					UserId = auditassetRequestDto.UserId,
					AllocationId = auditassetRequestDto.AllocationId,
					AuditStatus = auditassetRequestDto.AuditStatus,
					Item = auditassetRequestDto.Item,
					AuditDate = auditassetRequestDto.AuditDate,
				};
				dbContext.AuditRequests.Add(newAuditRequest);
				await dbContext.SaveChangesAsync();

				return Ok(newAuditRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
		[Authorize(Roles = "Admin")]
		[HttpPut("UpdateAuditRequest/{id}")]
		public async Task<IActionResult> UpdateAuditRequestById(int id, AuditRequestDto auditRequestDto)
		{
			try
			{
				var auditRequest = dbContext.AuditRequests.Find(id);
				if (auditRequest == null) 
				{ 
					return NotFound($"Audit request with ID {id} not found.");
				}

				auditRequest.UserId = auditRequestDto.UserId;
				auditRequest.AllocationId = auditRequestDto.AllocationId;
				auditRequest.AuditStatus = auditRequestDto.AuditStatus;
				auditRequest.Item = auditRequestDto.Item;
				auditRequest.AuditDate = auditRequestDto.AuditDate;


				dbContext.AuditRequests.Update(auditRequest);
				await dbContext.SaveChangesAsync();
				return Ok(auditRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("DeleteAuditRequest/{id}")]
		public async Task<IActionResult> DeleteAuditRequestById(int id)
		{
			try
			{
				var auditRequest = dbContext.AuditRequests.Find(id);
				if (auditRequest == null)
				{
					return NotFound($"Audit request with ID {id} not found.");
				}
				dbContext.AuditRequests.Remove(auditRequest);
				await dbContext.SaveChangesAsync();
				return Ok(auditRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
	}
}
