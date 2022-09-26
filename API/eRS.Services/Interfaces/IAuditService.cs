using eRS.Models.Dtos;
using eRS.Models.Models.Audits;

namespace eRS.Services.Interfaces;

public interface IAuditService
{
    public Task<PagedResult<AuditlogDto>> GetAllFilteredPaged(AuditRequest request);
    public Task<List<AuditlogDto>> GetAllFiltered(AuditRequest request);
    public Task<bool> AddAudit(AuditlogDto auditlogAdd);
    public Task<string> GenerateCsv(List<AuditlogDto> audits);

}
