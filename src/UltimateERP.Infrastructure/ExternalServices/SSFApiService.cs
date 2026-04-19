using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.ExternalServices;

public class SSFApiService : ISSFApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SSFApiService> _logger;
    private readonly string _baseUrl;
    private readonly string _employerCode;

    public SSFApiService(HttpClient httpClient, ILogger<SSFApiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["SSF:BaseUrl"] ?? "https://ssf-api.example.com";
        _employerCode = configuration["SSF:EmployerCode"] ?? string.Empty;
    }

    public async Task<SSFRegistrationResult> RegisterEmployeeAsync(SSFEmployeeDto employee)
    {
        try
        {
            _logger.LogInformation("Registering employee {Name} with SSF", employee.EmployeeName);

            var payload = new
            {
                employerCode = _employerCode,
                employeeName = employee.EmployeeName,
                panNumber = employee.PanNumber,
                dateOfBirth = employee.DateOfBirth,
                joinDate = employee.JoinDate,
                gender = employee.Gender,
                basicSalary = employee.BasicSalary
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/register", payload);

            if (response.IsSuccessStatusCode)
            {
                var ssfNumber = $"SSF-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
                _logger.LogInformation("SSF registration successful: {SSFNumber}", ssfNumber);

                return new SSFRegistrationResult
                {
                    IsSuccess = true,
                    SSFNumber = ssfNumber
                };
            }

            return new SSFRegistrationResult
            {
                IsSuccess = false,
                ErrorMessage = "SSF registration failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SSF registration error for {Name}", employee.EmployeeName);
            return new SSFRegistrationResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<SSFContributionResult> SubmitContributionAsync(SSFContributionDto contribution)
    {
        try
        {
            _logger.LogInformation("Submitting SSF contribution for {SSFNumber}: {Month}/{Year}",
                contribution.SSFNumber, contribution.ContributionMonth, contribution.ContributionYear);

            var payload = new
            {
                employerCode = _employerCode,
                ssfNumber = contribution.SSFNumber,
                employeeContribution = contribution.EmployeeContribution,
                employerContribution = contribution.EmployerContribution,
                contributionMonth = contribution.ContributionMonth,
                contributionYear = contribution.ContributionYear
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/contribute", payload);

            if (response.IsSuccessStatusCode)
            {
                var refNumber = $"SSFCONT-{DateTime.UtcNow:yyyyMMddHHmmss}";
                _logger.LogInformation("SSF contribution submitted: {ReferenceNumber}", refNumber);

                return new SSFContributionResult
                {
                    IsSuccess = true,
                    ReferenceNumber = refNumber
                };
            }

            return new SSFContributionResult
            {
                IsSuccess = false,
                ErrorMessage = "SSF contribution submission failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SSF contribution error for {SSFNumber}", contribution.SSFNumber);
            return new SSFContributionResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
