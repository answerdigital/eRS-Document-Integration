Class RFT.ERS.Tables.Attachments Extends %Persistent
{

Property attachmentID As %Integer;

Property insertedBy As %String;

Property contentType As %String;

Property url As %String;

Property size As %String;

Property title As %String(MAXLEN = 200);

Property createdDateTime As %String;

Property ubrn As %String;

Property docStatus As %String;

Property isDownloaded As %Integer;

Property retryCount As %Integer;

Property filePath As %String(MAXLEN = 1000);

Index Index On attachmentID [ IdKey, Unique ];

Storage Default
{
<Data name="AttachmentsDefaultData">
<Value name="1">
<Value>%%CLASSNAME</Value>
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
<Value>createdDateTime</Value>
</Value>
<Value name="8">
<Value>ubrn</Value>
</Value>
<Value name="9">
<Value>docStatus</Value>
</Value>
<Value name="10">
<Value>isDownloaded</Value>
</Value>
<Value name="11">
<Value>retryCount</Value>
</Value>
<Value name="12">
<Value>filePath</Value>
</Value>
</Data>
<DataLocation>^RFT.ERS.Tables.AttachmentsD</DataLocation>
<DefaultData>AttachmentsDefaultData</DefaultData>
<IdLocation>^RFT.ERS.Tables.AttachmentsD</IdLocation>
<IndexLocation>^RFT.ERS.Tables.AttachmentsI</IndexLocation>
<StreamLocation>^RFT.ERS.Tables.AttachmentsS</StreamLocation>
<Type>%Library.CacheStorage</Type>
}

}

