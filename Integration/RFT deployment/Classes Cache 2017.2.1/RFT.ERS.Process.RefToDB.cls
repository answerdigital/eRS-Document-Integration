Class RFT.ERS.Process.RefToDB Extends Ens.BusinessProcess [ ClassType = persistent ]
{

// Method OnRequest(pInput As RFT.ERS.Classes.RefToDB, Output pResponse As Ens.Response) As %Status

Method OnRequest(pInput As Ens.Request, Output pResponse As Ens.Response) As %Status
{
	set t=$$$OK 

	Set ClassType = $CLASSNAME(pInput)
	$$$LOGINFO("ClassType : "_ClassType)

	if ClassType= "RFT.ERS.Classes.RefToDB"
	{
		set ubrn=pInput.ubrn
	
		set referral=##class(RFT.ERS.Tables.Referrals).%OpenId(ubrn)

		$$$LOGINFO(referral)

		IF $ISOBJECT(referral)
		{
			//Insert referral into external DB 
			;set refQry="INSERT INTO ersdocs_workflow.ersRefReq_Details "
			set refQry="INSERT INTO dbo.ersRefReq_Details "
			set refQry=refQry_"(RefReq_UniqueID,RefReq_NHSNo,RefReq_UBRN,RefReq_TrustNACS,Appt_StDttm,Appt_EndDttm,RefReq_Specialty,RefReq_Status,"
			set refQry=refQry_"RefReq_intent,RefReq_Priority,RefReq_Noofdocs,RefReq_FullURL,rec_UpdatedBy,rec_insertedBy) "
			set refQry=refQry_"VALUES ('"_ubrn_"','"_referral.patientID_"','"_referral.ubrn_"','RFR','"_referral.apptStart_"','"_referral.apptEnd_"','"_referral.specialty_"','"
			set refQry=refQry_referral.status_"','"_referral.intent_"','"_referral.priority_"','"_referral.NumDocs_"','"
			set refQry=refQry_##class(Ens.Director).GetAdapterSettingValue("HttpOut_ERS","URL",.status)_"/referrals/FHIR/STU3/ReferralRequest/"_ubrn_"','TIE','TIE')"
			$$$LOGINFO(refQry)

			set insertRef=##class(Ens.StringContainer).%New()
			set insertRef.StringValue=refQry
			set t=..SendRequestSync("ERS_DB_UPDATE",insertRef)

			//get doc status
			set docStatus=referral.docStatus

			set attachmentID="" set contentType="" set createdDateTime="" set insertedBy="" set size="" 
			set title="" set url="" set filePath=""

			//Get all attachment details 
			//We will use embedded sql and doc cursor to ensure we grab all records
			&sql(DECLARE DocCursor CURSOR FOR SELECT attachmentID, contentType, createdDateTime, insertedBy, size, title, url,filePath
			into :attachmentID,:contentType,:createdDateTime,:insertedBy,:size,:title,:url,:filePath FROM RFT_ERS_Tables.Attachments
			WHERE isDownloaded=1 )

			&sql(OPEN DocCursor)			

			FOR {&SQL(FETCH DocCursor)

				$$$LOGINFO("here")

				QUIT:SQLCODE	
				set attchQry="INSERT INTO dbo.ersdoc_attachments " 
				set attchQry=attchQry_"(RefDoc_UniqueID,RefDocStatus,Attach_ID,Attach_InsertedBy,Attach_ContentType,Attach_URL,Attach_Size,Attach_Title,Attach_CrtdDTTM,"
				set attchQry=attchQry_"DocDownloadURL,DocLocationURI,rec_Updated) "
				set attchQry=attchQry_"VALUES ('"_ubrn_"','"_docStatus_"','"_attachmentID_"','"_insertedBy_"','"
				set attchQry=attchQry_contentType_"','"_url_"','"_size_"','"_title_"','"_createdDateTime_"','"
				set attchQry=attchQry_##class(Ens.Director).GetAdapterSettingValue("HttpOut_ERS","URL",.status)_"/referrals/FHIR/STU3/Binary/"
				set attchQry=attchQry_url_"','"_filePath_"','"_$zdatetime($horolog,3)_"')"
				$$$LOGINFO(attchQry)

				set insertAttachment=##class(Ens.StringContainer).%New()
				set insertAttachment.StringValue=attchQry
				set t=..SendRequestSync("ERS_DB_UPDATE",insertAttachment)
				}

			&sql(CLOSE DocCursor)

		}	

	}
	
	elseif ClassType = "Ens.StringContainer"
	{
		//Extract Dynamic object from StreamContainter
		// Lookup patient demographics from FHIR server
		set t=..SendRequestSync("HttpOut_ERS",pInput, .DmgContainer)
    	set PatientDetails = ##class(%DynamicAbstractObject).%FromJSON(DmgContainer.StringValue)


		// Build query to fetch patientID (NHS Number)
		//Check if nhs number and DOB exist
    	Set Qry = "SELECT COUNT(*) FROM dbo.patients WHERE pat_nhs = '"_PatientDetails.NHSnumber_"' and pat_dob = '"_PatientDetails.dob_"';"
    	//update if nhs number exists
    	$$$TRACE("MPI:"_PatientDetails.MPI)
    	Set Qry = Qry_"UPDATE dbo.patients SET pat_mrn = '"_PatientDetails.MPI
    	Set Qry = Qry_"', pat_familyName = '"_PatientDetails.FamilyName_"', pat_givenName = '"_PatientDetails.GivenName_"', pat_sex = '"_PatientDetails.Gender
    	Set Qry = Qry_"', pat_addressOne = '"_PatientDetails.Address1_"', pat_addressTwo = '"_PatientDetails.Address2_"', pat_addressThree = '"_PatientDetails.Address3
    	Set Qry = Qry_"', pat_PostCode = '"_PatientDetails.postalCode
		Set Qry = Qry_"', pat_contactNumber = '"_PatientDetails.phone
    	Set Qry = Qry_"' WHERE pat_nhs = '"_PatientDetails.NHSnumber_"' and rec_UpdatedBy = 'TIE'; "
    	//Insert if nhs number is new
    	Set Qry = Qry_"INSERT INTO dbo.patients (pat_mrn, pat_nhs, pat_familyName, pat_givenName, pat_sex, pat_dob, pat_addressOne, pat_addressTwo, pat_addressThree, "
    	Set Qry = Qry_"pat_PostCode, pat_contactNumber, rec_UpdatedBy) "
    	Set Qry = Qry_"SELECT '"_PatientDetails.MPI_"', '"_PatientDetails.NHSnumber_"', '"_PatientDetails.FamilyName_"', '"_PatientDetails.GivenName_"', '"_PatientDetails.Gender
    	Set Qry = Qry_"', '"_PatientDetails.dob_"', '"_PatientDetails.Address1_"', '"_PatientDetails.Address2_"', '"_PatientDetails.Address3_"', '"_PatientDetails.postalCode
    	Set Qry = Qry_"', '"_PatientDetails.phone_"', 'TIE'"
    	Set Qry = Qry_" WHERE NOT EXISTS (SELECT * FROM dbo.patients WHERE pat_nhs = '"_PatientDetails.NHSnumber_"' and pat_dob = '"_PatientDetails.dob_"');"

		// Send query to ODBC operation
		set insertAttachment=##class(Ens.StringContainer).%New()
		set insertAttachment.StringValue=Qry
		$$$LOGINFO("insertAttachment.StringValue : "_insertAttachment.StringValue)

    	Try
		{
			set t=..SendRequestSync("ERS_DB_UPDATE",insertAttachment)

			Set QryWL = "SELECT * FROM RFT_ERS_Tables.Worklist where patientID='"_PatientDetails.NHSnumber_"'"

			Set ST=##class(%SQL.Statement).%New()   
        	set tsc = ST.%Prepare(QryWL)
        	if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit}
        	set rset = ST.%Execute()
        	if $$$ISERR(tsc) {write "%Prepare failed:" do $SYSTEM.Status.DisplayError(tsc) quit}

        	while rset.%Next()
        	{
				set workListItem=##class(RFT.ERS.Tables.Worklist).%OpenId(rset.%Get("ID"))
				set workListItem.UpdatePatientDB = 1
				do workListItem.%Save()
        	}

		}
    	Catch ex {
    	    if $$$ISERR(t)
			{
				$$$LOGERROR("Update patients demographics failed")
			}
    	    $$$ThrowStatus(t)
    	    Set t=ex.AsStatus()
    	}	


	}
	

	
	
		
	
	
	q t
}

Storage Default
{
<Type>%Library.CacheStorage</Type>
}

}
