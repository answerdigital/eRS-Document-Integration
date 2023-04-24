Class RFT.ERS.Classes.ReferralDetailsResponse Extends Ens.Response
{

Property Referral As RFT.ERS.Classes.ReferralDetails;

Property Attachment As list Of RFT.ERS.Classes.Attachment;

Property StatusCode As %Integer;

Property ErrorText As %String;

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
</Data>
<DefaultData>ReferralDetailsResponseDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

