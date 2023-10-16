﻿namespace eRS.Data.Entities;

public sealed class WfsHistory
{
    public int ErsdocsRowid { get; set; }
    public int? RefReqRowId { get; set; }
    public ErsRefReqDetail? ErsRefReqDetail { get; set; }
    public int? RefDocRowId { get; set; }
    public ErsdocAttachment? ErsdocAttachment { get; set; }
    public string? ErstrnsUid { get; set; }
    public string? DoctrnsUid { get; set; }
    public string? StatusCode { get; set; }
    public WfsMaster? WfsMaster { get; set; }
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