using System;
using System.Collections.Generic;

namespace eRS.Models.Models;

public partial class ErsdocAttachment
{
    public int RefDocRowId { get; set; }
    public int? RefrequestRowId { get; set; }
    public string? RefReqUniqueId { get; set; }
    public int? RefDocSrlno { get; set; }
    public string? RefDocUniqueId { get; set; }
    public string? RefDocStatus { get; set; }
    public string? AttachId { get; set; }
    public string? AttachInsertedBy { get; set; }
    public string? AttachContentType { get; set; }
    public string? AttachUrl { get; set; }
    public string? AttachSize { get; set; }
    public string? AttachTitle { get; set; }
    public DateTime? AttachCrtdDttm { get; set; }
    public string? DocDownloadUrl { get; set; }
    public string? DocLocationUri { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}