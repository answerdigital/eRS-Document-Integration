Class RFT.ERS.Tables.Worklist Extends %Persistent
{

Property ubrn As %String;

Property specialty As %String;

Property requestContextStatus As %String;

Property clinicalInfoPrinted As %String;

Property clinicalInfoFirstSubmitted As %String;

Property clinicalInfoLastUpdate As %String;

Property priority As %String;

Property patientID As %String;

Property eRefPathwayStart As %String;

Property refType As %String;

Property clinician As %String;

Property apptStart As %String;

Property statusFlag As %Integer;

Property UpdatePatientDB As %Integer;

Index ubrnIndex On ubrn [ IdKey, Unique ];

Storage Default
{
<Data name="WorklistDefaultData">
<Value name="1">
<Value>%%CLASSNAME</Value>
</Value>
<Value name="2">
<Value>specialty</Value>
</Value>
<Value name="3">
<Value>requestContextStatus</Value>
</Value>
<Value name="4">
<Value>clinicalInfoPrinted</Value>
</Value>
<Value name="5">
<Value>clinicalInfoFirstSubmitted</Value>
</Value>
<Value name="6">
<Value>clinicalInfoLastUpdate</Value>
</Value>
<Value name="7">
<Value>priority</Value>
</Value>
<Value name="8">
<Value>patientID</Value>
</Value>
<Value name="9">
<Value>eRefPathwayStart</Value>
</Value>
<Value name="10">
<Value>refType</Value>
</Value>
<Value name="11">
<Value>clinician</Value>
</Value>
<Value name="12">
<Value>apptStart</Value>
</Value>
<Value name="13">
<Value>statusFlag</Value>
</Value>
<Value name="14">
<Value>UpdatePatientDB</Value>
</Value>
</Data>
<DataLocation>^RFT.ERS.Tables.WorklistD</DataLocation>
<DefaultData>WorklistDefaultData</DefaultData>
<ExtentSize>1</ExtentSize>
<IdLocation>^RFT.ERS.Tables.WorklistD</IdLocation>
<IndexLocation>^RFT.ERS.Tables.WorklistI</IndexLocation>
<Property name="%%CLASSNAME">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="%%ID">
<AverageFieldSize>14</AverageFieldSize>
<Selectivity>1</Selectivity>
</Property>
<Property name="UpdatePatientDB">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="apptStart">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="clinicalInfoFirstSubmitted">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="clinicalInfoLastUpdate">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="clinicalInfoPrinted">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="clinician">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="eRefPathwayStart">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="patientID">
<AverageFieldSize>12</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="priority">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="refType">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="requestContextStatus">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="specialty">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="statusFlag">
<AverageFieldSize>2</AverageFieldSize>
<Selectivity>100.0000%</Selectivity>
</Property>
<Property name="ubrn">
<AverageFieldSize>14</AverageFieldSize>
<Selectivity>1</Selectivity>
</Property>
<SQLMap name="ubrnIndex">
<BlockCount>-4</BlockCount>
</SQLMap>
<StreamLocation>^RFT.ERS.Tables.WorklistS</StreamLocation>
<Type>%Library.CacheStorage</Type>
}

}

