Class RFT.ERS.Classes.WorklistItem Extends Ens.Response
{

Property specialty As %String;

Property requestContextStatus As %String;

Property clinicalInfoPrinted As %String;

Property clinicalInfoFirstSubmitted As %String;

Property clinicalInfoLastUpdate As %String;

Property priority As %String;

Property patientID As %String;

Property eRefPathwayStart As %String;

Property refType As %String;

Property clinician As %String;

Property apptStart As %String;

Property ubrn As %String;

Storage Default
{
<Data name="WorklistItemDefaultData">
<Subscript>"WorklistItem"</Subscript>
<Value name="1">
<Value>specialty</Value>
</Value>
<Value name="2">
<Value>requestContextStatus</Value>
</Value>
<Value name="3">
<Value>clinicalInfoPrinted</Value>
</Value>
<Value name="4">
<Value>clinicalInfoFirstSubmitted</Value>
</Value>
<Value name="5">
<Value>clinicalInfoLastUpdate</Value>
</Value>
<Value name="6">
<Value>priority</Value>
</Value>
<Value name="7">
<Value>patientID</Value>
</Value>
<Value name="8">
<Value>eRefPathwayStart</Value>
</Value>
<Value name="9">
<Value>refType</Value>
</Value>
<Value name="10">
<Value>clinician</Value>
</Value>
<Value name="11">
<Value>apptStart</Value>
</Value>
<Value name="12">
<Value>ubrn</Value>
</Value>
</Data>
<DefaultData>WorklistItemDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}

