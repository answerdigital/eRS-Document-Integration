Class RFT.ERS.Operation.ERS Extends Ens.BusinessOperation
{

Parameter ADAPTER = "EnsLib.HTTP.OutboundAdapter";

Property Adapter As EnsLib.HTTP.OutboundAdapter;

Parameter INVOCATION = "Queue";

Property FhirServer As %String [ InitialExpression = "https://server.fire.ly/r3" ];

Property ODSCode As %String [ InitialExpression = "B9J4U" ];

Parameter SETTINGS = "FhirServer,ODSCode";

Method RetrieveWorklist(pInput As RFT.ERS.Classes.RequestWorklist, Output pResponse As Ens.Response)
{
    set tsc=$$$OK
    
    //create body for http request
    set body={}
    set body.resourceType="Parameters"
    set body.meta={}
    set body.meta.profile=["https://fhir.nhs.uk/STU3/StructureDefinition/eRS-FetchWorklist-Parameters-1"]
    set body.parameter=[]
    
    set parameterObj={}
    set parameterObj.name="listType"
    set parameterObj.valueCodeableConcept={}
    set parameterObj.valueCodeableConcept.coding=[]
    set coding={}
    set coding.system="https://fhir.nhs.uk/STU3/CodeSystem/eRS-ReferralListSelector-1"
    set coding.code="APPOINTMENT_SLOT_ISSUES"
    do parameterObj.valueCodeableConcept.coding.%Push(coding)
    do body.parameter.%Push(parameterObj)

    set stream = ##class(%Stream.GlobalCharacter).%New()
    do stream.WriteLine(body.%ToJSON())
    $$$LOGINFO("body.%ToJSON() : "_body.%ToJSON())
    
    set httpReq=##class(%Net.HttpRequest).%New()
    set httpReq.EntityBody= stream
    set httpReq.Authorization = "Bearer "_^LastToken("auth_token")
    
    set tsc = httpReq.SetHeader("NHSD-End-User-Organisation-ODS",..ODSCode)
    set tsc = httpReq.SetHeader("NHSD-eRS-Business-Function","SERVICE_PROVIDER_CLINICIAN")
    set tsc = httpReq.SetHeader("Content-Type","application/fhir+json")

    set tURL=..Adapter.HTTPServer_"/referrals/FHIR/STU3/ReferralRequest/$ers.fetchworklist"
    SET tsc=..Adapter.SendFormDataURL(tURL, .tHttpResponse, "POST", httpReq)

   
    #DIM tHttpResponse As %Net.HttpResponse
    
    $$$TRACE("Status Code: "_ tHttpResponse.StatusLine)
    IF tHttpResponse.StatusCode'=200
    {
   
        set alert=##class(Ens.AlertRequest).%New()
        set alertText="Error Code: "_tHttpResponse.StatusCode
        set alertText=alertText_" There has been an error in sending the latest FHIR bundle to the UI"
        set alert.AlertText=alertText
        set alert.AlertDestination="Ens.Alert"
        set t=..SendAlert(alert)
        $$$ThrowStatus(tsc)
        
    }   
    
    //Extract values from fhir response 
    set data=tHttpResponse.Data
       
    IF $ISOBJECT(data)
    {
        set obj=[].%FromJSON(data)
        set resp=##class(RFT.ERS.Classes.WorklistResponse).%New()
        set entry=obj.entry
    }
    
    IF $ISOBJECT(entry)
    {
	    set entryIterator=entry.%GetIterator()
        while entryIterator.%GetNext(.key,.entryItem)
        {
            $$$TRACE("ubrn:"_entryItem.item.reference)
	   	    set ref=##class(RFT.ERS.Classes.WorklistItem).%New()
	        set ubrn=entryItem.item.reference
	        set ref.ubrn=$PIECE(ubrn,"/",2)
	        set outerExtensionArray=entryItem.extension
    
	        set extensionObject=outerExtensionArray.extension
    
    
	        set innerExtensionArray=extensionObject.extension
	        set innerExtensionIterator=innerExtensionArray.%GetIterator()
	        while innerExtensionIterator.%GetNext(.key,.extensionItem)
	        {
		        if extensionItem.url="priority" 
		    	{
		    		set code=##class(RFT.Utility.CodeableConcept).RetriveCode(extensionItem)
		    		set ref.priority=code
		    	}
		    	if extensionItem.url="specialty"
		    	{		
		    		set code=##class(RFT.Utility.CodeableConcept).RetriveCode(extensionItem)
		    		set ref.specialty=code
		    	}  
    
		    	if extensionItem.url="clinicalInfoPrinted" set ref.clinicalInfoPrinted=extensionItem.valueBoolean
                if extensionItem.url="clinicalInfoLastUpdated" set ref.clinicalInfoLastUpdate=extensionItem.valueDateTime
                if extensionItem.url="patient" set ref.patientID=extensionItem.valueReference.identifier.value    
	        } 
    
            do resp.Item.Insert(ref)
        }   
        
    }   
    set pResponse=resp
END 
    quit tsc
}

Method RetrieveReferralDetails(pInput As RFT.ERS.Classes.RequestReferralDetails, Output pResponse As Ens.Response)
{
    set tsc=$$$OK
    
    set httpReq=##class(%Net.HttpRequest).%New()
    set httpReq.Authorization = "Bearer "_^LastToken("auth_token")
    set tsc = httpReq.SetHeader("NHSD-eRS-Business-Function","SERVICE_PROVIDER_CLINICIAN")
    set tsc = httpReq.SetHeader("Accept","application/fhir+json")
    set tsc = httpReq.SetHeader("NHSD-End-User-Organisation-ODS",..ODSCode)
    
    set tURL=..Adapter.HTTPServer_"/referrals/FHIR/STU3/ReferralRequest/"_pInput.ubrn    
    
    SET tsc=..Adapter.SendFormDataURL(tURL, .httpResp, "GET", httpReq)
    #DIM httpResp as %Net.HttpResponse
    $$$TRACE("Status Code: "_ httpResp.StatusLine)
    
    set data=httpResp.Data
    
    IF $ISOBJECT(data)
    {
        set obj=[].%FromJSON(data)
        set resp=##class(RFT.ERS.Classes.ReferralDetailsResponse).%New()
        set refDetails=##class(RFT.ERS.Classes.ReferralDetails).%New()
        
        set extension=obj.extension
        set extIter=extension.%GetIterator()
        while extIter.%GetNext(.key,.extItem)
        
        {
            
          if extItem.url="https://fhir.nhs.uk/STU3/StructureDefinition/Extension-eRS-ReferralPriority-1"
           {
                //set refDetails.priority=extItem.valueCodeableConcept.coding.GetAt(1).code
                               
				set priorityCode=##class(RFT.Utility.CodeableConcept).RetriveCode(extItem)
				set refDetails.priority=priorityCode
				    
            }
                   
        }  
        
        set contained=obj.contained
        set containedIter=contained.%GetIterator()
        while containedIter.%GetNext(.key,.containedItem)
        {
            if containedItem.resourceType="Appointment"
            {
                set refDetails.status=containedItem.status
                
                IF containedItem.status="booked"
                {
	                set refDetails.apptStart=containedItem.start
               		 set refDetails.apptEnd=containedItem.end	                
                }  
                
            }
            
            if containedItem.resourceType="DocumentReference"
            {
                
                //grab attachment details
                //may be multiple attch objects inside content array
                set content=containedItem.content
                set contentIter=content.%GetIterator()
               	while contentIter.%GetNext(.key,.contentItem)
                {   
                    set attachment=##class(RFT.ERS.Classes.Attachment).%New()
                    
                    $$$TRACE("Found attachment id: "_contentItem.attachment.id)
                   
                    set attachment.attachmentID=contentItem.attachment.id
                    
        
                    set attachmentExt=contentItem.attachment.extension
                  
                    if attachmentExt.%Get(0).url="https://fhir.nhs.uk/STU3/StructureDefinition/Extension-eRS-AttachedBy-1"
                    {
                        set attachment.insertedBy=attachmentExt.%Get(0).valueReference.identifier.value
                    }
                    set attachment.contentType=contentItem.attachment.contentType
                    set attachment.url=contentItem.attachment.url
                    set attachment.size=contentItem.attachment.size
                    set attachment.title=contentItem.attachment.title
                    set attachment.createdDateTime=contentItem.attachment.creation

                    do resp.Attachment.Insert(attachment)
                        
                }
                
                set refDetails.docStatus=containedItem.status
                
            }      
            
        }
        
        set refDetails.intent=obj.intent
  		Set codingArray=obj.specialty.coding
  		
  		set codingIterator=codingArray.%GetIterator()
  		while codingIterator.%GetNext(.key,.item)
  		{
	  		set refDetails.specialty=item.code
	  		
  		}	
		//set refDetails.specialty=obj.specialty.coding
        set refDetails.patientID=obj.subject.identifier.value   
        
        set resp.Referral=refDetails
        
        set pResponse=resp
        
    
    }   
        
END
    quit tsc
}

Method RetrieveAttachment(pInput As RFT.ERS.Classes.RequestAttachment, Output pResponse As Ens.Response)
{
    
    set t = $$$OK
    
    set httpReq=##class(%Net.HttpRequest).%New()
    

    do httpReq.SetHeader("NHSD-eRS-Business-Function","SERVICE_PROVIDER_CLINICIAN")
    do httpReq.SetHeader("X-Correlation-ID","11C46F5F-CDEF-4865-94B2-0EE0EDCC26DA")
    do httpReq.SetHeader("NHSD-End-User-Organisation-ODS",..ODSCode)
    set httpReq.Authorization = "Bearer "_^LastToken("auth_token")
    
    set tURL=..Adapter.HTTPServer_"/referrals/FHIR/STU3/Binary/"_pInput.url    
    SET sc=..Adapter.SendFormDataURL(tURL, .httpResp, "GET", httpReq)
    #DIM httpResp as %Net.HttpResponse
    
    set response=##class(RFT.ERS.Classes.AttachmentResponse).%New()
    set response.StatusCode=httpResp.StatusCode
    
    
   $$$LOGINFO("StatusCode: "_httpResp.StatusCode)
    
    IF httpResp.StatusCode=200
    {
        // Get NHS Number for file naming
        Set sqlArgument = ##class(%DynamicObject).%New()
        Set sqlArgument.code = pInput.url
        Set sqlArgument.type = "attachmentLogicalID"

        // Get extension
        Set Qry = "SELECT REPLACE(title,' ', '') as TrimmedTitle FROM RFT_ERS_Tables.Attachments WHERE url ='"_pInput.url_"'"

        Set ST=##class(%SQL.Statement).%New()   
        set tsc = ST.%Prepare(Qry)
        if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit} 
        set rset = ST.%Execute()
        if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit}

        Set TrimmedTitle = ""
        while (rset.%Next()) && (TrimmedTitle = "")
        {
            set TrimmedTitle = $PIECE(rset.%Get("TrimmedTitle"),".",1)
            set TrimmedTitle = TrimmedTitle_"_"_$zstrip($zdatetime($horolog,3), "*P")
        }
        if TrimmedTitle = ""
        {
            write "fileExt Not found"
            quit
        }

        // File Extension
        set fileType=httpResp.GetHeader("Content-Type")
        set fileExt=##class(Ens.Util.FunctionSet).Lookup("ERS.ContentTypeToExtension",fileType)

        // Naming file
        Set PatientDetails = ##class(%DynamicObject).%New()
        Set tsc = ..LookUpPatient(sqlArgument, .PatientDetails)

        // create/check download directory "MM-YYYY"
        Set OutputDir = ##class(Ens.Director).GetItemSettingValue("DocumentDownloadProcess","Host","OutputDir",.status)_"\"_$E($ZDT($H,8),1,4)_"-"_$E($ZDT($H,8),5,6)
		Set OutputDirExists=##class(%File).DirectoryExists(OutputDir)
		If ('OutputDirExists) {set tsc=##class(%File).CreateDirectory(OutputDir)}

        // Save file
        set fileName=OutputDir_"\"_PatientDetails.MPI_"_"_PatientDetails.NHSnumber_"_"_PatientDetails.FamilyName _"_"_PatientDetails.GivenName_"_"_TrimmedTitle_"."_fileExt
        set stream=##class(%Stream.FileBinary).%New()
        set t=stream.LinkToFile(fileName)
        set t=stream.CopyFromAndSave(httpResp.Data)

        //add file path to local table 
        set attch=##class(RFT.ERS.Tables.Attachments).%OpenId(pInput.attachmentID)

        IF $ISOBJECT(attch)
        {
            set attch.filePath=fileName
            set t= attch.%Save()

            set response.FilePath=fileName
            set response.FileType=fileExt
        }

    }
    elseif httpResp.StatusCode = 401
    {
        set alert=##class(Ens.AlertRequest).%New()
        set alertText="Error Code: "_httpResp.StatusCode
        set alertText=alertText_" Retrieve attachment method failed due to token expiry."
        set alert.AlertText=alertText
        set alert.AlertDestination="Ens.Alert"
        set t=..SendAlert(alert)
        quit t
    }
    
    $$$TRACE(response.StatusCode)
    set pResponse=response
    
    quit t
}

Method RetrieveClincalInformation(pInput As RFT.ERS.Classes.RequestClinicalInformation, Output pResponse As Ens.Response)
{
    set tsc=$$$OK
    
    set httpReq=##class(%Net.HttpRequest).%New()
    set httpReq.Authorization = "Bearer "_^LastToken("auth_token")

    set tsc = httpReq.SetHeader("Accept","application/pdf")
    set tsc = httpReq.SetHeader("NHSD-eRS-Business-Function", "SERVICE_PROVIDER_CLINICIAN")
    set tsc = httpReq.SetHeader("NHSD-End-User-Organisation-ODS",..ODSCode)
    set tsc = httpReq.SetHeader("Content-Type","application/fhir+json")

    set ubrn = pInput.ubrn
    set tURL=..Adapter.HTTPServer_"/referrals/FHIR/STU3/ReferralRequest/"_ubrn_"/$ers.generateCRI"
    
    set StatusCode = 999
    Set Count = ##class(Ens.Director).GetItemSettingValue("DocumentDownloadProcess","Host","AttachmentRetryCount",.tsc)
    
    Try {
        while $FIND("200,404,422", StatusCode)=0 && (Count > 0)
        {
            $$$TRACE("counter: "_Count)
            SET tsc=..Adapter.SendFormDataURL(tURL, .tHttpResponse, "POST", httpReq)
            #DIM tHttpResponse As %Net.HttpResponse

            $$$LOGINFO("Status Code: "_ tHttpResponse.StatusCode)
            Set StatusCode = tHttpResponse.StatusCode
            Set Count = Count - 1
        }
        Set retryCount = ##class(Ens.Director).GetItemSettingValue("DocumentDownloadProcess","Host","AttachmentRetryCount",.tsc) - Count
    }
    Catch ex {
        set alert=##class(Ens.AlertRequest).%New()
        set alertText="Error Code: "_tHttpResponse.StatusCode
        set alertText=alertText_" There has been an error in etrieving clinical information (A007). ubrn: "_ubrn
        set alert.AlertText=alertText
        set alert.AlertDestination="Ens.Alert"
        set tsc=..SendAlert(alert)
        $$$ThrowStatus(tsc)
        Set tsc=ex.AsStatus()
    }

    if StatusCode = 200
    {
        set response=##class(RFT.ERS.Classes.AttachmentResponse).%New()
        set response.StatusCode=tHttpResponse.StatusCode
    
        set data=tHttpResponse.Data
        set respTitle = $PIECE(tHttpResponse.GetHeader("Content-Disposition"),"=",2)
    
        // Get NHS Number for file naming
        Set sqlArgument = ##class(%DynamicObject).%New()
        Set sqlArgument.code = ubrn
        Set sqlArgument.type = "ubrn"

        set stream=##class(%Stream.FileBinary).%New()

        Set PatientDetails = ##class(%DynamicObject).%New()
        Set tsc = ..LookUpPatient(sqlArgument, .PatientDetails)
        $$$LOGINFO("returned to RetrieveClincalInformation")

        // create/check download directory "MM-YYYY"
        Set OutputDir = ##class(Ens.Director).GetItemSettingValue("DocumentDownloadProcess","Host","OutputDir",.status)_"\"_$E($ZDT($H,8),1,4)_"-"_$E($ZDT($H,8),5,6)
		Set OutputDirExists=##class(%File).DirectoryExists(OutputDir)
		If ('OutputDirExists) {set tsc=##class(%File).CreateDirectory(OutputDir)}

	    set fileName=OutputDir_"\"_PatientDetails.MPI_"_"_PatientDetails.NHSnumber_"_"_PatientDetails.FamilyName _"_"_PatientDetails.GivenName_"_ClinicalSummary"
        set fileName=fileName_"_"_$zstrip($zdatetime($horolog,3), "*P")_".pdf"
        set response.FilePath=fileName
		// Copy into PDFs Folder
        Set TargetDir = ##class(Ens.Director).GetItemSettingValue("DocumentDownloadProcess","Host","PDFsFolder",.status)_"\"_$E($ZDT($H,8),1,4)_"-"_$E($ZDT($H,8),5,6)
        Set TargetDirExists=##class(%File).DirectoryExists(TargetDir)
        If ('TargetDirExists) {set tsc=##class(%File).CreateDirectory(TargetDir)}


        set tsc=stream.LinkToFile(fileName)
        set tsc=stream.CopyFromAndSave(data)
        set response.FileType="pdf"

	    set tsc=stream.LinkToFile(fileName)
	    set tsc=stream.CopyFromAndSave(data)

        set cmd= "xcopy "_fileName_" "_TargetDir
        SET runCmd=$ZF(-1,cmd)

        // update DB with clincal summary path
        set InsertQry = "INSERT INTO RFT_ERS_Tables.Attachments (createdDateTime, filePath, isDownloaded, "
        set InsertQry = InsertQry_"retryCount, size, title, ubrn)" ;, url)"
        set InsertQry = InsertQry_" VALUES ("
        set InsertQry = InsertQry_$ZDATE($PIECE($H,",",1))_", "
        set InsertQry = InsertQry_"'"_fileName_"', "
        set InsertQry = InsertQry_"1"_", "
        set InsertQry = InsertQry_retryCount_", "
        set InsertQry = InsertQry_"'"_stream.SizeGet()_"', "
        set InsertQry = InsertQry_"'"_respTitle_"', "
        set InsertQry = InsertQry_"'"_ubrn_"')"

        $$$TRACE(InsertQry)

        Set ST=##class(%SQL.Statement).%New()   
        set tsc = ST.%Prepare(InsertQry)
        if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit} 
        set rset = ST.%Execute()
        if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit}
    }

    if response
    {
        set pResponse=response
    }
    
    
    quit tsc
}

Method LookUpPatient(pInupt As %DynamicObject, Output PatientDetails As %DynamicObject) As %Status
{
    set tsc = $$$OK

    Try
    {
        set PatientDetails = ##class(%DynamicObject).%New()

        if pInupt.type '= "PatientID"
        {
            if pInupt.type = "attachmentLogicalID"
            {
                Set Qry = "SELECT w.patientID, w.ubrn, w.specialty FROM RFT_ERS_Tables.Worklist w INNER JOIN RFT_ERS_Tables.Attachments a ON w.ubrn = a.ubrn where a.url = '"_pInupt.code_"'"
            }
            elseif pInupt.type = "ubrn"
            {
                Set Qry = "SELECT patientID, ubrn, specialty FROM RFT_ERS_Tables.Worklist where ubrn='"_pInupt.code_"'"
            }

            Set ST=##class(%SQL.Statement).%New()   
            set tsc = ST.%Prepare(Qry)
            if $$$ISERR(tsc) {$$$ThrowStatus(tsc)}
            set rset = ST.%Execute()
            if $$$ISERR(tsc) {$$$ThrowStatus(tsc)}

            Set PatientID = ""
            while (rset.%Next()) && (PatientID = "")
            {
                set PatientID = rset.%Get("patientID")
                set ubrn = rset.%Get("ubrn")
                set specialty = rset.%Get("specialty")
                $$$TRACE("PatientID :"_PatientID)
                $$$TRACE("ubrn :"_ubrn)
                $$$TRACE("specialty :"_specialty)
            }
            if PatientID = ""
            {
                write "PatientID Not found"
                quit
            }
        }
        elseif pInupt.type = "PatientID"
        {
            set PatientID = pInupt.code
        }

        $$$TRACE("LOOKUP PATIENT : "_PatientID)

        set PatientDetails.NHSnumber = PatientID

        // Get Patient resource from FHIR Server
        set httpReq=##class(%Net.HttpRequest).%New()
        set tURL=..FhirServer_"/Patient?identifier="_PatientID
        set tsc=..Adapter.SendFormDataURL(tURL, .tHttpResponse, "GET", httpReq)

	    set response=##class(RFT.ERS.Classes.HttpResponse).%New()

	    set response.StatusCode= tHttpResponse.StatusCode
	    set response.ResponseBody=tHttpResponse.Data.Read()
        $$$TRACE("response.ResponseBody :"_response.ResponseBody)

        set RespObj = ##class(%DynamicAbstractObject).%FromJSON(response.ResponseBody)
    
	    if response.StatusCode = 200
	    {
	        if RespObj.total = 0
            {
                set alert=##class(Ens.AlertRequest).%New()
                set alertText="Cannot perform the patient demographics look-up process, no records were found in the FHIR server for patient: "_PatientID
                set alert.AlertText=alertText
                set alert.AlertDestination="Ens.Alert"
                set tsc=..SendAlert(alert)
                $$$ThrowStatus(tsc)
            }

            set IdentifierArray = RespObj.entry.%Get(0).resource.identifier
            // Extracting MPI
            set MPI = ""
            set iter = IdentifierArray.%GetIterator()
            While iter.%GetNext(.key , .value)
            {
                if IdentifierArray.%Get(key).system = "https://fhir.nhs.uk/Id/local-patient-identifier"
                {
                    Set MPI = IdentifierArray.%Get(key).value
                }
            }

            if MPI = ""
            {
                write "MPI Not found"
                quit
            }

            set PatientDetails.MPI = MPI
            // Extracting name
            set NameArray = RespObj.entry.%Get(0).resource.name
            set GivenName = ""
            set FamilyName = ""
            set iter = NameArray.%GetIterator()
            While iter.%GetNext(.key , .value)
            {
                if NameArray.%Get(key).use = "official"
                {
                    set GivenNameArray = NameArray.%Get(key).given
                    set iterGN = GivenNameArray.%GetIterator()
                    While iterGN.%GetNext(.keyName , .value)
                    {
                        // GiveName
                        if GivenName '="" {Set GivenName = GivenName_"-"_$ZCONVERT(GivenNameArray.%Get(keyName), "W")}
                        else {Set GivenName = $ZCONVERT(GivenNameArray.%Get(keyName), "W")}
                    }

                    // FamilyName
                    if FamilyName '="" {Set FamilyName = FamilyName_"-"_$ZCONVERT(NameArray.%Get(key).family, "U")}
                    else {Set FamilyName = $ZCONVERT(NameArray.%Get(key).family, "U")}
                }
            }

            if (GivenName = "") || (FamilyName = "") {$$$ThrowStatus(tsc)}

            set PatientDetails.GivenName = GivenName
            set PatientDetails.FamilyName = FamilyName
            set PatientDetails.Gender = $ZCONVERT($EXTRACT(RespObj.entry.%Get(0).resource.gender), "W") //Gender
            set PatientDetails.dob = RespObj.entry.%Get(0).resource.birthDate //dob

            // Address
            set AddressArray = RespObj.entry.%Get(0).resource.address
            set Address1 = ""
            set Address2 = ""
            set Address3 = ""
            set iterAd = AddressArray.%GetIterator()
            While iterAd.%GetNext(.keyAddress , .value)
            {
                set keyhomeAdd = -1
                if AddressArray.%Get(keyAddress).use = "home" {set keyhomeAdd = keyAddress}
            }

            if keyhomeAdd >= 0
            {
                set AddLineArray = AddressArray.%Get(keyhomeAdd).line
                set postalCode = AddressArray.%Get(keyhomeAdd).postalCode
            }
            else
            {
                set AddLineArray = AddressArray.%Get(0).line
                set postalCode = AddressArray.%Get(0).postalCode

            }

            set iterLn = AddLineArray.%GetIterator()
            While iterLn.%GetNext(.keyLine , .value)
            {
                if keyLine = 0 {set Address1 = AddLineArray.%Get(keyLine)}
                elseif keyLine = 1 {set Address2 = AddLineArray.%Get(keyLine)}
                elseif keyLine = 2 {set Address3 = AddLineArray.%Get(keyLine)}
                elseif keyLine > 2 {set Address3 = Address3_" "_AddLineArray.%Get(keyLine)}
            }

            Set PatientDetails.Address1 = Address1
            Set PatientDetails.Address2 = Address2
            Set PatientDetails.Address3 = Address3
            Set PatientDetails.postalCode = postalCode

            //Phone
            set TelecomArray = RespObj.entry.%Get(0).resource.telecom
            set iterTC = TelecomArray.%GetIterator()
            set mobile = 0
            set home = 0
            set work = 0

            While iterTC.%GetNext(.keyTC , .value)
            {
                if (TelecomArray.%Get(keyTC).use = "mobile")
                {
                    set mobile = $ZSTRIP(TelecomArray.%Get(keyTC).value,"*A")
                    set mobile = $ZSTRIP(mobile,"*P")
                }
                elseif TelecomArray.%Get(keyTC).use = "home"
                {
                    set home = $ZSTRIP(TelecomArray.%Get(keyTC).value,"*A")
                    set home = $ZSTRIP(home,"*P")
                }
                elseif TelecomArray.%Get(keyTC).use = "work"
                {
                    set work = $ZSTRIP(TelecomArray.%Get(keyTC).value,"*A")
                    set work = $ZSTRIP(work,"*P")
                }
                elseif TelecomArray.%Get(keyTC).use = "temp"
                {
                    set temp = $ZSTRIP(TelecomArray.%Get(keyTC).value,"*A")
                    set temp = $ZSTRIP(temp,"*P")
                }
            }
            if $Length(mobile) > 9 {set PatientDetails.phone = mobile}
            elseif $Length(home) > 9 {set PatientDetails.phone = home}
            elseif $Length(work) > 9 {set PatientDetails.phone = work}
            elseif $Length(temp) > 9 {set PatientDetails.phone = temp}
            else {set PatientDetails.phone = ""}
	    }

        $$$TRACE("LOOKUP OK")

    }

    Catch
    {

    }

    

    quit tsc
}

Method ExtractPatientID(Input As Ens.StringContainer, Output DmgContainer As Ens.StringContainer) As %Status
{
    set tsc = $$$OK
    $$$TRACE(Input.StringValue)
    set PatientID = ##class(%DynamicAbstractObject).%FromJSON(Input.StringValue)
    $$$TRACE(PatientID.code)
    $$$TRACE(PatientID.type)

    Set tsc = ..LookUpPatient(PatientID, .PatientDetails)
    Set DmgContainer = ##class(Ens.StringContainer).%New()
	Set DmgContainer.StringValue = PatientDetails.%ToJSON() 

    quit tsc
}

XData MessageMap
{
<MapItems>
<MapItem MessageType="RFT.ERS.Classes.RequestWorklist">
<Method>RetrieveWorklist</Method>
</MapItem>
<MapItem MessageType="RFT.ERS.Classes.RequestReferralDetails">
<Method>RetrieveReferralDetails</Method>
</MapItem>
<MapItem MessageType="RFT.ERS.Classes.RequestClinicalInformation">
<Method>RetrieveClincalInformation</Method>
</MapItem>
<MapItem MessageType="RFT.ERS.Classes.RequestAttachment">
<Method>RetrieveAttachment</Method>
</MapItem>
<MapItem MessageType="Ens.StringContainer">
<Method>ExtractPatientID</Method>
</MapItem>
</MapItems>
}

}

