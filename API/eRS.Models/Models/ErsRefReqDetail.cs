using System;
using System.Collections.Generic;

namespace eRS.Models.Models;

public partial class ErsRefReqDetail
{
    public int RefReqRowId { get; set; }
    public string? RefReqUniqueId { get; set; }
    public string? RefReqNhsno { get; set; }
    public string? RefReqUbrn { get; set; }
    public string? RefReqTrustNacs { get; set; }
    public DateTime? ApptStDttm { get; set; }
    public DateTime? ApptEndDttm { get; set; }
    public string? RefReqSpecialty { get; set; }
    public string? RefReqStatus { get; set; }
    public string? RefReqIntent { get; set; }
    public string? RefReqPriority { get; set; }
    public int? RefReqNoofdocs { get; set; }
    public string? RefReqFullUrl { get; set; }
    public string? WfsCode { get; set; }
    public DateTime? RecExpiryDttm { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}