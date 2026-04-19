using FluentValidation.TestHelper;
using UltimateERP.Application.Features.Auth.Commands;

namespace UltimateERP.Tests.Auth;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCredentials_NoErrors()
    {
        var command = new LoginCommand("admin", "Password123!");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyUsername_HasError()
    {
        var command = new LoginCommand("", "Password123!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_EmptyPassword_HasError()
    {
        var command = new LoginCommand("admin", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ShortPassword_HasError()
    {
        var command = new LoginCommand("admin", "ab");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}

public class ChangePasswordValidatorTests
{
    private readonly ChangePasswordValidator _validator = new();

    [Fact]
    public void Validate_StrongPassword_NoErrors()
    {
        var command = new ChangePasswordCommand("OldPass1!", "NewPass1!");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WeakPassword_HasErrors()
    {
        var command = new ChangePasswordCommand("OldPass1!", "weak");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Validate_NoUppercase_HasError()
    {
        var command = new ChangePasswordCommand("OldPass1!", "lowercase1!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Validate_NoDigit_HasError()
    {
        var command = new ChangePasswordCommand("OldPass1!", "NoDigitHere!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Validate_NoSpecialChar_HasError()
    {
        var command = new ChangePasswordCommand("OldPass1!", "NoSpecial1A");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }
}
