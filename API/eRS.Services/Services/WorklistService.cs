using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Models.Models.Files;
using eRS.Models.Models.Wfs;
using eRS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eRS.Services.Services;

public class WorklistService : IWorklistService
{
    private readonly eRSContext context;
    private readonly ILogger<WorklistService> logger;
    private readonly IMapper mapper;
    private readonly IAuditService auditService;

    public WorklistService(eRSContext context, IMapper mapper, ILogger<WorklistService> logger, IAuditService auditService)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
        this.auditService = auditService;
    }

    public async Task<PagedResult<ErsRefReqDetailDto>> GetWorklistFiltered(WorklistRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var worklistQuery = GetWorklistFilteredQuery(request.Filters);

        var pageSize = 10;
        var result = new PagedResult<ErsRefReqDetailDto> { CurrentPage = request.PageNumber, PageSize = pageSize, RowCount = worklistQuery.Count() };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (request.PageNumber - 1) * pageSize;

        var resultItems = await worklistQuery.Skip(skip).Take(pageSize).ToListAsync();
        result.Results = this.mapper.Map<List<ErsRefReqDetailDto>>(resultItems);

        return result;
    }

    private IQueryable<ErsRefReqDetail> GetWorklistFilteredQuery(WorklistFilter filters)
    {
        var worklistQuery = this.context.ErsRefReqDetails
            .Where(r => r.RecStatus != "D")
            .Include(r => r.WfsHistoryList)
            .Include(r => r.Patient)
            .Include(r => r.ErsdocAttachments)
            .ThenInclude(a => a.WfsHistoryList)
            //.Where(r => !r.WfsHistoryList.Any(h => h.StatusCode == "R-ULEPR-SUCC")) //Remove fully uploaded refreqs from the list
            .OrderBy(r => r.RecUpdated)
            .OrderBy(r => r.WfsHistoryList.Any(h => h.StatusCode == "R-ULEPR-SUCC")) //Fully uploaded refreqs at the end of the list
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.RefReqStatus))
        {
            worklistQuery = worklistQuery.Where(r =>
                !string.IsNullOrWhiteSpace(r.RefReqStatus) && EF.Functions.Like(r.RefReqStatus, $"%{filters.RefReqStatus}%"));
        }

        if (!string.IsNullOrWhiteSpace(filters.RefReqSpecialty))
        {
            worklistQuery = worklistQuery.Where(r =>
                !string.IsNullOrWhiteSpace(r.RefReqSpecialty) && EF.Functions.Like(r.RefReqSpecialty, $"%{filters.RefReqSpecialty}%"));
        }

        if (!string.IsNullOrWhiteSpace(filters.RefReqUbrn))
        {
            worklistQuery = worklistQuery.Where(r =>
                !string.IsNullOrWhiteSpace(r.RefReqUbrn) && EF.Functions.Like(r.RefReqUbrn, $"%{filters.RefReqUbrn}%"));
        }

        if (filters.InvestigationMode is not null && filters.InvestigationMode == true)
        {
            worklistQuery = worklistQuery.Where(r => r.WfsHistoryList.Any(h => h.StatusCode != null && h.StatusCode.Contains("FAIL")));
        }

        if (filters.ApptDttmFrom is not null)
        {
            worklistQuery = worklistQuery.Where(r => r.ApptStDttm >= filters.ApptDttmFrom);
        }

        if (filters.ApptDttmTo is not null)
        {
            worklistQuery = worklistQuery.Where(r => r.ApptEndDttm <= filters.ApptDttmTo);
        }

        if (filters.RecInsertedFrom is not null)
        {
            worklistQuery = worklistQuery.Where(r => r.RecUpdated >= filters.RecInsertedFrom);
        }

        if (filters.RecInsertedTo is not null)
        {
            worklistQuery = worklistQuery.Where(r => r.RecUpdated <= filters.RecInsertedTo);
        }

        /*
        if (filters.MeditechPathway is not null && filters.MeditechPathway.Any())
        {
            worklistQuery = worklistQuery.Where(r => r.RefReqPathway != null && filters.MeditechPathway.Contains(r.RefReqPathway));
        }
        if (filters.ErsService is not null)
        {
            worklistQuery = worklistQuery.Where(r => r.RecInserted < filters.FilterByRecInsertedTo);
        }
        */

        return worklistQuery;
    }

    public async Task<List<ErsdocAttachmentDto>> GetAttachments(string refUid)
    {
        var attachments = await this.context.ErsdocAttachments
            .Where(a => a.RefDocUniqueId == refUid)
            .Include(a => a.WfsHistoryList)
            .OrderBy(a => a.RefDocSrlno)
            .ToListAsync();

        //For each attachment, get the most recent one not marked as PreviouslyDownloadedDoc
        var groupedAttachments = attachments.GroupBy(a => a.AttachId);
        var filteredAttachments = new List<ErsdocAttachment>();

        foreach(var attachId in groupedAttachments)
        {
            var newestValidAttach = attachId.FirstOrDefault(a => a.PreviouslyDownloadedDoc == null || a.PreviouslyDownloadedDoc != "N");

            if (newestValidAttach is not null)
            {
                filteredAttachments.Add(newestValidAttach);
            }
        }

        return this.mapper.Map<List<ErsdocAttachmentDto>>(filteredAttachments);
    }

    public async Task<WfsMasterResponse> GetWorkflowStates()
    {
        var wfs = await this.context.WfsMasters.OrderBy(w => w.WfsmHierarchy).ToListAsync();

        var refReqStates = this.mapper.Map<List<WfsMasterDto>>(wfs.Where(s => s.WfsmCode[0] == 'R'));
        var refDocStates = this.mapper.Map<List<WfsMasterDto>>(wfs.Where(s => s.WfsmCode[0] == 'D'));

        var wfsResponse = new WfsMasterResponse
        {
            RefReqStates = refReqStates,
            RefDocStates = refDocStates
        };

        return wfsResponse;
    }

    public async Task<List<WfsHistoryDto>> GetWorkflowHistory(string? refUid, string? docUid)
    {
        var wfh = await this.context.WfsHistories
            .Where(h => (refUid != null && h.ErstrnsUid == refUid) || (docUid != null && h.DoctrnsUid == docUid))
            .Include(h => h.WfsMaster)
            .Include(h => h.ErsdocAttachment)
            .OrderBy(h => h.RecInserted)
            .ToListAsync();

        return this.mapper.Map<List<WfsHistoryDto>>(wfh);
    }

    public async Task<List<WfsHistoryDto>?> AddToWorkflowHistory(WfsHistoryDto newHistory)
    {
        var existingHistory = await this.context.WfsHistories
            .FirstOrDefaultAsync(h =>
            h.ErstrnsUid == newHistory.ErstrnsUid &&
            h.DoctrnsUid == newHistory.DoctrnsUid);

        if (existingHistory is not null)
        {
            return await this.UpdateWorkflowHistory(newHistory, existingHistory);
        }

        var now = DateTime.UtcNow;
        newHistory.RecInserted = now;
        newHistory.RecUpdated = now;

        var history = this.mapper.Map<WfsHistory>(newHistory);

        if (history.ErstrnsUid is null)
        {
            throw new ArgumentNullException(nameof(history.ErstrnsUid));
        }

        var refReq = await this.context.ErsRefReqDetails.FirstOrDefaultAsync(r => r.RefReqUniqueId == history.ErstrnsUid);
            
        if (refReq is null)
        {
            return null;
        }

        history.RefReqRowId = refReq.RefReqRowId;

        if (history.DoctrnsUid is not null)
        {
            var doc = await this.context.ErsdocAttachments
                .FirstOrDefaultAsync(r => r.AttachId == history.DoctrnsUid && r.RefDocUniqueId == history.ErstrnsUid);

            if (doc is null)
            {
                return null;
            }

            history.RefDocRowId = doc.RefDocRowId;
        }

        await this.context.WfsHistories.AddAsync(history);
        await this.context.SaveChangesAsync();

        var audit = new Auditlog
        {
            ToEventCode = history.StatusCode,
            ToStatusComments = history.StatusComments,
            ErstrnsUid = history.ErstrnsUid,
            DoctrnsUid = history.DoctrnsUid,
            RecInsertedBy = history.RecInsertedBy
        };

        if (!await this.auditService.AddAudit(audit))
        {
            this.logger.LogWarning(
                $"Audit failed to log for event transition from No Status to ${audit.ToEventCode} in referral ${audit.ErstrnsUid} or attachment ${audit.DoctrnsUid}"
            );
        }

        return await GetWorkflowHistory(history.ErstrnsUid, history.DoctrnsUid);
    }

    public async Task<List<WfsHistoryDto>?> UpdateWorkflowHistory(WfsHistoryDto newHistory, WfsHistory? oldHistory)
    {
        var existingHistory = oldHistory ??
            await this.context.WfsHistories
                .FirstOrDefaultAsync(h =>
                h.ErstrnsUid == newHistory.ErstrnsUid &&
                h.DoctrnsUid == newHistory.DoctrnsUid);

        if (existingHistory is null)
        {
            return null;
        }

        var audit = new Auditlog
        {
            FromEventCode = existingHistory.StatusCode,
            FromStatusComments = existingHistory.StatusComments,
            ToEventCode = newHistory.StatusCode,
            ToStatusComments = newHistory.StatusComments,
            ErstrnsUid = newHistory.ErstrnsUid,
            DoctrnsUid = newHistory.DoctrnsUid,
            RecInsertedBy = newHistory.RecInsertedBy
        };

        existingHistory.RecUpdated = DateTime.UtcNow;
        existingHistory.StatusCode = newHistory.StatusCode;
        existingHistory.StatusComments = newHistory.StatusComments;

        this.context.WfsHistories.Update(existingHistory);

        await this.context.SaveChangesAsync();

        if (!await this.auditService.AddAudit(audit))
        {
            this.logger.LogWarning(
                $"Audit failed to log for event transition from ${audit.FromEventCode} to ${audit.ToEventCode} in referral ${audit.ErstrnsUid} or attachment ${audit.DoctrnsUid}"
            );
        }

        return await GetWorkflowHistory(newHistory.ErstrnsUid, newHistory.DoctrnsUid);
    }

    public async Task<List<DownloadFile>> GetAttachmentURLs(string refUid)
    {
        var attachments = await this.context.ErsdocAttachments
            .Where(a => a.RefDocUniqueId == refUid)
            .Select(a => new DownloadFile
            {
                AttachDownloadURL = a.DocDownloadUrl,
                AttachFileName = a.AttachTitle
            })
            .ToListAsync();

        return attachments;
    }

}