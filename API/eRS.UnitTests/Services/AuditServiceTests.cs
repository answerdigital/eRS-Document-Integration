using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Services.Services;
using eRS.UnitTests.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace eRS.UnitTests.Services
{
    public class AuditServiceTests
    {
        private eRSContext dbContext = null!;
        private IMapper mapper = null!;
        private Mock<ILogger<AuditService>> mockLogger = new Mock<ILogger<AuditService>>();

        public AuditServiceTests()
        {
            dbContext = ContextHelper.CreateDbContext();
            mapper = MapperHelper.CreateMapper();
        }

        [Fact]
        public async void GetAllFilteredPaged_InvalidRequest_ThrowsException()
        {
            var auditRequest = new AuditRequest
            {
                Filters = new()
            };

            var sut = this.BuildSut();

            var action = async () => await sut.GetAllFilteredPaged(auditRequest);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async void GetAllFilteredPaged_ReturnsPagedResult()
        {
            var auditLog = new Auditlog
            {
                ErstrnsUid = "70000"
            };

            await this.dbContext.Auditlogs.AddAsync(auditLog);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                PageNumber = 1,
                Filters = new()
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFilteredPaged(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<PagedResult<AuditlogDto>>(result);
        }

        [Fact]
        public async void GetAllFiltered_ReturnsList()
        {
            var auditLog = new Auditlog
            {
                ErstrnsUid = "70000"
            };

            await this.dbContext.Auditlogs.AddAsync(auditLog);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
        }

        [Fact]
        public async void GetAllFiltered_FilterDate()
        {
            var now = DateTime.UtcNow;

            var auditLog1 = new Auditlog
            {
                RecInserted = now
            };

            var auditLog2 = new Auditlog
            {
                RecInserted = now.AddDays(7)
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    RecInsertedFrom = now,
                    RecInsertedTo = now.AddDays(4)
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterEventCode()
        {
            var auditLog1 = new Auditlog
            {
                ToEventCode = "D-QCEPR-SUCC"
            };

            var auditLog2 = new Auditlog
            {
                ToEventCode = "D-QCEPR-FAIL"
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    EventCode = "D-QCEPR-SUCC"
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterUser()
        {
            var auditLog1 = new Auditlog
            {
                RecInsertedBy = "User"
            };

            var auditLog2 = new Auditlog
            {
                RecInsertedBy = "Admin"
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    RecInsertedBy = "User"
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterRefReqUid()
        {
            var auditLog1 = new Auditlog
            {
                ErstrnsUid = "70000"
            };

            var auditLog2 = new Auditlog
            {
                ErstrnsUid = "70002"
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    RefReqUid = "70000"
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterRefDocUid()
        {
            var auditLog1 = new Auditlog
            {
                DoctrnsUid = "8000"
            };

            var auditLog2 = new Auditlog
            {
                DoctrnsUid = "8001"
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    RefDocUid = "8000"
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterPatientNhsNo()
        {
            var patientNhs1 = "0000000000";
            var patientNhs2 = "0000000001";
            var ubrn1 = "70000";
            var ubrn2 = "70002";

            var refReq1 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn1,
                RefReqUniqueId = "70000",
                RefReqNhsno = patientNhs1
            };

            var auditLog1 = new Auditlog
            {
                ErstrnsUid = "70000",
                ErsRefReqDetail = refReq1,
            };

            var refReq2 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn2,
                RefReqUniqueId = "70002",
                RefReqNhsno = patientNhs2
            };

            var auditLog2 = new Auditlog
            {
                ErstrnsUid = "70002",
                ErsRefReqDetail = refReq2
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    NhsNo = patientNhs1
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void GetAllFiltered_FilterPatientName()
        {
            var patientName1 = "Test Patient";
            var patientName2 = "Nobody";
            var ubrn1 = "0000000000";
            var ubrn2 = "0000000001";

            var patient1 = new Patient
            {
                PatUbrn = ubrn1,
                PatFullName = patientName1
            };

            var patient2 = new Patient
            {
                PatUbrn = ubrn2,
                PatFullName = patientName2
            };

            var refReq1 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn1,
                RefReqUniqueId = "70000",
                Patient = patient1
            };

            var auditLog1 = new Auditlog
            {
                ErstrnsUid = "70000",
                ErsRefReqDetail = refReq1
            };

            var refReq2 = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn2,
                RefReqUniqueId = "70002",
                Patient = patient2
            };

            var auditLog2 = new Auditlog
            {
                ErstrnsUid = "70002",
                ErsRefReqDetail = refReq2
            };

            await this.dbContext.Auditlogs.AddRangeAsync(auditLog1, auditLog2);
            await this.dbContext.SaveChangesAsync();

            var auditRequest = new AuditRequest
            {
                Filters = new()
                {
                    NhsNo = patientName1
                }
            };

            var sut = this.BuildSut();

            var result = await sut.GetAllFiltered(auditRequest);
            Assert.NotNull(result);
            Assert.IsType<List<AuditlogDto>>(result);
            Assert.Single(result);
            Assert.Collection(result, i => i.Equals(auditLog1));
        }

        [Fact]
        public async void AddAudit_Valid()
        {
            var ubrn = "0000000000";
            var uid = "70000";

            var patient = new Patient
            {
                PatUbrn = ubrn
            };

            var refReq = new ErsRefReqDetail
            {
                RefReqUbrn = ubrn,
                RefReqUniqueId = uid,
                Patient = patient
            };

            var auditLog = new Auditlog
            {
                ErstrnsUid = uid
            };

            var sut = this.BuildSut();

            var result = await sut.AddAudit(auditLog);
            Assert.IsType<bool>(result);
        }

        private AuditService BuildSut() => new(this.dbContext, this.mapper, this.mockLogger.Object);
    }
}
