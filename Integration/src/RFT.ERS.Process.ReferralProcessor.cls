Class RFT.ERS.Process.ReferralProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Method OnRequest(pInput As RFT.ERS.Classes.RequestReferralDetails, Output pResponse As Ens.Response) As %Status
{
	set t=$$$OK
	$$$TRACE(pInput.ubrn)
	set ubrn = pInput.ubrn
	
	//Query ERS API for referral details
	set t=..SendRequestSync("HttpOut_ERS",pInput,.refResp)
	#DIM refResp As RFT.ERS.Classes.ReferralDetailsResponse

	IF t
	{
		$$$TRACE("Will do  clinicalInfoFirstSubmitted & clinicalInfoFirstSubmitted2")
		//First check if ubrn exists in local list
		set workList=##class(RFT.ERS.Tables.Worklist).%OpenId(ubrn)
		
		IF $ISOBJECT(workList)
		{	 //We need to check if there is an update been made on ERS thats not been updated locally
			$$$TRACE("UBRN "_ubrn_" exists on local list")
	
			// Case workList.clinicalInfoFirstSubmitted empty need to pick up the response value
			if $LENGTH(refResp.Referral.clinicalInfoFirstSubmitted) '= 0
			{
				if $LENGTH(workList.clinicalInfoFirstSubmitted) = 0
				{
					set workList.clinicalInfoFirstSubmitted = refResp.Referral.clinicalInfoFirstSubmitted
				}
			}

			// if refResp.Referral.clinicalInfoLastUpdate exist then Populate the variable ersLastUpdate
			if $LENGTH(refResp.Referral.clinicalInfoLastUpdate) '= 0
			{
				set ersLastUpdate = refResp.Referral.clinicalInfoLastUpdate
			}
			else
			{
				set ersLastUpdate = refResp.Referral.clinicalInfoFirstSubmitted
			}

			// Case workList.clinicalInfoLastUpdate empty
			set localLastUpdated = workList.clinicalInfoLastUpdate
			if $LENGTH(localLastUpdated) = 0
			{
				set workList.clinicalInfoLastUpdate = ersLastUpdate
			}

			if localLastUpdated '= ersLastUpdate
			{
				$$$TRACE("Update to local list required. UBRN: "_ubrn)
				set workList.specialty=refResp.Referral.specialty
				set workList.apptStart=refResp.Referral.apptStart
				;set workList.clinicalInfoLastUpdate=refResp.Referral.clinicalInfoLastUpdate
				set workList.priority=refResp.Referral.priority
				;set workList.refType=refResp.Referral.refType
				;set workList.clinician=refResp.Referral.clinician
				set workList.apptStart=refResp.Referral.apptStart
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


		// Updating referrals table

		set ref=##class(RFT.ERS.Tables.Referrals).%OpenId(pInput.ubrn)
		
		IF '$ISOBJECT(ref)
		{
			$$$TRACE("Adding new entry to local list")
			set ref=##class(RFT.ERS.Tables.Referrals).%New()
		}
		
		set ref.patientID=refResp.Referral.patientID
		if refResp.Referral.apptStart'="" set ref.apptStart=refResp.Referral.apptStart
		if refResp.Referral.apptEnd'="" set ref.apptEnd=refResp.Referral.apptEnd
		set ref.specialty=refResp.Referral.specialty
		set ref.status=refResp.Referral.status
		set ref.intent=refResp.Referral.intent
		set ref.priority=refResp.Referral.priority
		set ref.docStatus=refResp.Referral.docStatus
		set ref.ubrn=pInput.ubrn
		set ref.statusFlag=0

		//count the number of attachments in response
		set numbDocs=0
		
		//set previousNumbDocs=ref.NumDocs
		for i=1:1:refResp.Attachment.Count()
		{
			set attchItem=refResp.Attachment.GetAt(i)
			set attach=##class(RFT.ERS.Tables.Attachments).%New()
			set numbDocs=numbDocs+1
			
			IF attchItem.size>5242880
			{
				$$$TRACE("Attachment too large")
				//This will error if we try to download
				//Send email alert 			
				set errorText="The attachment ID: "_attchItem.attachmentID
				set errorText=errorText_" for UBRN: "_pInput.ubrn_" is too large "
				set errorText=errorText_" please only upload files below 5mb"
				set t=..GenerateAlert(errorText)
			
			GOTO END
			}
		
			set attach.size=attchItem.size
			
			set attId=attchItem.attachmentID
			set url=$P(attchItem.url,"/",2)
			set attach.attachmentID=attId
			$$$LOGINFO("adding id: "_attId)
			set attach.insertedBy=attchItem.insertedBy
			//set attach.contentType=attchItem.contentType
			set attach.url=url
			set attach.title=attchItem.title
			set attach.createdDateTime=attchItem.createdDateTime
			set attach.ubrn=pInput.ubrn 
			set attach.isDownloaded=0
				
			set t=attach.%Save()
			IF '$$$ISERR(t) $$$TRACE("Attachment details saved")
				
			$$$TRACE("Now trying to download attachment")				
				
			set req=##class(RFT.ERS.Classes.RequestAttachment).%New()
			set req.url=url
			set req.attachmentID=attId
			set t=..SendRequestSync("DocumentDownloadProcess",req,.resp)
				
END					
			}	
			
		$$$LOGINFO("numbDocs :"_numbDocs)
		set ref.NumDocs=numbDocs
		set t=ref.%Save()
		IF '$$$ISERR(t) $$$TRACE("Referral saved to local list")
	}
	
	$$$LOGINFO("refResp :"_refResp)

	IF $FIND("200,422,404", refResp.StatusCode ) '= 0
	{
		//If api call returns at 404 or 422 we do not need to try again 
		//update status flag in worklist
		
		set workListItem=##class(RFT.ERS.Tables.Worklist).%OpenId(pInput.ubrn)
		set workListItem.statusFlag=1
		do workListItem.%Save()

	}		
	
	// Clinical Information
	set t=..SendRequestSync("HttpOut_ERS",pInput,.refResp)
	// 
	
	
	quit t
}

Method GenerateAlert(AlertText As %String)
{
	set t=$$$OK
	
	$$$LOGINFO("HELLO")	
	set alert=##class(Ens.AlertRequest).%New()
	set alert.AlertText=AlertText
	set alert.AlertDestination="Ens.Alert"
	set t=..SendAlert(alert)
	quit t
}

Storage Default
{
<Type>%Storage.Persistent</Type>
}

}

