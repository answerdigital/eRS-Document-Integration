Class RFT.ERS.Classes.RequestWorklist Extends Ens.Request
{

Property sessionID As %Integer;

Storage Default
{
<Data name="RequestWorklistDefaultData">
<Subscript>"RequestWorklist"</Subscript>
<Value name="1">
<Value>sessionID</Value>
</Value>
</Data>
<DefaultData>RequestWorklistDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}

