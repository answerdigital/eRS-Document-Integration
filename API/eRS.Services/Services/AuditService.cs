using AutoMapper;
using eRS.Data;
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

    public async Task<PagedResult<AuditlogDto>?> GetAllFiltered(AuditRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var filters = request.Filters;
        var auditsQuery = this.context.Auditlogs.Where(a => a.RecStatus != "D").AsQueryable();

        auditsQuery.Where(a =>
            (!string.IsNullOrWhiteSpace(a.EventCode) && EF.Functions.Like(a.EventCode, $"%{filters.SearchByEventCode}%")) ||
            (!string.IsNullOrWhiteSpace(a.EventDescription) && EF.Functions.Like(a.EventDescription, $"%{filters.SearchByEventDescription}%")) ||
            (!string.IsNullOrWhiteSpace(a.EventDetails) && EF.Functions.Like(a.EventDetails, $"%{filters.SearchByEventDetails}%")) ||
            (!string.IsNullOrWhiteSpace(a.RecInsertedBy) && EF.Functions.Like(a.RecInsertedBy, $"%{filters.SearchByRecInsertedBy}%")));

        if (filters.FilterByRecInsertedFrom != null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted > filters.FilterByRecInsertedFrom);
        }

        if (filters.FilterByRecInsertedTo != null)
        {
            auditsQuery = auditsQuery.Where(a => a.RecInserted < filters.FilterByRecInsertedTo);
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

    public async Task<AuditlogDto?> GetById(int auditId)
    {
        var audit = await this.context.Auditlogs.FirstOrDefaultAsync(a => a.AuditRowId == auditId);

        return this.mapper.Map<AuditlogDto>(audit);
    }
}
