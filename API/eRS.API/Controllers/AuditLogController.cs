using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogController : ControllerBase
{
    private readonly IAuditService auditService;
    private readonly ILogger<AuditLogController> logger;

    public AuditLogController(
        IAuditService auditService,
        ILogger<AuditLogController> logger)
    {
        this.auditService = auditService;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RequestAudits([FromBody] AuditRequest request)
    {
        try
        {
            var audits = await this.auditService.GetAllFiltered(request);

            return audits is null
                ? this.NotFound()
                : this.Ok(audits);

        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
            
    }
}
