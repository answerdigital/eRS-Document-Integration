using AutoMapper;
using CsvHelper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace eRS.Services.Services;

public class AuditService : IAuditService
{
    private readonly eRSContext context;
    private readonly IMapper mapper;
    private readonly ILogger<AuditService> logger;

    public AuditService(eRSContext context, IMapper mapper, ILogger<AuditService> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<List<AuditlogDto>> GetAllFiltered(AuditRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var auditsQuery = GetAllFilteredQuery(request.Filters);

        var resultItems = await auditsQuery.ToListAsync();

        return this.mapper.Map<List<AuditlogDto>>(resultItems);
    }

    public async Task<PagedResult<AuditlogDto>> GetAllFilteredPaged(AuditRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.PageNumber is null)
        {
            throw new ArgumentNullException(nameof(request.PageNumber));
        }

        var auditsQuery = GetAllFilteredQuery(request.Filters);

        var pageSize = 10;
        var result = new PagedResult<AuditlogDto> { CurrentPage = request.PageNumber.Value, PageSize = pageSize, RowCount = auditsQuery.Count() };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (request.PageNumber.Value - 1) * pageSize;

        var resultItems = await auditsQuery.Skip(skip).Take(pageSize).ToListAsync();

        result.Results = this.mapper.Map<List<AuditlogDto>>(resultItems);

        return result;
    }

    private IQueryable<Auditlog> GetAllFilteredQuery(AuditFilter filters)
    {
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

        if (filters.RecInsertedFrom is not null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted >= filters.RecInsertedFrom);
        }

        if (filters.RecInsertedTo is not null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted <= filters.RecInsertedTo);
        }

        return auditsQuery;
    }

    public async Task<bool> AddAudit(Auditlog audit)
    {
        audit.RecInserted = DateTime.UtcNow;

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

    public async Task<string> GenerateCsv(List<AuditlogDto> audits)
    {
        string csvString = "";

        using (var writer = new StringWriter())
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(audits);
            csv.Flush();
            csvString = writer.ToString();
        }

        return csvString;
    }
}
