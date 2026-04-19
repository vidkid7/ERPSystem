using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.API.Controllers.Security;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AuditLogDto>>>> GetLogs([FromQuery] AuditLogFilterDto filter)
    {
        var logs = await _auditLogService.GetLogsAsync(filter);
        return Ok(ApiResponse<List<AuditLogDto>>.Success(logs, "Audit logs retrieved", logs.Count));
    }
}
