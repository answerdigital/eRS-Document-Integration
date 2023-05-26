Class RFT.ERS.Classes.RefToDB Extends Ens.Request
{

Property ubrn As %String;

Property attachmentCount As %Integer;

Property downloadCount As %Integer;

Storage Default
{
<Data name="RefToDBDefaultData">
<Subscript>"RefToDB"</Subscript>
<Value name="1">
<Value>ubrn</Value>
</Value>
<Value name="2">
<Value>attachmentCount</Value>
</Value>
<Value name="3">
<Value>downloadCount</Value>
</Value>
</Data>
<DefaultData>RefToDBDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}

