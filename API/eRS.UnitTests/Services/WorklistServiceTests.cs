using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Models.Models.Wfs;
using eRS.Services.Interfaces;
using eRS.Services.Services;
using eRS.UnitTests.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace eRS.UnitTests.Services
{
    public class WorklistServiceTests
    {
        private eRSContext dbContext = null!;
        private IMapper mapper = null!;
        private Mock<ILogger<WorklistService>> mockLogger = new Mock<ILogger<WorklistService>>();
        private Mock<IAuditService> mockAuditService = new Mock<IAuditService>();

        public WorklistServiceTests()
        {
            dbContext = ContextHelper.CreateDbContext();
            mapper = MapperHelper.CreateMapper();
        }

        [Fact]
        public async void GetWorklistFiltered_InvalidRequest_ThrowsException()
        {
            var sut = this.BuildSut();

            var action = async () => await sut.GetWorklistFiltered(null);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async void GetWorklistFiltered_ReturnsPagedResult()
        {
            var refReq = new ErsRefReqDetail
            {
                RefReqUbrn = "70000"
            };

            await this.dbContext.ErsRefReqDetails.AddAsync(refReq);
            await this.dbContext.SaveChangesAsync();

            var worklistRequest = new WorklistRequest
            {
                PageNumber = 1,
                Filters = new()
            };

            var sut = this.BuildSut();

            var result = await sut.GetWorklistFiltered(worklistRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ErsRefReqDetailDto>>(result);
        }

        [Fact]
        public async void GetWorklistFiltered_RecStatusFiltered()
        {
            var refReq = new ErsRefReqDetail
            {
                RefReqUbrn = "70000",
                RecStatus = "D"
            };

            await this.dbContext.ErsRefReqDetails.AddAsync(refReq);
            await this.dbContext.SaveChangesAsync();

            var worklistRequest = new WorklistRequest
            {
                PageNumber = 1,
                Filters = new()
            };

            var sut = this.BuildSut();

            var result = await sut.GetWorklistFiltered(worklistRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ErsRefReqDetailDto>>(result);
            Assert.Empty(result.Results);
        }

        [Fact]
        public async void GetWorklistFiltered_FilterDate()
        {
            var now = DateTime.UtcNow;
            var ubrn1 = "70000";
            var ubrn2 = "70002";

            var patient1 = new Patient
            {
                PatUbrn = ubrn1
            };

            var patient2 = new Patient
            {
                PatUbrn = ubrn2
            };

            var refReq1 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn1,
                RecUpdated = now
            };

            var refReq2 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn2,
                RecUpdated = now.AddDays(7)
            };

            await this.dbContext.Patients.AddRangeAsync(patient1, patient2);
            await this.dbContext.ErsRefReqDetails.AddRangeAsync(refReq1, refReq2);
            await this.dbContext.SaveChangesAsync();

            var worklistRequest = new WorklistRequest
            {
                Filters = new()
                {
                    RecInsertedFrom = now,
                    RecInsertedTo = now.AddDays(4)
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetWorklistFiltered(worklistRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ErsRefReqDetailDto>>(result);
            Assert.Single(result.Results);
            Assert.Collection(result.Results, i => i.Equals(mapper.Map<ErsRefReqDetailDto>(refReq1)));
        }

        [Fact]
        public async void GetWorklistFilteredQuery_FilterRefReqSpecialty()
        {
            var spec1 = "CARDIOLOGY";
            var spec2 = "NEUROSURGERY";
            var ubrn1 = "70000";
            var ubrn2 = "70002";

            var patient1 = new Patient
            {
                PatUbrn = ubrn1
            };

            var patient2 = new Patient
            {
                PatUbrn = ubrn2
            };

            var refReq1 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn1,
                RefReqSpecialty = spec1
            };

            var refReq2 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn2,
                RefReqSpecialty = spec2
            };

            await this.dbContext.Patients.AddRangeAsync(patient1, patient2);
            await this.dbContext.ErsRefReqDetails.AddRangeAsync(refReq1, refReq2);
            await this.dbContext.SaveChangesAsync();

            var worklistRequest = new WorklistRequest
            {
                Filters = new()
                {
                    RefReqSpecialty = spec1
                }
            }; 

            var sut = this.BuildSut();

            var result = await sut.GetWorklistFiltered(worklistRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ErsRefReqDetailDto>>(result);
            Assert.Single(result.Results);
            Assert.Collection(result.Results, i => i.Equals(refReq1));
        }

        [Fact]
        public async void GetWorklistFiltered_FilterInvestigationMode()
        {
            var ubrn1 = "70000";
            var ubrn2 = "70002";

            var patient1 = new Patient
            {
                PatUbrn = ubrn1
            };

            var patient2 = new Patient
            {
                PatUbrn = ubrn2
            };

            var refReq1 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn1
            };

            var refReq2 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn2
            };

            var successCode = new WfsMaster
            {
                WfsmCode = "D-QCEPR-SUCC"
            };

            var failCode = new WfsMaster
            {
                WfsmCode = "D-QCEPR-FAIL"
            };

            var status1 = new WfsHistory
            {
                StatusCode = "D-QCEPR-SUCC",
                ErsRefReqDetail = refReq1
            };

            var status2 = new WfsHistory
            {
                StatusCode = "D-QCEPR-FAIL",
                ErsRefReqDetail = refReq2
            };

            await this.dbContext.Patients.AddRangeAsync(patient1, patient2);
            await this.dbContext.WfsMasters.AddRangeAsync(successCode, failCode);
            await this.dbContext.WfsHistories.AddRangeAsync(status1, status2);
            await this.dbContext.ErsRefReqDetails.AddRangeAsync(refReq1, refReq2);
            await this.dbContext.SaveChangesAsync();

            var worklistRequest = new WorklistRequest
            {
                Filters = new()
                {
                    InvestigationMode = true
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetWorklistFiltered(worklistRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<ErsRefReqDetailDto>>(result);
            Assert.Single(result.Results);
            Assert.Collection(result.Results, i => i.Equals(refReq1));
        }

        [Fact]
        public async void GetWorkflowStates_ReturnsWfsMasterResponse()
        {
            var wfs1 = new WfsMaster
            {
                WfsmCode = "R-QCEPR-SUCC"
            };

            var wfs2 = new WfsMaster
            {
                WfsmCode = "D-QCEPR-SUCC"
            };

            await this.dbContext.WfsMasters.AddRangeAsync(wfs1, wfs2);
            await this.dbContext.SaveChangesAsync();

            var sut = this.BuildSut();

            var result = await sut.GetWorkflowStates();
            Assert.NotNull(result);
            Assert.IsType<WfsMasterResponse>(result);
            Assert.IsType<List<WfsMasterDto>>(result.RefReqStates);
            Assert.IsType<List<WfsMasterDto>>(result.RefDocStates);
            Assert.Single(result.RefReqStates);
            Assert.Collection(result.RefReqStates, i => i.Equals(wfs1));
            Assert.Single(result.RefDocStates);
            Assert.Collection(result.RefReqStates, i => i.Equals(wfs2));
        }

        [Fact]
        public async void GetWorkflowHistory_ReferralStatus()
        {
            var refUid = "70000";

            var successCode = new WfsMaster
            {
                WfsmCode = "D-QCEPR-SUCC"
            };

            var wfs = new WfsHistory
            {
                ErstrnsUid = refUid,
                StatusCode = "D-QCEPR-SUCC"
            };

            await this.dbContext.WfsMasters.AddAsync(successCode);
            await this.dbContext.WfsHistories.AddAsync(wfs);
            await this.dbContext.SaveChangesAsync();

            var sut = this.BuildSut();

            var result = await sut.GetWorkflowHistory(refUid, null);
            Assert.NotNull(result);
            Assert.IsType<List<WfsHistoryDto>>(result);
        }

        [Fact]
        public async void GetWorkflowHistory_AttachmentStatus()
        {
            var refUid = "70000";
            var docUid = "8000";

            var successCode = new WfsMaster
            {
                WfsmCode = "D-QCEPR-SUCC"
            };

            var wfs = new WfsHistory
            {
                ErstrnsUid = refUid,
                DoctrnsUid = docUid,
                StatusCode = "D-QCEPR-SUCC"
            };

            await this.dbContext.WfsMasters.AddAsync(successCode);
            await this.dbContext.WfsHistories.AddAsync(wfs);
            await this.dbContext.SaveChangesAsync();

            var sut = this.BuildSut();

            var result = await sut.GetWorkflowHistory(refUid, docUid);
            Assert.NotNull(result);
            Assert.IsType<List<WfsHistoryDto>>(result);
        }

        [Fact]
        public async void AddToWorkflowHistory_NoRefUid_ThrowsException()
        {
            var wfsHistory = new WfsHistoryDto { };

            var sut = this.BuildSut();

            var action = async () => await sut.AddToWorkflowHistory(wfsHistory);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async void AddToWorkflowHistory_ValidReferral()
        {
            var ubrn = "0000000000";
            var refUid = "70000";

            var patient = new Patient
            {
                PatUbrn = ubrn
            };

            var refReq = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn,
                RefReqUniqueId = refUid,
                Patient = patient
            };

            await this.dbContext.Patients.AddAsync(patient);
            await this.dbContext.ErsRefReqDetails.AddAsync(refReq);
            await this.dbContext.SaveChangesAsync();

            var wfsHistory = new WfsHistoryDto
            {
                ErstrnsUid = refUid
            };

            var sut = this.BuildSut();

            var result = await sut.AddToWorkflowHistory(wfsHistory);
            Assert.IsType<List<WfsHistoryDto?>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(wfsHistory));
        }

        [Fact]
        public async void AddToWorkflowHistory_ValidReferral_NotFound()
        {
            var refUid = "70000";

            var wfsHistory = new WfsHistoryDto
            {
                ErstrnsUid = refUid
            };

            var sut = this.BuildSut();

            var result = await sut.AddToWorkflowHistory(wfsHistory);
            Assert.Null(result);
        }

        [Fact]
        public async void AddToWorkflowHistory_ValidAttachment()
        {
            var ubrn = "0000000000";
            var refUid = "70000";
            var docUid = "8000";

            var patient = new Patient
            {
                PatUbrn = ubrn
            };

            var refReq = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn,
                RefReqUniqueId = refUid,
                Patient = patient
            };

            var refDoc = new ErsdocAttachment
            {
                RefDocUniqueId = refUid,
                AttachId = docUid
            };

            await this.dbContext.Patients.AddAsync(patient);
            await this.dbContext.ErsRefReqDetails.AddAsync(refReq);
            await this.dbContext.ErsdocAttachments.AddAsync(refDoc);
            await this.dbContext.SaveChangesAsync();

            var wfsHistory = new WfsHistoryDto
            {
                ErstrnsUid = refUid,
                DoctrnsUid = docUid
            };

            var sut = this.BuildSut();

            var result = await sut.AddToWorkflowHistory(wfsHistory);
            Assert.IsType<List<WfsHistoryDto?>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(wfsHistory));
        }

        [Fact]
        public async void AddToWorkflowHistory_ValidAttachment_NotFound()
        {
            var refUid = "70000";
            var docUid = "8000";

            var wfsHistory = new WfsHistoryDto
            {
                ErstrnsUid = refUid,
                DoctrnsUid = docUid
            };

            var sut = this.BuildSut();

            var result = await sut.AddToWorkflowHistory(wfsHistory);
            Assert.Null(result);
        }

        [Fact]
        public async void AddToWorkflowHistory_HistoryExists()
        {
            var refUid = "70000";
            var docUid = "8000";

            var existingHistory = new WfsHistory
            {
                ErstrnsUid = refUid,
                DoctrnsUid = docUid,
                StatusCode = "D-QCEPR-FAIL"
            };

            await this.dbContext.WfsHistories.AddAsync(existingHistory);
            await this.dbContext.SaveChangesAsync();

            var newHistory = new WfsHistoryDto
            {
                ErstrnsUid = refUid,
                DoctrnsUid = docUid,
                StatusCode = "D-QCEPR-SUCC"
            };

            var sut = this.BuildSut();

            var result = await sut.AddToWorkflowHistory(newHistory);
            Assert.IsType<List<WfsHistoryDto?>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(newHistory));
        }

        private WorklistService BuildSut() => new(this.dbContext, this.mapper, this.mockLogger.Object, this.mockAuditService.Object);
    }
}
