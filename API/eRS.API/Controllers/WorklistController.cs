using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Models.Models.Files;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace eRS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class WorklistController : ControllerBase
{
    private readonly IWorklistService worklistService;
    private readonly ILogger<WorklistController> logger;
    private readonly IHttpClientFactory httpClientFactory;

    public WorklistController(
        IWorklistService worklistService,
        ILogger<WorklistController> logger,
        IHttpClientFactory httpClientFactory)
    {
        this.worklistService = worklistService;
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
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
            var updatedWfsHistory = await this.worklistService.UpdateWorkflowHistory(wfh, null);

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

    [HttpGet("zip/{refUId}")]
    public async Task<IActionResult> DownloadAttachments(string refUid)
    {
        var attachmentFiles = await this.worklistService.GetAttachmentURLs(refUid);
        var httpClient = this.httpClientFactory.CreateClient();

        using (var zipFileMemoryStream = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
            {
                foreach (var attach in attachmentFiles)
                {
                    if (attach.AttachDownloadURL is null || attach.AttachFileName is null)
                    {
                        continue;
                    }

                    var file = await httpClient.GetAsync(attach.AttachDownloadURL);

                    var entry = archive.CreateEntry(attach.AttachFileName);
                    using (var entryStream = entry.Open())
                    using (var fileStream = System.IO.File.OpenRead(attach.AttachDownloadURL))
                    {
                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }

            zipFileMemoryStream.Seek(0, SeekOrigin.Begin);

            var zipFile = File(zipFileMemoryStream.ToArray(), "application/octet-stream");

            return this.Ok(zipFile);
        }
        
    }
}
