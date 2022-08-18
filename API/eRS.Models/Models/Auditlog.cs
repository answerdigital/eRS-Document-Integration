using System;
using System.Collections.Generic;

namespace eRS.Models.Models;

public partial class Auditlog
{
    public int AuditRowId { get; set; }
    public DateTime? EventDttm { get; set; }
    public string? EventCode { get; set; }
    public string? EventDescription { get; set; }
    public string? EventDetails { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}