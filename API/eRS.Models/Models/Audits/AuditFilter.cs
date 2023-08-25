namespace eRS.Models.Models.Audits;

public class AuditFilter
{
    public string? EventCode { get; set; }
    public DateTime? RecInsertedFrom { get; set; }
    public DateTime? RecInsertedTo { get; set; }
    public string? RecInsertedBy { get; set; }
    public string? RefReqUid { get; set; }
    public string? RefDocUid { get; set; }
    public string? NhsNo { get; set; }
}