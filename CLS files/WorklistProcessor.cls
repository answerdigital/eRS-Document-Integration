<?xml version="1.0" encoding="UTF-8"?>
<Export generator="IRIS" version="26" zv="IRIS for Windows (x86-64) 2023.1.1 (Build 380U)" ts="2023-08-25 15:44:12">
<Class name="RFT.ERS.Process.WorklistProcessor">
<ClassType>persistent</ClassType>
<Super>Ens.BusinessProcess</Super>
<TimeChanged>66711,38544.8217866</TimeChanged>
<TimeCreated>66392,43790.9226508</TimeCreated>

<Method name="OnRequest">
<FormalSpec>pRequest:Ens.Request,*pResponse:Ens.StringContainer</FormalSpec>
<ReturnType>%Status</ReturnType>
<Implementation><![CDATA[

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
				
				//get username
				set Username = $USERNAME
	 			$$$TRACE("Username "_Username)
	 			
				//code seg to create Audit event on response from server
				Set ProdNS = $SYSTEM.SYS.NameSpace()
				$$$TRACE(ProdNS)
				Set $Namespace = "%SYS" ; Switch to the target namespace
				//source, type, name
				set AuditEvent = ##class(Security.Events).Exists(Username, "WorkList Retrieval", "ERSResponse")
				Set $Namespace = ProdNS ; Switch back to the original namespace
				$$$LOGINFO("AuditEvent Exists? 1 = yes, 0 = no"_AuditEvent)
				
				//create audit event if doesn't exist
				if AuditEvent = 0 {
				Set $Namespace = "%SYS" ; Switch to the target namespace
				Set status = ##class(Security.Events).Create(Username,"WorkList Retrieval","ERSResponse","Description", 1)
				Set $Namespace = ProdNS ; Switch back to the original namespace 
				}
				
				//format NHS Number
				set patientID = item.patientID
				If $Length(patientID) = 10 {
    				Set NHSNum = $E(patientID, 1, 3)_" "_$E(patientID, 4, 6)_" "_$E(patientID, 7, 9)_" "_$E(patientID, 10)
    				Write NHSNum
				} Else {
    				Write "Invalid input number"
				}
				//format UBRN
				set UBRN = item.ubrn
				If $Length(UBRN) = 12 {
    				Set formattedUBRN = $E(UBRN, 1, 4)_" "_$E(UBRN, 5, 8)_" "_$E(UBRN, 9, 12)
    				Write formattedUBRN
				} Else {
   					 Write "Invalid input number"
				}
	
	
				set EndUserID = ^LastToken("nhsid_useruid")
	 			
				$$$TRACE("EndUserID "_EndUserID)
				$$$TRACE("patientID: "_NHSNum)
				$$$TRACE("UBRN :"_formattedUBRN)
	
				//print created audit event to audit DB
				Set status = ##class(RFT.ERS.Classes.AuditClass).PerformAction(Username, "ERSResponse", "Description: NHS Number: "_NHSNum_" UBRN: "_formattedUBRN_" EndUserID: "_EndUserID)
				If $$$ISERR(status) {
    			$$$LOGINFO("error updating audit")
				}
				
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
]]></Implementation>
</Method>

<Storage name="Default">
<Type>%Storage.Persistent</Type>
</Storage>
</Class>
</Export>
