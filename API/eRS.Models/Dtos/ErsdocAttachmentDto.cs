namespace eRS.Models.Dtos;

public partial class ErsdocAttachmentDto
{
    public int? RefrequestRowId { get; set; }
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
    public string? RecInsertedBy { get; set; }

    public WfsHistoryDto? WfsHistory { get; set; }
}