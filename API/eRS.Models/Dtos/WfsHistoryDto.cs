namespace eRS.Models.Dtos;

public partial class WfsHistoryDto
{
    public string? ErstrnsUid { get; set; }
    public string? DoctrnsUid { get; set; }
    public string? AttachTitle { get; set; }
    public string? StatusCode { get; set; }
    public WfsMasterDto? WfsMaster { get; set; }
    public int? StatusHierarchy { get; set; }
    public string? StatusComments { get; set; }
    public DateTime? StatusEffdttm { get; set; }
    public string? StatusPerformedBy { get; set; }
    public DateTime? StatusCancelledDttm { get; set; }
    public string? StatusCancelledBy { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}