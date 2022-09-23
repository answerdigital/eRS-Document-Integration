using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eRS.Services.Services;

public class AuditService : IAuditService
{
    private readonly eRSContext context;
    private readonly IMapper mapper;

    public AuditService(eRSContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PagedResult<AuditlogDto>> GetAllFiltered(AuditRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var filters = request.Filters;
        var auditsQuery = this.context.Auditlogs
            .Where(a => a.RecStatus != "D")
            .Include(a => a.ErsRefReqDetail)
            .ThenInclude(r => r.Patient)
            .Include(a => a.ErsdocAttachment)
            .OrderByDescending(a => a.RecInserted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.EventCode))
        {
            auditsQuery = auditsQuery.Where(a => !string.IsNullOrWhiteSpace(a.ToEventCode) && EF.Functions.Like(a.ToEventCode, $"%{filters.EventCode}%"));
        }

        if (!string.IsNullOrWhiteSpace(filters.NhsNo))
        {
            auditsQuery = auditsQuery.Where(a =>
            (!string.IsNullOrWhiteSpace(a.ErsRefReqDetail.RefReqNhsno) && EF.Functions.Like(a.ErsRefReqDetail.RefReqNhsno, $"%{filters.NhsNo}%")) ||
            (
                a.ErsRefReqDetail.Patient != null &&
                !string.IsNullOrWhiteSpace(a.ErsRefReqDetail.Patient.PatFullName) &&
                EF.Functions.Like(a.ErsRefReqDetail.Patient.PatFullName, $"%{filters.NhsNo}%"))
            );
        }

        if (!string.IsNullOrWhiteSpace(filters.RefReqUid))
        {
            auditsQuery = auditsQuery.Where(a => !string.IsNullOrWhiteSpace(a.ErstrnsUid) && EF.Functions.Like(a.ErstrnsUid, $"%{filters.RefReqUid}%"));
        }

        if (!string.IsNullOrWhiteSpace(filters.RefDocUid))
        {
            auditsQuery = auditsQuery.Where(a => !string.IsNullOrWhiteSpace(a.DoctrnsUid) && EF.Functions.Like(a.DoctrnsUid, $"%{filters.RefDocUid}%"));
        }

        if (!string.IsNullOrWhiteSpace(filters.RecInsertedBy))
        {
            auditsQuery = auditsQuery.Where(a => !string.IsNullOrWhiteSpace(a.RecInsertedBy) && EF.Functions.Like(a.RecInsertedBy, $"%{filters.RecInsertedBy}%"));
        }

        if (filters.RecInsertedFrom != null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted >= filters.RecInsertedFrom);
        }

        if (filters.RecInsertedTo != null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted <= filters.RecInsertedTo);
        }

        var pageSize = 10;
        var result = new PagedResult<AuditlogDto> { CurrentPage = request.PageNumber, PageSize = pageSize, RowCount = auditsQuery.Count() };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (request.PageNumber - 1) * pageSize;

        var resultItems = await auditsQuery.Skip(skip).Take(pageSize).ToListAsync();

        result.Results = this.mapper.Map<List<AuditlogDto>>(resultItems);

        return result;
    }

    public async Task<bool> AddAudit(AuditlogDto auditlogAdd)
    {
        var audit = this.mapper.Map<Auditlog>(auditlogAdd);

        audit.RecInserted = DateTime.UtcNow;

        //Get rowIDs if needed
        if (audit.ErstrnsUid is not null)
        {
            var refReq = await this.context.ErsRefReqDetails.FirstOrDefaultAsync(r => r.RefReqUniqueId == audit.ErstrnsUid);
            audit.RefReqRowId = refReq?.RefReqRowId;
        }

        if (audit.DoctrnsUid is not null)
        {
            var doc = await this.context.ErsdocAttachments.FirstOrDefaultAsync(a => a.AttachId == audit.DoctrnsUid);
            audit.RefDocRowId = doc?.RefDocRowId;
        }

        await this.context.Auditlogs.AddAsync(audit);

        return await this.context.SaveChangesAsync() > 0;
    }
}
