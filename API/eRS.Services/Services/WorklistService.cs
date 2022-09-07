using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eRS.Services.Services;

public class WorklistService : IWorklistService
{
    private readonly eRSContext context;
    private readonly IMapper mapper;

    public WorklistService(eRSContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PagedResult<ErsRefReqDetailDto>> GetWorklistFiltered(WorklistRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var filters = request.Filters;
        var worklistQuery = this.context.ErsRefReqDetails
            .Where(r => r.RecStatus != "D")
            .Include(r => r.WfsHistoryList)
            .OrderBy(r => r.RecUpdated)
            .AsQueryable();

        worklistQuery.Where(r => !string.IsNullOrWhiteSpace(r.RefReqStatus) && EF.Functions.Like(r.RefReqStatus, $"%{filters.RefReqStatus}%"));

        if (filters.ApptDttmFrom != null)
        {
            worklistQuery = worklistQuery.Where(r => r.ApptStDttm > filters.ApptDttmFrom);
        }

        if (filters.ApptDttmFrom != null)
        {
            worklistQuery = worklistQuery.Where(r => r.ApptEndDttm < filters.ApptDttmFrom);
        }

        if (filters.RecInsertedFrom != null)
        {
            worklistQuery = worklistQuery.Where(r => r.RecUpdated > filters.RecInsertedFrom);
        }

        if (filters.RecInsertedTo != null)
        {
            worklistQuery = worklistQuery.Where(r => r.RecUpdated < filters.RecInsertedTo);
        }

        /*
        if (filters.MeditechPathway != null && filters.MeditechPathway.Any())
        {
            worklistQuery = worklistQuery.Where(r => r.RefReqPathway != null && filters.MeditechPathway.Contains(r.RefReqPathway));
        }*/

        if (filters.RefReqSpecialty != null && filters.RefReqSpecialty.Any())
        {
            worklistQuery = worklistQuery.Where(r => r.RefReqSpecialty != null && filters.RefReqSpecialty.Contains(r.RefReqSpecialty));
        }

        if (filters.ErsService != null)
        {
            //worklistQuery = worklistQuery.Where(r => r.RecInserted < filters.FilterByRecInsertedTo);
        }

        var pageSize = 10;
        var result = new PagedResult<ErsRefReqDetailDto> { CurrentPage = request.PageNumber, PageSize = pageSize, RowCount = worklistQuery.Count() };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (request.PageNumber - 1) * pageSize;

        var resultItems = await worklistQuery.Skip(skip).Take(pageSize).ToListAsync();

        result.Results = this.mapper.Map<List<ErsRefReqDetailDto>>(resultItems);

        return result;
    }

    public async Task<List<ErsdocAttachmentDto>> GetAttachments(string refUid)
    {
        var attachments = await this.context.ErsdocAttachments
            .Where(a => a.RefDocUniqueId == refUid)
            .Include(a => a.WfsHistoryList)
            .OrderBy(a => a.RefDocSrlno)
            .ToListAsync();

        return this.mapper.Map<List<ErsdocAttachmentDto>>(attachments);
    }

    public async Task<List<WfsMasterDto>> GetWorkflowStates()
    {
        var wfs = await this.context.WfsMasters.ToListAsync();

        return this.mapper.Map<List<WfsMasterDto>>(wfs);
    }

    public async Task<List<WfsHistoryDto>> GetWorkflowHistory(string? refUid, string? docUid)
    {
        var wfh = await this.context.WfsHistories
            .Where(h => (refUid != null && h.ErstrnsUid == refUid) || (docUid != null && h.DoctrnsUid == docUid))
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
            existingHistory.StatusCode = newHistory.StatusCode;
            existingHistory.StatusComments = newHistory.StatusComments;

            this.context.WfsHistories.Update(existingHistory);

            if (await this.context.SaveChangesAsync() == 0)
            {
                return null;
            }

            return await GetWorkflowHistory(newHistory.ErstrnsUid, newHistory.DoctrnsUid);
        }

        newHistory.RecInserted = DateTime.UtcNow;

        var history = this.mapper.Map<WfsHistory>(newHistory);

        //Link to ref and/or attachment doc
        if (history.ErstrnsUid is not null)
        {
            var refReq = await this.context.ErsRefReqDetails.FirstOrDefaultAsync(r => r.RefReqUniqueId == history.ErstrnsUid);
            
            if (refReq is null)
            {
                return null;
            }

            history.ErsRefReqDetail = refReq;
            history.RefReqRowId = refReq.RefReqRowId;
        }

        if (history.DoctrnsUid is not null)
        {
            var doc = await this.context.ErsdocAttachments.FirstOrDefaultAsync(r => r.AttachId == history.DoctrnsUid);

            if (doc is null)
            {
                return null;
            }

            history.ErsdocAttachment = doc;
            history.RefDocRowId = doc.RefDocRowId;
        }

        this.context.WfsHistories.Add(history);

        return await this.context.SaveChangesAsync() == 0
            ? null
            : await GetWorkflowHistory(history.ErstrnsUid, history.DoctrnsUid);
    }

    public async Task<List<WfsHistoryDto>?> UpdateWorkflowHistory(WfsHistoryDto updatedHistory)
    {
        var history = await this.context.WfsHistories
            .FirstOrDefaultAsync(h =>
            h.ErstrnsUid == updatedHistory.ErstrnsUid &&
            h.DoctrnsUid == updatedHistory.DoctrnsUid);

        if (history is null)
        {
            return null;
        }

        history.StatusCode = updatedHistory.StatusCode;
        history.StatusComments = updatedHistory.StatusComments;

        var result = await this.context.SaveChangesAsync() == 0;

        return result
            ? null
            : await GetWorkflowHistory(history.ErstrnsUid, history.DoctrnsUid);
    }

} 