namespace eRS.Models.Models.ersRefRequests;

public class WorklistFilter
{
    public string? RefReqStatus { get; set; }
    public string? MeditechPathway { get; set; }
    public string? RefReqSpecialty { get; set; }
    public string? Consultant { get; set; }
    public string? ErsService { get; set; }
    public DateTime? ApptDttmFrom { get; set; }
    public DateTime? ApptDttmTo { get; set; }
    public DateTime? RecInsertedFrom { get; set; }
    public DateTime? RecInsertedTo { get; set; }
    public bool? InvestigationMode { get; set; }
    public string? RefReqUbrn { get; set; }
}