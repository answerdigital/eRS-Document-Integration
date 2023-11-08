Class RFT.ERS.Classes.ReferralDetailsResponse Extends Ens.Response
{

Property Referral As RFT.ERS.Classes.ReferralDetails;

Property Attachment As list Of RFT.ERS.Classes.Attachment;

Property StatusCode As %Integer;

Property ErrorText As %String;

Property clinicalInfoFirstSubmitted As %String;

Property clinicalInfoLastUpdate As %String;

Storage Default
{
<Data name="ReferralDetailsResponseDefaultData">
<Subscript>"ReferralDetailsResponse"</Subscript>
<Value name="1">
<Value>Referral</Value>
</Value>
<Value name="2">
<Value>Attachment</Value>
</Value>
<Value name="3">
<Value>StatusCode</Value>
</Value>
<Value name="4">
<Value>ErrorText</Value>
</Value>
<Value name="5">
<Value>clinicalInfoFirstSubmitted</Value>
</Value>
<Value name="6">
<Value>clinicalInfoLastUpdate</Value>
</Value>
</Data>
<DefaultData>ReferralDetailsResponseDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}

