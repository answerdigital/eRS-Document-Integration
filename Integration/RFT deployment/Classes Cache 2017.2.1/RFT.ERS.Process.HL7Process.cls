Class RFT.ERS.Process.HL7Process Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Method OnRequest(pRequest As EnsLib.HL7.Message, Output pResponse As Ens.Response) As %Status
{

	set t=$$$OK
	
	Set msg=pRequest
	set nhs="" set mrn=""

	//Loop through PID to get NHS && MRN
	set x=msg.FindSegmentValues("PID:3(*)")
	for i=1:1:x
		{
			
			if msg.FindSegmentValues("PID:3("_i_").5")="SS"
			{
				set nhs=msg.FindSegmentValues("PID:3("_i_").1")
					
			}
				if msg.FindSegmentValues("PID:3("_i_").5")="MR"
			{
				set mrn=msg.FindSegmentValues("PID:3("_i_").1")
					
			}
			
			
	}
	$$$TRACE("NHS NUMBER : "_nhs)	
		
	IF nhs=""
	{	
		$$$TRACE("NO NHS NUMBER FOUND. DISCARDING MESSAGE")
		GOTO END
	}	
	
	//Check NHS Numb against worklist to see if we need to send to DB
	
	set ubrn="" set specialty=""
	&sql(SELECT ubrn,specialty into :ubrn,:specialty FROM RFT_ERS_Tables.Worklist WHERE patientID=:nhs)
	
	IF ubrn=""
	{
		$$$TRACE("PATIENT NOT ON WORKLIST. DISCARDING MESSAGE. NHS NUMBER : "_nhs)
		GOTO END
	}	
	
	$$$TRACE("Patient exists on worklist. NHS NUMBER : "_nhs)
	
	//Get message type
	
	set messageType=msg.FindSegmentValues("MSH:9.2")
	
	IF messageType="S12"
	{
		
		$$$TRACE("EXTRACTING VALUES FROM HL7")
		set lastName=msg.FindSegmentValues("PID:5(1).1.1")
		set firstName=msg.FindSegmentValues("PID:5(1).2")
		set sex=msg.FindSegmentValues("PID:8")
		set dob=msg.FindSegmentValues("PID:7.1")
		set addressOne=msg.FindSegmentValues("PID:11.1")
		set addressTwo=msg.FindSegmentValues("PID:11.2")
		set addressThree=msg.FindSegmentValues("PID:11.3")
		set postcode=msg.FindSegmentValues("PID:11.5")
		set contactNumber=msg.FindSegmentValues("PID:13.1")
		
		set qry="INSERT INTO patients (pat_ubrn,pat_mrn,pat_nhs,pat_familyName,pat_givenName,pat_sex,pat_dob,"
		set qry=qry_"pat_addressOne,pat_addressTwo,pat_addressThree,pat_PostCode,pat_contactNumber,pat_speciality,"
		set qry=qry_"rec_Updated,rec_UpdatedBy) VALUES ('"_ubrn_"','"_mrn_"','"_nhs_"','"_lastName_"','"_firstName_"','"
		set qry=qry_sex_"','"_dob_"','"_addressOne_"','"_addressTwo_"','"_addressThree_"','"_postcode_"','"_contactNumber_"','"_specialty_"','"
		set qry=qry_$zdatetime($horolog,3)_"','TIE')"	
		quit t
		
	}
	
			

	
	
	

	

		
	
END	
	quit t
}

Storage Default
{
<Type>%Storage.Persistent</Type>
}

}

