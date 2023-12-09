Class RFT.ERS.Operation.ExecuteQuery Extends Ens.BusinessOperation
{

Parameter ADAPTER = "EnsLib.SQL.OutboundAdapter";

Property Adapter As EnsLib.SQL.OutboundAdapter;

Parameter INVOCATION = "Queue";

Method OnRequest(pRequest As %DynamicObject, Output pResponse As Ens.Response)
{
		Set tsc = $$$OK
		$$$TRACE("Start")
	Try {
		Set pResponse = ##class(EnsLib.SQL.Snapshot).%New()
		Set Qry = "INSERT INTO TABLE patients" _ pRequest.Identifier _ " " _ pRequest.FamilyName  _ " " _ pRequest.GivenName _ " " _ pRequest.Gender _ " " _ pRequest.DOB _ " " _ pRequest.Telecom
		$$$TRACE(Qry)
		Set tsc = ..Adapter.ExecuteUpdate(.pResponse, Qry)
	}
	Catch Ex {
		Set tsc = Ex.AsStatus()
	}
	
	quit tsc

	//XData MessageMap
	//{
	//<MapItems>
	//  <MapItem MessageType="Ens.StringContainer">
	//    <Method>UpdateData</Method>
	//  </MapItem>
	//  </MapItems>
	//}
}

}

