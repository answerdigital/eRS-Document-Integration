Class RFT.ERS.Classes.AttachmentResponse Extends Ens.Response
{

Property StatusCode As %Integer;

Property FilePath As %String(MAXLEN = 200);

Property FileType As %String(MAXLEN = 100);

Storage Default
{
<Data name="AttachmentResponseDefaultData">
<Subscript>"AttachmentResponse"</Subscript>
<Value name="1">
<Value>StatusCode</Value>
</Value>
<Value name="2">
<Value>FilePath</Value>
</Value>
<Value name="3">
<Value>FileType</Value>
</Value>
</Data>
<DefaultData>AttachmentResponseDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

