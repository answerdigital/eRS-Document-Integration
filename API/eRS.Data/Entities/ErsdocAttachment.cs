namespace eRS.Data.Entities;

public partial class ErsdocAttachment
{
    public int RefDocRowId { get; set; }
    public int? RefrequestRowId { get; set; }
    public ErsRefReqDetail? RefReqDetail { get; set; }
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
    public string? PreviouslyDownloadedDoc { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public string? RecInsertedBy { get; set; }
    public List<WfsHistory> WfsHistoryList { get; }
    public List<Auditlog> Audits { get; }
}