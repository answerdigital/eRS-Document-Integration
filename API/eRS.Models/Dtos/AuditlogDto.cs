using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace eRS.Models.Dtos;

public sealed class AuditlogDto
{
    [Optional]
    public string? ErstrnsUid { get; set; }

    [Optional]
    public string? DoctrnsUid { get; set; }

    [Ignore]
    public DateTime? EventDttm { get; set; }

    [Optional]
    public string? FromEventCode { get; set; }

    [Optional]
    public string? FromStatusComments { get; set; }

    [Optional]
    public string? ToEventCode { get; set; }

    [Optional]
    public string? ToStatusComments { get; set; }

    [Optional]
    public DateTime? RecInserted { get; set; }

    [Optional]
    public string? RecInsertedBy { get; set; }

    [Optional]
    public string? NhsNo { get; set; }

    [Optional]
    public string? PatName { get; set; }

    [Ignore]
    public ErsRefReqDetailDto? ErsRefReqDetail { get; set; }

    [Ignore]
    public ErsdocAttachmentDto? ErsdocAttachment { get; set; }

}