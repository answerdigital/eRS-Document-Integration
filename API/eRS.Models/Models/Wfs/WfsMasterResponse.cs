using eRS.Models.Dtos;

namespace eRS.Models.Models.Wfs;

public class WfsMasterResponse
{
    public List<WfsMasterDto>? RefDocStates { get; set; }
    public List<WfsMasterDto>? RefReqStates { get; set; }
}