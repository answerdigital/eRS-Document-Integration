using AutoMapper;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Users;
using System.Diagnostics.CodeAnalysis;

namespace Playground.Service.Mappers;

[ExcludeFromCodeCoverage]
public sealed class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        this.CreateMap_Audit();
        this.CreateMap_RefReqDetail();
        this.CreateMap_ErsdocAttachment();
        this.CreateMap_Wfs();
        this.CreateMap_Patient();
        this.CreateMap_User();
    }

    private void CreateMap_Audit()
    {
        this.CreateMap<Auditlog, AuditlogDto>()
            .ForMember(dto => dto.NhsNo, map => map.MapFrom(a => a.ErsRefReqDetail != null ? a.ErsRefReqDetail.RefReqNhsno : null))
            .ForMember(dto => dto.PatName, map => map.MapFrom(a => a.ErsRefReqDetail != null && a.ErsRefReqDetail.Patient != null ? a.ErsRefReqDetail.Patient.PatFullName : null));
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
            .ForMember(dto => dto.WfsHistory, map => map.MapFrom(a => a.WfsHistoryList != null ? a.WfsHistoryList.MaxBy(h => h.RecUpdated) : null))
            .ForMember(dto => dto.Patient, map => map.MapFrom(a => a.RefReqDetail != null ? a.RefReqDetail.Patient : null));
        this.CreateMap<ErsdocAttachmentDto, ErsdocAttachment>()
            .ForMember(a => a.WfsHistoryList, map => map.Ignore());
    }

    private void CreateMap_Wfs()
    {
        this.CreateMap<WfsMaster, WfsMasterDto>()
            .ForMember(dto => dto.ErrorStatus, map => map.MapFrom(wfs => wfs.WfsmCode != null ? wfs.WfsmCode.Contains("FAIL") : false));
        this.CreateMap<WfsMasterDto, WfsMaster>();

        this.CreateMap<WfsHistory, WfsHistoryDto>()
            .ForMember(dto => dto.AttachTitle, map => map.MapFrom(wfs => wfs.ErsdocAttachment != null ? wfs.ErsdocAttachment.AttachTitle : null));
        this.CreateMap<WfsHistoryDto, WfsHistory>();
    }

    private void CreateMap_Patient()
    {
        this.CreateMap<Patient, PatientDto>();
        this.CreateMap<PatientDto, Patient>();
    }

    private void CreateMap_User()
    {
        this.CreateMap<User, UserDto>()
            .ForMember(dto => dto.UserFullName, map => map.MapFrom(u => u.UserForename != null && u.UserSurname != null ? $"{u.UserForename} {u.UserSurname}" : null));
        this.CreateMap<UserDto, User>();
        this.CreateMap<UserCreate, User>();
    }
}