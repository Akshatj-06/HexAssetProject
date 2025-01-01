using HexAsset.Data;
using HexAsset.Models.Dto;
using HexAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace HexAsset.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext dbContext;
		private readonly IConfiguration _config;

		public UserController(AppDbContext dbContext, IConfiguration config)
		{
			this.dbContext = dbContext;
			this._config = config;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("GetUser")]
		public async Task<IActionResult> GetAllUsers()
		{
			try
			{
				var users = await dbContext.Users.ToListAsync();
				return Ok(users);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}


		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] UserDto userdto)
		{
			try
			{
				if (await dbContext.Users.AnyAsync(u => u.Name == userdto.Name))
				{
					return Conflict("Username already exists.");
				}

				var user = new User
				{
					Role = userdto.Role,
					Name = userdto.Name,
					Email = userdto.Email,
					Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password),
					ContactNumber = userdto.ContactNumber,
					Address = userdto.Address,
					DateCreated = userdto.DateCreated,
				};

				dbContext.Users.Add(user);
				await dbContext.SaveChangesAsync();
				return Ok("User registered successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto userlogin)
		{
			try
			{
				if (userlogin == null || string.IsNullOrEmpty(userlogin.Email) || string.IsNullOrEmpty(userlogin.Password))
				{
					return BadRequest("Email or Password cannot be empty.");
				}

				var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userlogin.Email);
				if (user == null || !BCrypt.Net.BCrypt.Verify(userlogin.Password, user.Password))
				{
					return Unauthorized("Invalid username or password.");
				}

				var token = GenerateJwtToken(user);
				return Ok(new { Token = token });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
			}
		}

		private string GenerateJwtToken(User user)
		{
			try
			{
				var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
				var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

				var claims = new[]
				{
			new Claim(JwtRegisteredClaimNames.Sub, user.Email),
			new Claim("UserId", user.UserId.ToString()),
			new Claim("role", user.Role),
			new Claim(ClaimTypes.Role, user.Role),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

		};
				Console.WriteLine($"Generated token for user {user.Email} with role: {user.Role}");


				var token = new JwtSecurityToken(
					_config["Jwt:Issuer"],
					_config["Jwt:Audience"],
					claims,
					expires: DateTime.Now.AddMinutes(30),
					signingCredentials: credentials);

				return new JwtSecurityTokenHandler().WriteToken(token);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Error generating the JWT token.", ex);
			}
		}

		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
		{
			try
			{
				if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
				{
					return BadRequest("Email or new password is missing.");
				}

				var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
				if (user == null)
				{
					return NotFound("User with the specified email does not exist.");
				}

				user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

				dbContext.Users.Update(user);
				await dbContext.SaveChangesAsync();

				return Ok(new { message = "Password updated successfully!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}




		[HttpGet("GetUserById/{id}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			try
			{
				var user = dbContext.Users.Find(id);
				if (user == null)
				{
					return NotFound($"User with ID {id} not found.");
				}
				await dbContext.SaveChangesAsync();
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}


		[HttpPut("UpdateUser/{id}")]
		public async Task<IActionResult> UpdateUserById(int id, [FromBody] UserDto userDto)
		{
			try
			{
				var user = dbContext.Users.Find(id);
				if (user == null)
					return NotFound($"User with ID {id} not found.");

				user.Role = userDto.Role;
				user.Name = userDto.Name;
				user.Email = userDto.Email;
				user.ContactNumber = userDto.ContactNumber;
				user.Address = userDto.Address;

				if (!string.IsNullOrWhiteSpace(userDto.Password))
					user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

				dbContext.Users.Update(user);
				await dbContext.SaveChangesAsync();

				return Ok(new { message = "User updated successfully!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}


		[HttpDelete("DeleteUser/{id}")]

		public async Task<IActionResult> DeleteUserById(int id)
		{
			try
			{
				var user = dbContext.Users.Find(id);
				if (user == null)
					return NotFound($"User with ID {id} not found.");

				dbContext.Users.Remove(user);
				await dbContext.SaveChangesAsync();

				return Ok(new { message = "User deleted successfully!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}
	}
}
