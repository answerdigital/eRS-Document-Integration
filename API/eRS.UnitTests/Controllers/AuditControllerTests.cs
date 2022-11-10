using eRS.API.Controllers;
using eRS.Models.Dtos;
using eRS.Models.Models.Audits;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace eRS.UnitTests.Controllers
{
    public class AuditControllerTests
    {
        private Mock<ILogger<AuditLogController>> mockLogger = new Mock<ILogger<AuditLogController>>();
        private Mock<IAuditService> mockAuditService = new Mock<IAuditService>();

        [Fact]
        public async void RequestAudits_NoAudits_ReturnsNotFound()
        {
            var auditRequest = new AuditRequest
            {
                PageNumber = 1,
                Filters = {}
            };

            var sut = this.BuildSut();

            var result = await sut.RequestAudits(auditRequest) as NotFoundResult;
            Assert.NotNull(result);

            this.mockAuditService.Verify(x => x.GetAllFilteredPaged(It.IsAny<AuditRequest>()), Times.Once);
        }

        [Fact]
        public async void RequestAudits_RequestInvalid_ReturnsBadRequest()
        {
            var auditRequest = new AuditRequest
            {
                Filters = { }
            };

            this.mockAuditService.Setup(x => x.GetAllFilteredPaged(It.IsAny<AuditRequest>())).Throws(new Exception());

            var sut = this.BuildSut();

            var result = await sut.RequestAudits(auditRequest) as BadRequestObjectResult;
            Assert.NotNull(result);

            mockAuditService.Verify(x => x.GetAllFilteredPaged(It.IsAny<AuditRequest>()), Times.Once);
        }

        [Fact]
        public async void RequestAudits_Valid_ReturnsPagedAudits()
        {
            var auditRequest = new AuditRequest
            {
                PageNumber = 1,
                Filters = { }
            };

            var mockAuditResult = new PagedResult<AuditlogDto>
            {
                PageCount = 1,
                PageSize = 10,
                CurrentPage = 1,
                Results = new List<AuditlogDto>
                {
                    new AuditlogDto()
                }
            };

            this.mockAuditService.Setup(x => x.GetAllFilteredPaged(It.IsAny<AuditRequest>())).ReturnsAsync(mockAuditResult);

            var sut = this.BuildSut();

            var result = await sut.RequestAudits(auditRequest) as OkObjectResult;
            Assert.NotNull(result);

            this.mockAuditService.Verify(x => x.GetAllFilteredPaged(It.IsAny<AuditRequest>()), Times.Once);

            var resultValue = result.Value as PagedResult<AuditlogDto>;
            Assert.NotNull(resultValue);
        }

        private AuditLogController BuildSut() => new(this.mockAuditService.Object, this.mockLogger.Object);
    }
}