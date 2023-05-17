Class RFT.ERS.Classes.Attachment Extends Ens.Response
{

Property attachmentID As %Integer;

Property insertedBy As %String;

Property contentType As %String(MAXLEN = 100);

Property url As %String;

Property size As %String;

Property title As %String(MAXLEN = 100);

Property docStatus As %String;

Property createdDateTime As %String;

Storage Default
{
<Data name="AttachmentDefaultData">
<Subscript>"Attachment"</Subscript>
<Value name="1">
<Value>attachmentID</Value>
</Value>
<Value name="2">
<Value>insertedBy</Value>
</Value>
<Value name="3">
<Value>contentType</Value>
</Value>
<Value name="4">
<Value>url</Value>
</Value>
<Value name="5">
<Value>size</Value>
</Value>
<Value name="6">
<Value>title</Value>
</Value>
<Value name="7">
<Value>docStatus</Value>
</Value>
<Value name="8">
<Value>createdDateTime</Value>
</Value>
</Data>
<DefaultData>AttachmentDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

