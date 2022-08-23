namespace eRS.Models.Models.Audits;

public class AuditFilter
{
    public DateTime? FilterByEventDttm { get; set; }
    public string? SearchByEventCode { get; set; }
    public string? SearchByEventDescription { get; set; }
    public string? SearchByEventDetails { get; set; }
    public DateTime? FilterByRecInsertedFrom { get; set; }
    public DateTime? FilterByRecInsertedTo { get; set; }
    public string? SearchByRecInsertedBy { get; set; }
}