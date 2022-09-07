using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Models.Models.ersRefRequests;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eRS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorklistController : ControllerBase
{
    private readonly IWorklistService worklistService;
    private readonly ILogger<WorklistController> logger;

    public WorklistController(
        IWorklistService worklistService,
        ILogger<WorklistController> logger)
    {
        this.worklistService = worklistService;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RequestWorklist([FromBody] WorklistRequest request)
    {
        try
        {
            var worklist = await this.worklistService.GetWorklistFiltered(request);

            return worklist is null
                ? this.NotFound()
                : this.Ok(worklist);

        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        } 
    }

    [HttpGet("attachments/{refUid}")]
    public async Task<IActionResult> GetAttachments(string refUid)
    {
        try
        {
            var attachments = await this.worklistService.GetAttachments(refUid);

            return attachments is null
                ? this.NotFound()
                : this.Ok(attachments);

        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
    }

    [HttpGet("states")]
    public async Task<IActionResult> GetWorkflowStates()
    {
        try
        {
            var states = await this.worklistService.GetWorkflowStates();

            return states is null
                ? this.NotFound()
                : this.Ok(states);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
    }

    [HttpGet("history/{refUid}")]
    public async Task<IActionResult> GetWorkflowHistory(string refUid)
    {
        try
        {
            var history = await this.worklistService.GetWorkflowHistory(refUid, null);

            return history is null
                ? this.NotFound()
                : this.Ok(history);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
    }

    [HttpPost("history")]
    public async Task<IActionResult> AddToReferralWorkflowHistory([FromBody] WfsHistoryDto wfh)
    {
        try
        {
            var updatedWfsHistory = await this.worklistService.AddToWorkflowHistory(wfh);

            return updatedWfsHistory is null
                ? this.NotFound()
                : this.Ok(updatedWfsHistory);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
    }

    [HttpPut("history")]
    public async Task<IActionResult> UpdateReferralWorkflowHistory([FromBody] WfsHistoryDto wfh)
    {
        try
        {
            var updatedWfsHistory = await this.worklistService.UpdateWorkflowHistory(wfh);

            return updatedWfsHistory is null
                ? this.NotFound()
                : this.Ok(updatedWfsHistory);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message.ToString());
            return new BadRequestObjectResult(ex);
        }
    }
}
