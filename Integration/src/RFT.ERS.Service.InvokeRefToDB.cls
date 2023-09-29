Class RFT.ERS.Service.InvokeRefToDB Extends Ens.BusinessService
{

Parameter ADAPTER = "Ens.InboundAdapter";

Method OnProcessInput(pInput As %RegisteredObject, Output pOutput As %RegisteredObject) As %Status
{

	set t = $$$OK

	// Referral to DB
	set ref="" set attchCount="" set downloadCount=""
	
	&SQL(DECLARE DocCursor CURSOR FOR SELECT RFT_ERS_Tables.Referrals.ubrn,(Select Count(*) from RFT_ERS_Tables.Attachments 
	where RFT_ERS_Tables.Attachments.ubrn = RFT_ERS_Tables.Referrals.ubrn)
	,(Select Count(*) from RFT_ERS_Tables.Attachments where RFT_ERS_Tables.Attachments.ubrn = RFT_ERS_Tables.Referrals.ubrn and RFT_ERS_Tables.Attachments.isDownloaded = 1)
	into:ref,:attchCount,:downloadCount from RFT_ERS_Tables.Referrals
	Where NumDocs = (Select Count(*) from RFT_ERS_Tables.Attachments 
	where RFT_ERS_Tables.Attachments.ubrn = RFT_ERS_Tables.Referrals.ubrn and RFT_ERS_Tables.Attachments.isDownloaded = 1) 
	AND RFT_ERS_Tables.Referrals.statusFlag=0)
	
	&SQL(OPEN DocCursor)
	
	FOR {&SQL(FETCH DocCursor)	
		
		QUIT:SQLCODE
		set req=##class(RFT.ERS.Classes.RefToDB).%New()
		set req.ubrn=ref
		set req.attachmentCount=attchCount
		set req.downloadCount=downloadCount
		set t=..SendRequestSync("ReferralToDB",req)
				
	}
	&SQL(CLOSE DocCursor)

	// Patient to DB
	Try
	{
		Set Qry = "SELECT DISTINCT patientID FROM RFT_ERS_Tables.Worklist WHERE RFT_ERS_Tables.Worklist.UpdatePatientDB = 0"
		Set ToPatientDmg = ##class(%DynamicObject).%New()
		Set ST=##class(%SQL.Statement).%New()

   		set t = ST.%Prepare(Qry)
   		if $$$ISERR(t) {$$$ThrowStatus(t)}

   		set rset = ST.%Execute()
   		if $$$ISERR(t) {$$$ThrowStatus(t)}

   		while rset.%Next()
   		{
   		    set ToPatientDmg.code = rset.%Get("patientID")
				$$$TRACE(ToPatientDmg.code)

   		    set ToPatientDmg.type = "PatientID"
				Set Container = ##class(Ens.StringContainer).%New()
				Set Container.StringValue = ToPatientDmg.%ToJSON()
				set t=..SendRequestSync("ReferralToDB",Container)
   		}
		
	}
	Catch ex {
		Set t=ex.AsStatus()
	}
	
	quit t
}

}

