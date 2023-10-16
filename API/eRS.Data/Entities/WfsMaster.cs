﻿namespace eRS.Data.Entities;

public sealed class WfsMaster
{
    public int WfsmRowid { get; set; }
    public string? WfsmCode { get; set; }
    public List<WfsHistory>? WfsHistories { get; set; }
    public string? WfsmDescription { get; set; }
    public string? WfsmDisplayValue { get; set; }
    public int? WfsmHierarchy { get; set; }
    public string? WfsmPrevHierarchy { get; set; }
    public string? WfsmNextHierarchy { get; set; }
    public string? RecStatus { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public DateTime? RecInserted { get; set; }
    public string? RecInsertedBy { get; set; }
    public int? RecPurgeDays { get; set; }
}