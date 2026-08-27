using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RegistroCreditos.Api.Data;
using RegistroCreditos.Api.Models;
using RegistroCreditos.Api.Services;

namespace RegistroCreditos.Tests.Services;

public class AuthServiceTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _jwtServiceMock = new Mock<IJwtService>();

        _authService = new AuthService(_dbContext, _jwtServiceMock.Object);
    }

    [Fact]
    [Trait("Category", "positive")]
    [Trait("Category", "security")]
    public void HashPassword_Should_Return_Hashed_Password()
    {
        // Arrange
        var password = "SecurePassword123!";

        // Act
        var hash = _authService.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().NotBe(password);
        _authService.VerifyPassword(password, hash).Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "negative")]
    [Trait("Category", "security")]
    public async Task LoginAsync_Should_Return_Null_If_User_Not_Found()
    {
        // Act
        var result = await _authService.LoginAsync("nonexistent@example.com", "anypassword");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "negative")]
    [Trait("Category", "security")]
    public async Task LoginAsync_Should_Return_Null_If_Password_Is_Incorrect()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Test",
            Email = "test@example.com",
            PasswordHash = _authService.HashPassword("CorrectPassword")
        };
        _dbContext.Usuarios.Add(usuario);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _authService.LoginAsync(usuario.Email, "WrongPassword");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Category", "positive")]
    [Trait("Category", "security")]
    [Trait("Category", "critical-path")]
    public async Task LoginAsync_Should_Return_Token_If_Credentials_Are_Correct()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Test",
            Email = "test@example.com",
            PasswordHash = _authService.HashPassword("CorrectPassword")
        };
        _dbContext.Usuarios.Add(usuario);
        await _dbContext.SaveChangesAsync();

        var expectedToken = "fake-jwt-token";
        _jwtServiceMock.Setup(j => j.GenerateToken(It.Is<Usuario>(u => u.Email == usuario.Email)))
                       .Returns(expectedToken);

        // Act
        var result = await _authService.LoginAsync(usuario.Email, "CorrectPassword");

        // Assert
        result.Should().Be(expectedToken);
        _jwtServiceMock.Verify(j => j.GenerateToken(It.IsAny<Usuario>()), Times.Once);
    }
}


