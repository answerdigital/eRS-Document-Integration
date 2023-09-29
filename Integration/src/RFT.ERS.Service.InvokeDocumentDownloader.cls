Class RFT.ERS.Service.InvokeDocumentDownloader Extends Ens.BusinessService
{

Parameter ADAPTER = "Ens.InboundAdapter";

Method OnProcessInput(pInput As %RegisteredObject, Output pOutput As %RegisteredObject) As %Status
{

	set t=$$$OK
	
	set url=""
	&SQL(DECLARE DocCursor CURSOR FOR SELECT url into:url FROM RFT_ERS_Tables.Attachments WHERE isDownloaded='0')
	
	&SQL(OPEN DocCursor)
	
	FOR {&SQL(FETCH DocCursor)	
		
		QUIT:SQLCODE
		set req=##class(RFT.ERS.Classes.RequestAttachment).%New()
		set req.url=url
	
		set t=..SendRequestSync("DocumentDownloadProcess",req)
				
	}
	&SQL(CLOSE DocCursor)
	
	
	
		
	quit t
}

}

