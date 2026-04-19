using FluentValidation.TestHelper;
using UltimateERP.Application.Features.Setup.Commands;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Features.HR.Commands;
using UltimateERP.Application.Features.HR.DTOs;

namespace UltimateERP.Tests.Application;

public class SetupValidationTests
{
    private readonly CreateBranchValidator _branchValidator = new();

    [Fact]
    public void CreateBranch_ValidInput_NoErrors()
    {
        var command = new CreateBranchCommand(new CreateBranchDto { Code = "BR01", Name = "Main Branch" });
        var result = _branchValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateBranch_EmptyCode_HasError()
    {
        var command = new CreateBranchCommand(new CreateBranchDto { Code = "", Name = "Main Branch" });
        var result = _branchValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Branch.Code);
    }

    [Fact]
    public void CreateBranch_EmptyName_HasError()
    {
        var command = new CreateBranchCommand(new CreateBranchDto { Code = "BR01", Name = "" });
        var result = _branchValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Branch.Name);
    }

    [Fact]
    public void CreateBranch_CodeTooLong_HasError()
    {
        var command = new CreateBranchCommand(new CreateBranchDto { Code = new string('A', 25), Name = "Branch" });
        var result = _branchValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Branch.Code);
    }
}

public class HRValidationTests
{
    private readonly CreateEmployeeValidator _employeeValidator = new();

    [Fact]
    public void CreateEmployee_ValidInput_NoErrors()
    {
        var command = new CreateEmployeeCommand(new CreateEmployeeDto
        {
            EmployeeCode = "EMP01",
            FirstName = "Ram",
            LastName = "Sharma"
        });
        var result = _employeeValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateEmployee_EmptyCode_HasError()
    {
        var command = new CreateEmployeeCommand(new CreateEmployeeDto
        {
            EmployeeCode = "",
            FirstName = "Ram",
            LastName = "Sharma"
        });
        var result = _employeeValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Employee.EmployeeCode);
    }

    [Fact]
    public void CreateEmployee_EmptyFirstName_HasError()
    {
        var command = new CreateEmployeeCommand(new CreateEmployeeDto
        {
            EmployeeCode = "EMP01",
            FirstName = "",
            LastName = "Sharma"
        });
        var result = _employeeValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Employee.FirstName);
    }

    [Fact]
    public void CreateEmployee_EmptyLastName_HasError()
    {
        var command = new CreateEmployeeCommand(new CreateEmployeeDto
        {
            EmployeeCode = "EMP01",
            FirstName = "Ram",
            LastName = ""
        });
        var result = _employeeValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Employee.LastName);
    }
}
