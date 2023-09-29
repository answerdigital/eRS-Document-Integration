Class RFT.Utility.CodeableConcept Extends %RegisteredObject
{

ClassMethod RetriveCode(input As %Library.DynamicObject, Output code As %String)
{
 

		set t=$$$OK
		set coding= input.valueCodeableConcept.coding
		set iter=coding.%GetIterator()
		while iter.%GetNext(.key,.value)
		{
				set code=value.code			    	
		}
				    	
		quit code
}

}

