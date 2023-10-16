Class RFT.ERS.Process.WorklistProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Method OnRequest(pRequest As Ens.Request, Output pResponse As Ens.StringContainer) As %Status
{

	set t=$$$OK
	
	//Request worklist from ERS 
	
	set req=##class(RFT.ERS.Classes.RequestWorklist).%New()
	set req.sessionID=1
	
	set t=..SendRequestSync("HttpOut_ERS",req,.worklistResponse)

	Set ClassType = $CLASSNAME(worklistResponse)
	$$$LOGINFO("ClassType : "_ClassType)

	if ClassType= "Ens.StringContainer"
	{
		Set pResponse = ##class(Ens.StringContainer).%New()
		;set pResponse.StringValue = "{"_worklistResponse.StringValue_"}"
		set pResponse.StringValue = "{"_$c(34)_"responseCode"_$c(34)_":200,"_$c(34)_"message"_$c(34)_":"_$c(34)_"OK"_$c(34)_"}"
		GOTO END
	}
	
	IF '$$$ISERR(t)
	{
	
	#DIM worklistResponse As RFT.ERS.Classes.WorklistResponse
	
	//Loop through response
	//add worklist to local list
		for i=1:1:worklistResponse.Item.Count()
		{
			set item=worklistResponse.Item.GetAt(i)
			set ubrn=item.ubrn
			//First check if ubrn exists in local list
			set workList=##class(RFT.ERS.Tables.Worklist).%OpenId(ubrn)

			IF $ISOBJECT(workList)
			{	 //We need to check if there is an update been made on ERS thats not been updated locally

				$$$TRACE("UBRN "_ubrn_" exists on local list")
				set localLastUpdated=workList.clinicalInfoLastUpdate


				if $ISOBJECT(item.clinicalInfoLastUpdate) 
				{
					set ersLastUpdate=item.clincicalInfoLastUpdate
				}else
				{
					set ersLastUpdate=item.clinicalInfoFirstSubmitted
				}

				set dateDiff=##class(%SYSTEM.SQL).DATEDIFF("ss",localLastUpdated,ersLastUpdate)		

				if $G(dateDiff)>0
				{
					$$$TRACE("Update to local list required. UBRN: "_ubrn)
					set workList.specialty=item.specialty
					set workList.apptStart=item.apptStart
					set workList.clinicalInfoLastUpdate=item.clinicalInfoLastUpdate
					set workList.priority=item.priority
					set workList.refType=item.refType
					set workList.clinician=item.clinician
					set workList.apptStart=item.apptStart
					set workList.statusFlag=0 //We will poll A005 to grab ref details again
					set workList.UpdatePatientDB=0

					set t=workList.%Save()

					//delete from attachments table as we will redownload
					&sql(DELETE FROM RFT_ERS_Tables.Attachments where ubrn=:ubrn)
				}
				else 
				{
					$$$TRACE("No update required for ubrn: "_ubrn)
				}	
			}
			else 
			{	
				$$$TRACE("Adding new record on local list for: "_ubrn)
				set workList=##class(RFT.ERS.Tables.Worklist).%New()
				set workList.specialty=item.specialty
				set workList.requestContextStatus=item.requestContextStatus
				set workList.clinicalInfoPrinted=item.clinicalInfoPrinted
				set workList.clinicalInfoFirstSubmitted=item.clinicalInfoFirstSubmitted
				set workList.eRefPathwayStart=item.eRefPathwayStart
				set workList.priority=item.priority
				set workList.refType=item.refType
				set workList.clinician=item.clinician
				set workList.apptStart=item.apptStart
				set workList.statusFlag=0 
				set workList.ubrn=item.ubrn
				set workList.patientID=item.patientID
				set workList.UpdatePatientDB=0
				set t=workList.%Save()
			}		


		}
		Set pResponse = ##class(Ens.StringContainer).%New()
		set pResponse.StringValue = "{"_$c(34)_"responseCode"_$c(34)_":200,"_$c(34)_"message"_$c(34)_":"_$c(34)_"OK"_$c(34)_"}"
	}
END
	quit t
}

Storage Default
{
<Type>%Storage.Persistent</Type>
}

}
