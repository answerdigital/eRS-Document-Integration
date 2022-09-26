using eRS.Models.Models;
using eRS.Models.Models.Audits;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eRS.API.Controllers;

[Authorize]
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
            var audits = await this.auditService.GetAllFilteredPaged(request);

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

    [HttpPost("export")]
    public async Task<IActionResult> ExportCsv([FromBody] AuditRequest request)
    {
        var audits = await this.auditService.GetAllFiltered(request);

        if (audits is null)
        {
            return this.NotFound();
        }

        var csv = await this.auditService.GenerateCsv(audits);

        if (csv is null)
        {
            return this.NotFound();
        }

        string fileName = $"auditlog_generated_{DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss")}.csv";
        var file = new CsvFile(csv, fileName);

        return this.Ok(file);
    }
}
