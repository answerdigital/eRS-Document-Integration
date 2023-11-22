Class RFT.ERS.Process.DocumentProcessor Extends Ens.BusinessProcess [ ClassType = persistent ]
{

Property LibreOffice As %String [ InitialExpression = "C:\Program Files\LibreOffice\program\soffice.exe" ];

Property AllowedFiles As %String [ InitialExpression = "docx,doc,txt,rtf,tif,jpeg" ];

Property OutputDir As %String(MAXLEN = 200) [ InitialExpression = "C:\" ];

Property PDFsFolder As %String(MAXLEN = 200) [ InitialExpression = "C:\" ];

Property DocumentStorageMode As %String(VALUELIST = ",MOUNTHLY,UBRN");

Property AttachmentRetryCount As %Integer [ InitialExpression = 5 ];

// Property VirusScanCommand As %String [ InitialExpression = "%PROGRAMFILES%\Windows Defender\MPCMDRUN.exe" ];

Parameter SETTINGS = "LibreOffice,OutputDir,PDFsFolder,DocumentStorageMode,AttachmentRetryCount,AllowedFiles";

Method OnRequest(pInput As RFT.ERS.Classes.RequestAttachment, Output pResponse As Ens.Response) As %Status
{
	set t=$$$OK
	$$$TRACE("START DOWNLOAD PROCESS :"_pInput.attachmentID)
	$$$LOGINFO("pInput.attachmentID : "_pInput.attachmentID)
	set t=..SendRequestSync("HttpOut_ERS",pInput,.resp)
	#DIM resp As RFT.ERS.Classes.AttachmentResponse
	set attach=##class(RFT.ERS.Tables.Attachments).%OpenId(pInput.attachmentID)
	
	$$$LOGINFO("resp.StatusCode : "_resp.StatusCode)
	
	IF (resp.StatusCode=200) && (resp.FilePath'="")
	{
		set filePath=resp.FilePath
		set fileType=resp.FileType

		$$$TRACE("filePath / fileType: "_filePath_"/"_fileType)
		
        // create/check PDFs directory "MM-YYYY"
		if ..DocumentStorageMode = "MOUNTHLY" {set FolderName = $E($ZDT($H,8),1,4)_"-"_$E($ZDT($H,8),5,6)}
		elseif ..DocumentStorageMode = "UBRN" {set FolderName = attach.ubrn}

		$$$TRACE("FolderName :"_FolderName)

		Set TargetDir = ..PDFsFolder_"\"_FolderName
		Set TargetDirExists=##class(%File).DirectoryExists(TargetDir)
		If ('TargetDirExists) {set tsc=##class(%File).CreateDirectory(TargetDir)} //Create the PDF directory if doesn't exist
		
		// Virus Scan
		/*
		set ScanCmd = ..VirusScanCommand_" "_filePath
		$$$TRACE("ScanCmd : "_ScanCmd)
		set runScan = $zf(-1, ScanCmd)
		*/

		// Check if the scanned file is always available and convert to PDF if needed
		IF ##class(%File).Exists(filePath) 
		{
			// PDF convert if needed + copy to final directory
			if fileType = "pdf"
			{
				set cmd= "copy "_filePath_" "_TargetDir
				SET runCmd=$ZF(-1,cmd)
			}
			elseif fileType'="pdf"
			{
				If $$IsFileTypeAllowed(fileType)
				{
					set cmd=""""_..LibreOffice_""""_" --headless --writer --convert-to pdf "_filePath_" --outdir "_TargetDir
					$$$TRACE("Will create PDF: "_cmd)
					SET runCmd=$ZF(-1,cmd)
				}
				Else
				{
        			; File type is not allowed, so just copy the original file to the target directory
        			Set copyCmd = "copy "_filePath_" "_TargetDir
					$$$LOGINFO("Will NOT create PDF. Will copy yhr orginal file to PDF direcotry: "_copyCmd)

        			Set copyResult = $ZF(-1, copyCmd)
    			}
			}
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
		$$$TRACE("attach.retryCount :"_attach.retryCount)
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

Method IsFileTypeAllowed(fileType As %String) As %Boolean
{
    Set fileType = $ZCONVERT(fileType, "L")  ; Convert to uppercase for case-insensitive comparison
    //Set allowedFiles = $LISTBUILD("DOCX", "DOC", "TXT", "RTF", "JPEG")
	Set allowedFiles = $LISTFROMSTRING(..AllowedFiles,",",0)
    Quit $LISTFIND(allowedFiles, fileType)
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
<Value name="6">
<Value>VirusScanCommand</Value>
</Value>
<Value name="7">
<Value>DocumentStorageMode</Value>
</Value>
</Data>
<DefaultData>DocumentProcessorDefaultData</DefaultData>
<Type>%Storage.Persistent</Type>
}

}
