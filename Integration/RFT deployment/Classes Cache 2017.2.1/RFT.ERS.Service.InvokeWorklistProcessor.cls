Class RFT.ERS.Service.InvokeWorklistProcessor Extends Ens.BusinessService
{

// Parameter ADAPTER = "Ens.InboundAdapter";

Parameter ADAPTER = "EnsLib.HTTP.InboundAdapter";

Property Adapter As EnsLib.HTTP.InboundAdapter;

Parameter INVOCATION = "Queue";

Method OnProcessInput(pInput As %Stream.GlobalCharacter, Output pResponse As Ens.Response) As %Status
{
    // Token ingest
	set tsc = $$$OK
	set TokenReceived = 0

    Set AuthJson=pInput.Read(,.tsc)
	set AuthJsonObj = ##class(%DynamicAbstractObject).%FromJSON(AuthJson)
	$$$LOGINFO("AuthJsonObj : "_AuthJsonObj.%Get("auth_token"))
	set ^LastToken("auth_token") = AuthJsonObj.%Get("auth_token")
	set ^LastToken("nhsid_useruid") = AuthJsonObj.%Get("nhsid_useruid")
	set ^LastToken("name") = AuthJsonObj.%Get("name")
	set ^LastToken("token_time") = $zdatetime($horolog,3)
	set TokenReceived = 1

    //make sure Read when OK
	if TokenReceived = 1
	{
		set tsc=..SendRequestSync("WorklistProcessor",TokenReceived)
		// if send with success return {"responseCode":200,"message":"OK"}
		Set pResponse=##class(%GlobalBinaryStream).%New()
		Do pResponse.SetAttribute("Content-Type","application/json")
    	Do pResponse.Write("{"_$c(34)_"responseCode"_$c(34)_":200,"_$c(34)_"message"_$c(34)_":"_$c(34)_"OK"_$c(34)_"}")
	}
	else {If $$$ISERR(tsc) do $System.Status.DisplayError(tsc)}
	
	quit tsc
}

}

