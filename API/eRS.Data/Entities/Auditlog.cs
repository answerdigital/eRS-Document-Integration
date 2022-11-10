namespace eRS.Data.Entities;

public partial class Auditlog
{
    public int AuditRowId { get; set; }
    public int? RefReqRowId { get; set; }
    public ErsRefReqDetail? ErsRefReqDetail { get; set; }
    public int? RefDocRowId { get; set; }
    public ErsdocAttachment? ErsdocAttachment { get; set; }
    public string? ErstrnsUid { get; set; }
    public string? DoctrnsUid { get; set; }
    public DateTime? EventDttm { get; set; }
    public string? FromEventCode { get; set; }
    public string? FromStatusComments { get; set; }
    public string? ToEventCode { get; set; }
    public string? ToStatusComments { get; set; }
    public string? RecStatus { get; set; } = "A";
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}