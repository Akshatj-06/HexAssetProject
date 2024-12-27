using HexAsset.Controllers;
using HexAsset.Data;
using HexAsset.Models;
using HexAsset.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHex
{
	[TestFixture]
	public class UserTests
	{
		private UserController _controller;
		private AppDbContext _context;
		private Mock<IConfiguration> _mockConfig;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "HexAssetTestDB")
				.Options;
			_context = new AppDbContext(options);

			// Seed in-memory database
			_context.Users.AddRange(new List<User>
			{
				new User
				{
					UserId = 1,
					Role = "Admin",
					Name = "Ram Mohan",
					Email = "ram@gmail.comcom",
					Password = BCrypt.Net.BCrypt.HashPassword("password123"),
					ContactNumber = "1234567890",
					Address = "Haryana",
					DateCreated = System.DateTime.UtcNow
				},
				new User
				{
					UserId = 2,
					Role = "Employee",
					Name = "Sham Mohan",
					Email = "sham@gmail.com",
					Password = BCrypt.Net.BCrypt.HashPassword("password456"),
					ContactNumber = "0987654321",
					Address = "Patiyala",
					DateCreated = System.DateTime.UtcNow
				}
			});

			_context.SaveChanges();

			_mockConfig = new Mock<IConfiguration>();
			_mockConfig.Setup(config => config["Jwt:Key"]).Returns("your_jwt_secret_key");
			_mockConfig.Setup(config => config["Jwt:Issuer"]).Returns("test_issuer");
			_mockConfig.Setup(config => config["Jwt:Audience"]).Returns("test_audience");

			_controller = new UserController(_context, _mockConfig.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public async Task Register_ValidUser_ReturnsOk()
		{
			// Arrange
			var newUser = new UserDto
			{
				Role = "Employee",
				Name = "Test User",
				Email = "test.user@example.com",
				Password = "securepassword",
				ContactNumber = "1234567890",
				Address = "Punjab"
			};

			// Act
			var result = await _controller.Register(newUser);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreEqual(3, _context.Users.Count()); // Verify the user count increased
		}

		[Test]
		public async Task Register_DuplicateEmail_ReturnsConflict()
		{
			// Arrange
			var newUser = new UserDto
			{
				Role = "Employee",
				Name = "Ram Mohan", // Duplicate Name
				Email = "ram@gmail.com",
				Password = "password123",
				ContactNumber = "1234567890",
				Address = "Haryana"
			};

			// Act
			var result = await _controller.Register(newUser);

			// Assert
			Assert.IsInstanceOf<ConflictObjectResult>(result);
		}

		[Test]
		public async Task Login_InvalidCredentials_ReturnsUnauthorized()
		{
			// Arrange
			var loginDto = new LoginDto
			{
				Email = "ram@gmail.com",
				Password = "wrongpassword"
			};

			// Act
			var result = await _controller.Login(loginDto);

			// Assert
			Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
		}

		[Test]
		public async Task GetUserById_ValidId_ReturnsUser()
		{
			// Act
			var result = await _controller.GetUserById(1) as OkObjectResult;

			// Assert
			Assert.IsNotNull(result);
			var user = result.Value as User;
			Assert.AreEqual("Ram Mohan", user.Name);
		}

		[Test]
		public async Task GetUserById_InvalidId_ReturnsNotFound()
		{
			// Act
			var result = await _controller.GetUserById(999);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}

		[Test]
		public async Task UpdateUserById_ValidId_ReturnsOk()
		{
			// Arrange
			var updatedUserDto = new UserDto
			{
				Role = "Admin",
				Name = "Updated Name",
				Email = "ram@gmail.com",
				Password = "newpassword",
				ContactNumber = "1112223333",
				Address = "Updated Address"
			};

			// Act
			var result = await _controller.UpdateUserById(1, updatedUserDto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var updatedUser = _context.Users.Find(1);
			Assert.AreEqual("Updated Name", updatedUser.Name);
		}

		[Test]
		public async Task DeleteUserById_ValidId_ReturnsOk()
		{
			// Act
			var result = await _controller.DeleteUserById(1);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreEqual(1, _context.Users.Count()); // Verify user count decreased
		}

		[Test]
		public async Task DeleteUserById_InvalidId_ReturnsNotFound()
		{
			// Act
			var result = await _controller.DeleteUserById(999);

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(result);
		}
	}
}
