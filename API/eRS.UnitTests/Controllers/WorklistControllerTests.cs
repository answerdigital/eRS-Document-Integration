using eRS.API.Controllers;
using eRS.Models.Dtos;
using eRS.Models.Models.ersRefRequests;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace eRS.UnitTests.Controllers
{
    public class WorklistControllerTests
    {
        private Mock<ILogger<WorklistController>> mockLogger = new Mock<ILogger<WorklistController>>();
        private Mock<IWorklistService> mockWorklistService = new Mock<IWorklistService>();
        private Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();

        [Fact]
        public async void RequestWorklist_NoReferrals_ReturnsNotFound()
        {
            var worklistRequest = new WorklistRequest
            {
                PageNumber = 1,
                Filters = { }
            };

            var sut = this.BuildSut();

            var result = await sut.RequestWorklist(worklistRequest) as NotFoundResult;
            Assert.NotNull(result);

            this.mockWorklistService.Verify(x => x.GetWorklistFiltered(It.IsAny<WorklistRequest>()), Times.Once);
        }

        [Fact]
        public async void RequestWorklist_RequestInvalid_ReturnsBadRequest()
        {
            var worklistRequest = new WorklistRequest
            {
                Filters = { }
            };

            this.mockWorklistService.Setup(x => x.GetWorklistFiltered(It.IsAny<WorklistRequest>())).Throws(new Exception());

            var sut = this.BuildSut();

            var result = await sut.RequestWorklist(worklistRequest) as BadRequestObjectResult;
            Assert.NotNull(result);

            mockWorklistService.Verify(x => x.GetWorklistFiltered(It.IsAny<WorklistRequest>()), Times.Once);
        }

        [Fact]
        public async void RequestWorklist_Valid_ReturnsPagedWorklist()
        {
            var worklistRequest = new WorklistRequest
            {
                PageNumber = 1,
                Filters = { }
            };

            var mockReferralResult = new PagedResult<ErsRefReqDetailDto>
            {
                PageCount = 1,
                PageSize = 10,
                CurrentPage = 1,
                Results = new List<ErsRefReqDetailDto>
                {
                    new ErsRefReqDetailDto()
                }
            };

            this.mockWorklistService.Setup(x => x.GetWorklistFiltered(It.IsAny<WorklistRequest>())).ReturnsAsync(mockReferralResult);

            var sut = this.BuildSut();

            var result = await sut.RequestWorklist(worklistRequest) as OkObjectResult;
            Assert.NotNull(result);

            this.mockWorklistService.Verify(x => x.GetWorklistFiltered(It.IsAny<WorklistRequest>()), Times.Once);

            var resultValue = result.Value as PagedResult<ErsRefReqDetailDto>;
            Assert.NotNull(resultValue);
        }

        [Fact]
        public async void GetAttachments_NoAttachments_ReturnsNotFound()
        {
            var refUid = "70000";

            var sut = this.BuildSut();

            var result = await sut.GetAttachments(refUid) as NotFoundResult;
            Assert.NotNull(result);

            this.mockWorklistService.Verify(x => x.GetAttachments(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetAttachments_Valid_ReturnsAttachments()
        {
            var refUid = "70000";
            var attachUid = "8000";

            var mockAttachmentResult =new List<ErsdocAttachmentDto>
            {
                new ErsdocAttachmentDto
                {
                    RefDocUniqueId = refUid,
                    AttachId = attachUid
                }
            };

            this.mockWorklistService.Setup(x => x.GetAttachments(It.IsAny<string>())).ReturnsAsync(mockAttachmentResult);

            var sut = this.BuildSut();

            var result = await sut.GetAttachments(refUid) as OkObjectResult;
            Assert.NotNull(result);

            mockWorklistService.Verify(x => x.GetAttachments(It.IsAny<string>()), Times.Once);
        }

        private WorklistController BuildSut() => new(this.mockWorklistService.Object, this.mockLogger.Object, this.mockHttpClientFactory.Object);

    }
}
