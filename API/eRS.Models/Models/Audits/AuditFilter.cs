namespace eRS.Models.Models.Audits;

public class AuditFilter
{
    public string? EventCode { get; set; }
    public string? EventDescription { get; set; }
    public string? EventDetails { get; set; }
    public DateTime? RecInsertedFrom { get; set; }
    public DateTime? RecInsertedTo { get; set; }
    public string? RecInsertedBy { get; set; }
}