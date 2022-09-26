namespace eRS.Models.Models.Audits;

public class AuditRequest
{
    public int? PageNumber { get; set; }
    public AuditFilter Filters { get; set; }
}
