Class RFT.ERS.Classes.ReferralDetails Extends Ens.Response
{

Property ubrn As %Integer;

Property patientID As %String;

Property apptStart As %String;

Property apptEnd As %String;

Property specialty As %String;

Property status As %String;

Property intent As %String;

Property priority As %String;

Property docStatus As %String;

Property clinicalInfoFirstSubmitted As %String;

Property clinicalInfoLastUpdate As %String;

Storage Default
{
<Data name="ReferralDetailsDefaultData">
<Subscript>"ReferralDetails"</Subscript>
<Value name="1">
<Value>ubrn</Value>
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
<Value>clinicalInfoFirstSubmitted</Value>
</Value>
<Value name="11">
<Value>clinicalInfoLastUpdate</Value>
</Value>
</Data>
<DefaultData>ReferralDetailsDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}

