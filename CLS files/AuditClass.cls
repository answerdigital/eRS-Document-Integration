Class RFT.ERS.Classes.AuditClass Extends %Persistent
{

ClassMethod PerformAction(userID As %String, action As %String, details As %String) As %Status
{
        // Construct the audit event data
        Set eventSource = "NHS ERS API"
        Set eventType = "WorkList Retrieval"
        Set event = action
        Set DateTime = $zts
        Set HumanDateTime = $zdatetime(DateTime, 5)
        Set eventData = details_". User: "_userID_"."_" Performed Action: "_action_". DateTime: "_HumanDateTime_"."
        Set description = "User '" _ userID _ "' Performed action: " _ action _"DateTime: " _HumanDateTime

        // Call the $SYSTEM.Security.Audit() command
        Do $SYSTEM.Security.Audit(userID, eventType, action, eventData, description)

        Quit $$$OK
}

Storage Default
{
<Data name="AuditClassDefaultData">
<Value name="1">
<Value>%%CLASSNAME</Value>
</Value>
</Data>
<DataLocation>^RFT.ERS.Classes.AuditClassD</DataLocation>
<DefaultData>AuditClassDefaultData</DefaultData>
<IdLocation>^RFT.ERS.Classes.AuditClassD</IdLocation>
<IndexLocation>^RFT.ERS.Classes.AuditClassI</IndexLocation>
<StreamLocation>^RFT.ERS.Classes.AuditClassS</StreamLocation>
<Type>%Storage.Persistent</Type>
}

}
