namespace eRS.Data.Entities;

public partial class ErseventErrorlog
{
    public int ERseventRowId { get; set; }
    public string? ERsEvent { get; set; }
    public int? RefReqRowId { get; set; }
    public int? RefDocRowId { get; set; }
    public string? ErstrnsUid { get; set; }
    public string? DoctrnsUid { get; set; }
    public string? ERsGetUri { get; set; }
    public string? ERseventResponseCode { get; set; }
    public string? ERseventResponseDesc { get; set; }
    public int? EnsSessionId { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
}