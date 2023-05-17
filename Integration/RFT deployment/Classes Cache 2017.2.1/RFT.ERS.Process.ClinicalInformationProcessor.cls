Class RFT.ERS.Process.ClinicalInformationProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Method OnRequest(pInput As RFT.ERS.Classes.RequestReferralDetails, Output pResponse As Ens.Response) As %Status
{
	set tsc = $$$OK
	set req=##class(RFT.ERS.Classes.RequestClinicalInformation).%New()
	set req.ubrn=pInput.ubrn
	set t=..SendRequestSync("HttpOut_ERS",req,.refResp)
	if refResp
	{
		set workListItem=##class(RFT.ERS.Tables.Worklist).%OpenId(req.ubrn)
		set workListItem.UpdatePatientDB=1
		$$$LOGINFO("workListItem.UpdatePatientDB=1")
	}
	quit tsc
}

Method GenerateAlert(AlertText As %String)
{
	set t=$$$OK
	
	set alert=##class(Ens.AlertRequest).%New()
	set alert.AlertText=AlertText
	set alert.AlertDestination="Ens.Alert"
	set t=..SendAlert(alert)
	quit t
}

Storage Default
{
<Type>%Library.CacheStorage</Type>
}

}

