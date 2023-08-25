Class test.ImportClassesUtil Extends %RegisteredObject
{

// To import an entire production and automatically create the components 
ClassMethod ParentClass(pDirectory As %String, pCSVFile As %String)
{
    set tsc = $$$OK
    set tsc = ..Import(pDirectory)
    set tsc = ..ImportCSV(pCSVFile)


    quit tsc
}

ClassMethod Import(pDirectory As %String)
{
	set tsc =$$$OK
	set ProdToImport =  $SYSTEM.OBJ.ImportDir(pDirectory,,"c")

	quit tsc
}

ClassMethod ImportCSV(pCSVFile As %String)
{
    set tsc = $$$OK
    write ("ImportingHere")
    // read from csv component and class names 
    set adapter = ##class(%File).%New(pCSVFile)
    set status = adapter.Open("R")

    if $$$ISERR(status)
    {
     do $System.Status.DisplayError(status)
    }

    set counter =1
    // looping through the lines in the csv 
    while 'adapter.AtEnd
    {
        set line=adapter.ReadLine()
        // create a dynamic object 
        set rec= ##class(%DynamicObject).%New()
        //// Set the values of the dynamic object's properties based on the values in the current line of the CSV file
        Set rec.Production = $piece(line,",",1)
        Set rec.Name = $piece(line,",",2)
        Set rec.ClassName = $piece(line,",",3)
        Set rec.PoolSize = $piece(line,",",4)
        Set rec.Comment = $piece(line,",",5)

        ;Set savestatus=rec.%Save()
        /*
        if $$$ISERR(savestatus)
        {
         do $System.Status.DisplayError(status)
        }
        */
        if rec.Production '=""
        {
            // calling the classmethod below 'addbusinessitem'
            set tsc= ..AddBusinessItem(rec.Production, rec.Name, rec.ClassName, rec.PoolSize, rec.Comment)
        }
        else {quit}
    }
    
      //if error found during processing,show it
      if $$$ISERR(status)
      {
       do $System.Status.DisplayError(status)
      }
      quit tsc
}

ClassMethod AddBusinessItem(pProduction As %String, pItemName As %String, pClassName As %String, pPoolSize As %Integer = 1, pComment As %String = "") As %Status
{
    Set ret = $$$OK
    write "started",!
    Try {
        // Check the mandatories values
        If ((pItemName '="") && (pClassName '=""))
        {
            If '##class(Ens.Config.Item).NameExists(pProduction,pItemName,.idItem)
            {
                Set production = ##class(Ens.Config.Production).%OpenId(pProduction)
                Write !,"Installing "_pItemName,!
                Set objItem = ##class(Ens.Config.Item).%New()
                Set objItem.Name = pItemName
                write "pClassName : "_pClassName,!
                Set objItem.ClassName = pClassName
                Set objItem.Enabled = 1
                Set objItem.PoolSize = pPoolSize
                Set objItem.Comment = pComment
                


                Do production.Items.Insert(objItem)

                Set st = production.%Save()
                Kill objItem

                Do ##class(Ens.Director).RestartProduction(0,1)
                Write !,"Item "_pItemName_" installed."
            }
            Else
            {
                Write !,"Item "_pItemName_" already exists."
            }

        }
        Else
        {
            If (pItemName = "") set attrib = "pItemName"
            If (pClassName ="") set attrib = "pClassName"

            $$$ThrowOnError($System.Status.Error(406,attrib))
        }
    }
    Catch ex {
        Set ret = ex.AsStatus()
        Write !,"Error #",$System.Status.GetErrorCodes(ret),!
        Write !,$System.Status.GetOneStatusText(ret,1),!
    }

    Quit ret
}

/*

Usage Example, run in the terminal: 

do ##class("test.ImportClassesUtil").ParentClass("C:\Users\MohammedAlhato\OneDrive - Answer Digital\Desktop\Integration-v0.0.2\Script[post_class_import]","C:\Users\MohammedAlhato\OneDrive - Answer Digital\Desktop\Integration-v0.0.2\Script[post_class_import]\items.csv")

*/
}
