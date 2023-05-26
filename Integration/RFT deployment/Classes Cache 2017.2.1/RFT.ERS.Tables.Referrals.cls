Class RFT.ERS.Tables.Referrals Extends %Persistent
{

Property ubrn As %String;

Property patientID As %String;

Property apptStart As %String;

Property apptEnd As %String;

Property specialty As %String;

Property status As %String;

Property intent As %String;

Property priority As %String;

Property docStatus As %String;

Property NumDocs As %Integer;

Property statusFlag As %Boolean;

Index ubrnIndex On ubrn [ IdKey, Unique ];

Storage Default
{
<Data name="ReferralsDefaultData">
<Value name="1">
<Value>%%CLASSNAME</Value>
</Value>
<Value name="2">
<Value>patientID</Value>
</Value>
<Value name="3">
<Value>apptStart</Value>
</Value>
<Value name="4">
<Value>apptEnd</Value>
</Value>
<Value name="5">
<Value>specialty</Value>
</Value>
<Value name="6">
<Value>status</Value>
</Value>
<Value name="7">
<Value>intent</Value>
</Value>
<Value name="8">
<Value>priority</Value>
</Value>
<Value name="9">
<Value>docStatus</Value>
</Value>
<Value name="10">
<Value>NumDocs</Value>
</Value>
<Value name="11">
<Value>statusFlag</Value>
</Value>
</Data>
<DataLocation>^RFT.ERS.Tables.ReferralsD</DataLocation>
<DefaultData>ReferralsDefaultData</DefaultData>
<IdLocation>^RFT.ERS.Tables.ReferralsD</IdLocation>
<IndexLocation>^RFT.ERS.Tables.ReferralsI</IndexLocation>
<StreamLocation>^RFT.ERS.Tables.ReferralsS</StreamLocation>
<Type>%Storage.Persistent</Type>
}

}

