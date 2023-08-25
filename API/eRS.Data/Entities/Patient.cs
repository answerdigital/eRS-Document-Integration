namespace eRS.Data.Entities;

public sealed class Patient
{
    public int PatRowID { get; set; }
    public string? PatUbrn { get; set; }
    public string? PatMrn { get; set; }
    public string? PatNhs { get; set; }
    public string? PatSpeciality { get; set; }
    public string? PatFamilyName { get; set; }
    public string? PatGivenName { get; set; }
    public string? PatFullName { get; set; }
    public string? PatSex { get; set; }
    public DateTime? PatDob { get; set; }
    public string? PatAddressOne { get; set; }
    public string? PatAddressTwo { get; set; }
    public string? PatAddressThree { get; set; }
    public string? PatPostCode { get; set; }
    public string? PatContactNumber { get; set; }
    public DateTime? RecUpdated { get; set; }
    public string? RecUpdatedBy { get; set; }
    public ErsRefReqDetail? ErsRefReqDetail { get; }
}
