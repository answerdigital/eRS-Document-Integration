Class test.ExportClassesUtil Extends %RegisteredObject
{

// Method to export classes from the given package name

ClassMethod UDLExport(packageName As %String) As %Status
{
        set tsc =$$$OK
        // Construct SQL query to retrieve class names from the given package
        set Qry= "SELECT Name FROM %Library.RoutineMgr_StudioOpenDialog('"_packageName_"',1,1,0,1,0,0)"
        Set ST=##class(%SQL.Statement).%New()
        // Create a new %SQL.Statement object and prepare the query
        set tsc = ST.%Prepare(Qry)
        // Execute the prepared query to obtain a result set
        set rset = ST.%Execute()

        // Loop through each result in the result set
        while rset.%Next()
        {
            // Get the class name from the result set
            set ClassName = rset.%Get("Name")
            // Export the class in the form of a UDL file using the $System.OBJ.ExportUDL method
            set tsc =  $System.OBJ.ExportUDL(ClassName,ClassName,,,)
            write ClassName,!
        }
    quit tsc
}

// Method to export production classes as xml for a specific IRIS/Cache version 

ClassMethod ExportProd(version As %String, packageName As %String, ProductionExportFile As %String) As %Status
{
    set tsc =$$$OK
    // Set the export version qualifier
    set tsc = $SYSTEM.OBJ.SetQualifiers("/exportversion="_version)
    // Export the package classes using the $System.OBJ.Export method and store the exported data in the specified production export file
    do $System.OBJ.Export(.packageName,.ProductionExportFile)
       
    quit tsc
}

/*

Run in terminal: 

Class method 1: to export classes and lookup tables
    do ##class("test.ExportClassesUtil").UDLExport("RFT.*.cls,*.LUT") 

Class method 2: to export an entire production 
    // Use to export PROD to be deployed on Cache systems
    do ##class("test.ExportClassesUtil").ExportProd("cache2017.2.1","RFT.*.cls,*.LUT","ProductionToDeploy.xml")

    // Use to export PROD to be deployed on IRIS systems   
    do ##class("test.ExportClassesUtil").ExportProd("iris2022.3","RFT.*.cls,*.LUT,*.PTD","ProductionToDeploy.xml")

*/
}
