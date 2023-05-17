Class RFT.ERS.Classes.HttpResponse Extends Ens.Response
{

Property StatusCode As %Integer;

Property ResponseBody As %String(MAXLEN = 100000);

Storage Default
{
<Data name="HttpResponseDefaultData">
<Subscript>"HttpResponse"</Subscript>
<Value name="1">
<Value>StatusCode</Value>
</Value>
<Value name="2">
<Value>ResponseBody</Value>
</Value>
</Data>
<DefaultData>HttpResponseDefaultData</DefaultData>
<Type>%Library.CacheStorage</Type>
}

}

