using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Models.Models.Wfs;

namespace eRS.Services.Interfaces;

public interface IWorklistService
{
    public Task<PagedResult<ErsRefReqDetailDto>> GetWorklistFiltered(WorklistRequest request);
    public Task<List<ErsdocAttachmentDto>> GetAttachments(string refUid);
    public Task<WfsMasterResponse> GetWorkflowStates();
    public Task<List<WfsHistoryDto>> GetWorkflowHistory(string? refUid, string? docUid);
    public Task<List<WfsHistoryDto>?> AddToWorkflowHistory(WfsHistoryDto newHistory);
    public Task<List<WfsHistoryDto>?> UpdateWorkflowHistory(WfsHistoryDto newHistory, WfsHistory? oldHistory);

}