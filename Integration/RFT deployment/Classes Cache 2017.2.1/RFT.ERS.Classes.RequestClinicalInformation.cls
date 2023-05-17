Class RFT.ERS.Classes.RequestClinicalInformation Extends Ens.Request
{

Property ubrn As %String;

Storage Default
{
<Data name="ClinicalInformationDefaultData">
<Subscript>"ClinicalInformation"</Subscript>
<Value name="1">
<Value>ubrn</Value>
</Value>
</Data>
<DefaultData>ClinicalInformationDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

