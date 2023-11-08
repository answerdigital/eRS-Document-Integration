Class RFT.ERS.Production Extends Ens.Production
{

XData ProductionDefinition
{
<Production Name="RFT.ERS.Production" LogGeneralTraceEvents="false">
  <Description></Description>
  <ActorPoolSize>2</ActorPoolSize>
  <Item Name="Invoke_DocumentDownloader" Category="ERS" ClassName="RFT.ERS.Service.InvokeDocumentDownloader" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="false" Schedule="">
    <Setting Target="Adapter" Name="CallInterval">1800</Setting>
  </Item>
  <Item Name="Invoke_RefToDB" Category="ERS" ClassName="RFT.ERS.Service.InvokeRefToDB" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
    <Setting Target="Adapter" Name="CallInterval">1800</Setting>
  </Item>
  <Item Name="Sch_InvokeReferralProcessor" Category="ERS" ClassName="RFT.ERS.Service.InvokeReferralProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
    <Setting Target="Adapter" Name="CallInterval">1800</Setting>
  </Item>
  <Item Name="Sch_InvokeWorklistProcessor" Category="ERS" ClassName="RFT.ERS.Service.InvokeWorklistProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
    <Setting Target="Adapter" Name="Port">9985</Setting>
    <Setting Target="Adapter" Name="SSLConfig"></Setting>
    <Setting Target="Host" Name="ArchiveIO">1</Setting>
  </Item>
  <Item Name="ClinicalInformationDownloadProcessor" Category="ERS" ClassName="RFT.ERS.Process.ClinicalInformationProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="false" Schedule="">
  </Item>
  <Item Name="DocumentDownloadProcess" Category="ERS" ClassName="RFT.ERS.Process.DocumentProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
    <Setting Target="Host" Name="OutputDir">C:\Users\HaitemElAaouani\Documents\eRS\OuPut\raw</Setting>
    <Setting Target="Host" Name="PDFsFolder">C:\Users\HaitemElAaouani\Documents\eRS\OuPut\pdf</Setting>
    <Setting Target="Host" Name="DocumentStorageMode">MOUNTHLY</Setting>
    <Setting Target="Host" Name="AllowedFiles">docx,doc,txt,rtf,tif,jpeg,xml</Setting>
  </Item>
  <Item Name="ReferralProcessor" Category="ERS" ClassName="RFT.ERS.Process.ReferralProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
  </Item>
  <Item Name="ReferralToDB" Category="ERS" ClassName="RFT.ERS.Process.RefToDB" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="false" Schedule="">
  </Item>
  <Item Name="WorklistProcessor" Category="ERS" ClassName="RFT.ERS.Process.WorklistProcessor" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
  </Item>
  <Item Name="HttpOut_ERS" Category="ERS" ClassName="RFT.ERS.Operation.ERS" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="true" Schedule="">
    <Setting Target="Adapter" Name="HTTPServer">https://int.api.service.nhs.uk</Setting>
    <Setting Target="Adapter" Name="SSLConfig">Simple</Setting>
    <Setting Target="Host" Name="FhirServer">https://server.fire.ly/r3</Setting>
    <Setting Target="Host" Name="WorkListSizeLimitKB">10000</Setting>
    <Setting Target="Host" Name="WorkListSizeLimitMB">0.1</Setting>
  </Item>
  <Item Name="ERS_DB_UPDATE" Category="ERS" ClassName="RFT.ERS.Operation.ExecuteQuery" PoolSize="1" Enabled="true" Foreground="false" Comment="" LogTraceEvents="false" Schedule="">
    <Setting Target="Adapter" Name="DSN">ERS</Setting>
    <Setting Target="Adapter" Name="StayConnected">30</Setting>
    <Setting Target="Adapter" Name="Credentials">ERS</Setting>
  </Item>
  <Item Name="Ens.Alert" Category="" ClassName="EnsLib.EMail.AlertOperation" PoolSize="1" Enabled="false" Foreground="false" Comment="" LogTraceEvents="false" Schedule="">
  </Item>
</Production>
}

}

