using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Domain.Common.Errors;

using FluentAssertions;

using FluentValidation.Results;

namespace BuberDinner.UnitTess.Validations;

public class AuthValidationTests
{
    private ValidationResult RegisterCommandValidateModel(RegisterCommand command)
    {
        var validator = new RegisterCommandValidator();
        var validateResult = validator.Validate(command);

        return validateResult;
    }
    
    private ValidationResult LoginQueryValidateModel(LoginQuery query)
    {
        var validator = new LoginQueryValidator();
        var validateResult = validator.Validate(query);

        return validateResult;
    }

    [Theory]
    [MemberData(nameof(RegisterCommandInputModel))]
    public void When_RegisterCommand_Return_ValidateResult(RegisterCommand command, bool isValid, int numberOfErrors, string validationMessage)
    {
        // Arrange
        
        // Act
        var validateResult = RegisterCommandValidateModel(command);
        
        // Assert
        validateResult.IsValid.Should().Be(isValid);
        validateResult.Errors.Count.Should().Be(numberOfErrors);
        if (numberOfErrors > 0)
        {
            var errors = string.Join(" ", validateResult.Errors.Select(error => error.ErrorMessage).ToList());
            errors.Should().BeEquivalentTo(validationMessage);
        }
    }

    [Theory]
    [MemberData(nameof(LoginQueryInputModel))]
    public void When_LoginQuery_Return_ValidateResult(LoginQuery query, bool isValid, int numberOfErrors, string validationMessage)
    {
        // Arrange

        // Act
        var validateResult = LoginQueryValidateModel(query);

        // Assert
        validateResult.IsValid.Should().Be(isValid);
        validateResult.Errors.Count.Should().Be(numberOfErrors);
        if (numberOfErrors > 0)
        {
            var errors = string.Join(" ", validateResult.Errors.Select(error => error.ErrorMessage).ToList());
            errors.Should().BeEquivalentTo(validationMessage);
        }
    }
    public static IEnumerable<object[]> RegisterCommandInputModel =>
        new List<object[]>
        {
            new object[]
            {
                new RegisterCommand("William", "Chen", "william.chen@email.address", "Passw0rd"), true, 0,
                string.Empty
            },
            new object[]
            {
                new RegisterCommand("Will", "Smith", "will.smith@email.address", ""),
                false, 1, "'Password' must not be empty."
            },
            new object[]
            {
                new RegisterCommand("Tom", "", "tom@email.address", "Password"), false, 1, "'Last Name' must not be empty."
            },
            new object[]
            {
                new RegisterCommand("", "Hanks", "hanks@email.address", "Password"), false, 1, "'First Name' must not be empty."
            },
            new object[]
            {
                new RegisterCommand("Tom", "Hanks", "", "Password"), false, 1, "'Email' must not be empty."
            },
            new object[]
            {
                new RegisterCommand("", "", "tom@email.address", "Password"), false, 2, "'First Name' must not be empty. 'Last Name' must not be empty."
            },
        };

    public static IEnumerable<object[]> LoginQueryInputModel =>
        new List<object[]>
        {
            new object[] { new LoginQuery("william.chen@email.address", "Passw0rd"), true, 0, string.Empty },
            new object[] { new LoginQuery("", "Passw0rd"), false, 1, "'Email' must not be empty." },
            new object[]
            {
                new LoginQuery("william.chen@email.address", ""), false, 1, "'Password' must not be empty."
            },
            new object[]
            {
                new LoginQuery("", ""), false, 2,
                "'Email' must not be empty. 'Password' must not be empty."
            }
        };
}