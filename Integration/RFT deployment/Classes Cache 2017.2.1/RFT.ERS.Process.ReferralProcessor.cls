Class RFT.ERS.Process.ReferralProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Method OnRequest(pInput As RFT.ERS.Classes.RequestReferralDetails, Output pResponse As Ens.Response) As %Status
{
	set t=$$$OK
	$$$TRACE(pInput.ubrn)
	
	//Query ERS API for referral details
	set t=..SendRequestSync("HttpOut_ERS",pInput,.refResp)
	#DIM refResp As RFT.ERS.Classes.ReferralDetailsResponse

	IF t
	{
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
			
		$$$LOGINFO(numbDocs)
		set ref.NumDocs=numbDocs
		set t=ref.%Save()
		IF '$$$ISERR(t) $$$TRACE("Referral saved to local list")
	}
	
	$$$LOGINFO("refResp :"_refResp)
	
	;IF refResp.StatusCode = "200,422,404"
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
<Type>%Library.CacheStorage</Type>
}

}

