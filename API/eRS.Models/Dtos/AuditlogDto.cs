namespace eRS.Models.Dtos;

public partial class AuditlogDto
{
    public DateTime? EventDttm { get; set; }
    public string? EventCode { get; set; }
    public string? EventDescription { get; set; }
    public string? EventDetails { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}