Class RFT.ERS.Classes.RequestAttachment Extends Ens.Request
{

Property attachmentID As %Integer;

Property url As %String;

Storage Default
{
<Data name="RequestAttachmentDefaultData">
<Subscript>"RequestAttachment"</Subscript>
<Value name="1">
<Value>attachmentID</Value>
</Value>
<Value name="2">
<Value>url</Value>
</Value>
</Data>
<DefaultData>RequestAttachmentDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

