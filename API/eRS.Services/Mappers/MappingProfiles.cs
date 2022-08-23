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
        this.CreateMap<Auditlog, AuditlogDto>();
        this.CreateMap<AuditlogDto, Auditlog>();
    }
}