namespace eRS.Models.Dtos;

public partial class AuditlogDto
{
    public string? ErstrnsUid { get; set; }
    public string? DoctrnsUid { get; set; }
    public DateTime? EventDttm { get; set; }
    public string? FromEventCode { get; set; }
    public string? FromStatusComments { get; set; }
    public string? ToEventCode { get; set; }
    public string? ToStatusComments { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
    public string? UserReference { get; set; }
    public string? NhsNo { get; set; }
    public string? PatName { get; set; }
    public ErsRefReqDetailDto? ErsRefReqDetail { get; set; }
    public ErsdocAttachmentDto? ErsdocAttachment { get; set; }
}