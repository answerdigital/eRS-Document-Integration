Class RFT.ERS.Service.InvokeReferralProcessor Extends Ens.BusinessService
{

Parameter ADAPTER = "Ens.InboundAdapter";

Method OnProcessInput(pInput As %RegisteredObject, Output pOutput As %RegisteredObject) As %Status
{

	set t=$$$OK
	
	//Check if any new referrals in local list 
	
	set ubrn=""
	&SQL(DECLARE DocCursor CURSOR FOR SELECT ubrn into:ubrn FROM RFT_ERS_Tables.Worklist WHERE statusFlag='0')
	
	&SQL(OPEN DocCursor)
	
	FOR {&SQL(FETCH DocCursor)	
		
		QUIT:SQLCODE
		set req=##class(RFT.ERS.Classes.RequestReferralDetails).%New()
		set req.ubrn=ubrn
	
		set t=..SendRequestSync("ReferralProcessor",req)
		set t=..SendRequestSync("ClinicalInformationDownloadProcessor",req)
		
		IF '$$$ISERR(t)
		{
			set workListItem=##class(RFT.ERS.Tables.Worklist).%OpenId(ubrn)
			set workListItem.statusFlag=1
			do workListItem.%Save()

		}	
		
			
				
	}
	&SQL(CLOSE DocCursor)
	
	
	
		
	quit t
}

}

