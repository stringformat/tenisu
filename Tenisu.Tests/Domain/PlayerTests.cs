using FluentAssertions;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Tests.Domain;

public class PlayerTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnSuccessWithPlayer()
    {
        // Arrange
        var expectedFirstname = "Rafael";
        var expectedLastname = "Nadal";
        var expectedAge = 37;
        var expectedGender = Gender.M;
        var expectedPicture = new Uri("https://example.com/nadal.jpg");
        var expectedHeight = 185;
        var expectedWeight = 85;
        var expectedCountryCode = "ESP";

        // Act
        var result = Player.Create(
            expectedFirstname, 
            expectedLastname, 
            expectedAge, 
            expectedGender, 
            expectedPicture, 
            expectedHeight, 
            expectedWeight, 
            expectedCountryCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.FirstName.Should().Be(expectedFirstname);
        result.Value.LastName.Should().Be(expectedLastname);
        result.Value.Age.Should().Be(expectedAge);
        result.Value.Gender.Should().Be(expectedGender);
        result.Value.Picture.Should().Be(expectedPicture);
        result.Value.Height.Should().Be(expectedHeight);
        result.Value.Weight.Should().Be(expectedWeight);
        result.Value.Country.Value.Should().Be(expectedCountryCode);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueId()
    {
        // Arrange
        var expectedFirstname = "Rafael";
        var expectedLastname = "Nadal";
        var expectedAge = 37;
        var expectedGender = Gender.M;
        var expectedPicture = new Uri("https://example.com/nadal.jpg");
        var expectedHeight = 185;
        var expectedWeight = 85;
        var expectedCountryCode = "ESP";

        // Act
        var player1 = Player.Create(
            expectedFirstname, 
            expectedLastname, 
            expectedAge, 
            expectedGender, 
            expectedPicture, 
            expectedHeight, 
            expectedWeight, 
            expectedCountryCode);
        
        var player2 = Player.Create(
            expectedFirstname, 
            expectedLastname, 
            expectedAge, 
            expectedGender, 
            expectedPicture, 
            expectedHeight, 
            expectedWeight, 
            expectedCountryCode);
        
        // Assert
        player1.IsSuccess.Should().BeTrue();
        player2.IsSuccess.Should().BeTrue();
        player1.Value.Id.Should().NotBe(player2.Value.Id);
        player1.Value.Id.Value.Should().NotBe(Guid.Empty);
        player2.Value.Id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithInvalidCountryCode_ShouldReturnCodeIncorrectError()
    {
        // Arrange
        var expectedFirstname = "Rafael";
        var expectedLastname = "Nadal";
        var expectedAge = 37;
        var expectedGender = Gender.M;
        var expectedPicture = new Uri("https://example.com/nadal.jpg");
        var expectedHeight = 185;
        var expectedWeight = 85;

        // Act
        var player1 = Player.Create(
            expectedFirstname, 
            expectedLastname, 
            expectedAge, 
            expectedGender, 
            expectedPicture, 
            expectedHeight, 
            expectedWeight,
            countryCode: "AAAA");
        
        var player2 = Player.Create(
            expectedFirstname, 
            expectedLastname, 
            expectedAge, 
            expectedGender, 
            expectedPicture, 
            expectedHeight, 
            expectedWeight, 
            countryCode: "");

        // Assert
        player1.IsFail.Should().BeTrue();
        player1.Error.Should().Be(PlayerErrors.CountryCodeIncorrect);
        player2.IsFail.Should().BeTrue();
        player2.Error.Should().Be(PlayerErrors.CountryCodeIncorrect);
    }
}