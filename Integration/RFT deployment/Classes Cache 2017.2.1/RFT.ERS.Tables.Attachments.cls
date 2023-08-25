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
<ExtentSize>1</ExtentSize>
<IdLocation>^RFT.ERS.Tables.AttachmentsD</IdLocation>
<IndexLocation>^RFT.ERS.Tables.AttachmentsI</IndexLocation>
<Property name="%%CLASSNAME">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="%%ID">
<AverageFieldSize>4</AverageFieldSize>
<Selectivity>1</Selectivity>
</Property>
<Property name="attachmentID">
<AverageFieldSize>4</AverageFieldSize>
<Selectivity>1</Selectivity>
</Property>
<Property name="contentType">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="createdDateTime">
<AverageFieldSize>12</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="docStatus">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="filePath">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="insertedBy">
<AverageFieldSize>14</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="isDownloaded">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="retryCount">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="size">
<AverageFieldSize>5</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="title">
<AverageFieldSize>27</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="ubrn">
<AverageFieldSize>14</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="url">
<AverageFieldSize>19</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<SQLMap name="Index">
<BlockCount>-4</BlockCount>
</SQLMap>
<StreamLocation>^RFT.ERS.Tables.AttachmentsS</StreamLocation>
<Type>%Storage.Persistent</Type>
}

}

