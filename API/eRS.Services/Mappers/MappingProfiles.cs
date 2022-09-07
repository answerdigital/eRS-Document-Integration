using AutoMapper;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Playground.Service.Mappers;

[ExcludeFromCodeCoverage]
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        this.CreateMap_Audit();
        this.CreateMap_RefReqDetail();
        this.CreateMap_ErsdocAttachment();
        this.CreateMap_Wfs();
    }

    private void CreateMap_Audit()
    {
        this.CreateMap<Auditlog, AuditlogDto>();
        this.CreateMap<AuditlogDto, Auditlog>();
    }

    private void CreateMap_RefReqDetail()
    {
        this.CreateMap<ErsRefReqDetail, ErsRefReqDetailDto>()
            .ForMember(dto => dto.WfsHistory,
                map => map.MapFrom(r => r.WfsHistoryList != null ? r.WfsHistoryList.MaxBy(h => h.RecUpdated) : null));
        this.CreateMap<ErsRefReqDetailDto, ErsRefReqDetail>()
            .ForMember(r => r.WfsHistoryList, map => map.Ignore());
    }

    private void CreateMap_ErsdocAttachment()
    {
        this.CreateMap<ErsdocAttachment, ErsdocAttachmentDto>()
            .ForMember(dto => dto.WfsHistory,
                map => map.MapFrom(a => a.WfsHistoryList != null ? a.WfsHistoryList.MaxBy(h => h.RecUpdated) : null));
        this.CreateMap<ErsdocAttachmentDto, ErsdocAttachment>()
            .ForMember(a => a.WfsHistoryList, map => map.Ignore());
    }

    private void CreateMap_Wfs()
    {
        this.CreateMap<WfsMaster, WfsMasterDto>();
        this.CreateMap<WfsMasterDto, WfsMaster>();

        this.CreateMap<WfsHistory, WfsHistoryDto>();
        this.CreateMap<WfsHistoryDto, WfsHistory>();
    }

}