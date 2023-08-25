namespace eRS.Models.HttpClient.Responses;

public class RefReqProperties
{
    public List<ContainedElements> Contained { get; set; }

    public RefReqSpeciality Speciality { get; set; }
}

public class ContainedElements
{

}

public class RefReqSpeciality
{
    public RefReqSpecialityCoding Coding { get; set; }
}

public class RefReqSpecialityCoding
{
    public string System { get; set; }
    public string Code { get; set; }
    public string Display { get; set; }
}
