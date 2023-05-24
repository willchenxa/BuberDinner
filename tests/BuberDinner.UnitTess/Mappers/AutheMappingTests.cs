using Mapster;
using MapsterMapper;
using BuberDinner.Api.Mapping;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Contracts.Menus;
using BuberDinner.Domain.User;

using FluentAssertions;

namespace BuberDinner.UnitTess.Mappers;

public class AuthMappingTests
{
    private readonly IMapper _mapper;

    public AuthMappingTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(AuthenticationMappingConfig).Assembly);
        _mapper = new Mapper(config);
    }

    [Fact]
    public void MenuMapper_Should_Success()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AuthenticationResult(new User
        {
            Email = "email@address",
            FirstName = "FirstName",
            Id = id,
            LastName = "LastName",
            Password = "Password"
        }, "token");

        // Act
        var response = _mapper.Map<AuthenticationResponse>(request);

        // Assert
        response.Should().NotBeNull();
        response.Email.Should().BeEquivalentTo("email@address");
        response.FirstName.Should().BeEquivalentTo("FirstName");
        response.LastName.Should().BeEquivalentTo("LastName");
        response.Id.ToString().Should().BeEquivalentTo(id.ToString());
        response.Token.Should().BeEquivalentTo("token");
    }
}