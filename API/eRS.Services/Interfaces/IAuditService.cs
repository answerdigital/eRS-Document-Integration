using eRS.Models.Dtos;
using eRS.Models.Models.Audits;

namespace eRS.Services.Interfaces;

public interface IAuditService
{
    public Task<PagedResult<AuditlogDto>> GetAllFiltered(AuditRequest request);
    public Task<bool> AddAudit(AuditlogDto auditlogAdd);

}
