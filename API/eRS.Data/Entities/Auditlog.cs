using System.ComponentModel.DataAnnotations;

namespace eRS.Data.Entities;

public partial class Auditlog
{
    public int AuditRowId { get; set; }
    public DateTime? EventDttm { get; set; }
    public string? EventCode { get; set; }
    public string? EventDescription { get; set; }
    public string? EventDetails { get; set; }
    public string? RecStatus { get; set; }
    [Timestamp]
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}