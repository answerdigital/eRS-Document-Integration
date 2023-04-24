Class RFT.ERS.Process.DocumentProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Property LibreOffice As %String [ InitialExpression = "C:\Program Files\LibreOffice\program\soffice.exe" ];

Property AllowedFiles As %String [ InitialExpression = "docx,doc,txt,rtf,tif,jpeg" ];

Property OutputDir As %String(MAXLEN = 200) [ InitialExpression = "C:\" ];

Property PDFsFolder As %String(MAXLEN = 200) [ InitialExpression = "C:\" ];

Property AttachmentRetryCount As %Integer [ InitialExpression = "5" ];

Parameter SETTINGS = "LibreOffice,OutputDir,PDFsFolder,AttachmentRetryCount,AllowedFiles";

Method OnRequest(pInput As RFT.ERS.Classes.RequestAttachment, Output pResponse As Ens.Response) As %Status
{

	set t=$$$OK
	

	set t=..SendRequestSync("HttpOut_ERS",pInput,.resp)
	#DIM resp As RFT.ERS.Classes.AttachmentResponse
	set attach=##class(RFT.ERS.Tables.Attachments).%OpenId(pInput.attachmentID)
	$$$LOGINFO("pInput.attachmentID : "_pInput.attachmentID)
	$$$LOGINFO("resp.StatusCode : "_resp.StatusCode)
	
	IF (resp.StatusCode=200) && (resp.FilePath'="")
	{
		
		set filePath=resp.FilePath
		set fileType=resp.FileType

		$$$TRACE("filePath / fileType: "_filePath_"/"_fileType)
		
        // create/check PDFs directory "MM-YYYY"
		Set TargetDir = ..PDFsFolder_"\"_$E($ZDT($H,8),5,6)_"-"_$E($ZDT($H,8),1,4)
		Set TargetDirExists=##class(%File).DirectoryExists(TargetDir)
		If ('TargetDirExists) {set tsc=##class(%File).CreateDirectory(TargetDir)}

		if fileType = "pdf"
		{
			set cmd= "copy "_filePath_" "_TargetDir
			SET runCmd=$ZF(-1,cmd)
		}

		elseif fileType'="pdf"
		{
			set cmd=""""_..LibreOffice_""""_" --headless --writer --convert-to pdf "_filePath_" --outdir "_TargetDir
			SET runCmd=$ZF(-1,cmd)
		}
			
		IF ##class(%File).Exists(filePath) ;_".pdf")
		{
			//update local list
			$$$TRACE("PDF File saved. Attchment ID: "_pInput.attachmentID)	
				
			IF $ISOBJECT(attach)
			{
				$$$TRACE("Updating local list. attach.filePath : "_attach.filePath)
				set attach.isDownloaded=1
				do attach.%Save()
			}
								
			}			
	}
	ELSE
	{
		//set retry count
		set attach.retryCount=attach.retryCount+1
		
		IF attach.retryCount>=..AttachmentRetryCount
		{
			$$$LOGWARNING("Reached retry max - deleting from local list")
			do attach.%Delete()
			do attach.%Save()
			
		}		
		
	}	

	quit t
}

Storage Default
{
<Data name="DocumentProcessorDefaultData">
<Subscript>"DocumentProcessor"</Subscript>
<Value name="1">
<Value>LibreOffice</Value>
</Value>
<Value name="2">
<Value>AllowedFiles</Value>
</Value>
<Value name="3">
<Value>OutputDir</Value>
</Value>
<Value name="4">
<Value>PDFsFolder</Value>
</Value>
<Value name="5">
<Value>AttachmentRetryCount</Value>
</Value>
</Data>
<DefaultData>DocumentProcessorDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

