using AutoMapper;
using Hospital.Services.BedAPI.Controllers;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services.Bedapi.Tests
{
    public class BedsAllotmentControllerTests
    {
        private readonly Mock<IBedAllotment> _mockBedAllotment;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BedAllotmentController _controller;


        public BedsAllotmentControllerTests()
        {
            _mockBedAllotment = new Mock<IBedAllotment>();
            _mockMapper = new Mock<IMapper>();
            _controller = new BedAllotmentController(_mockBedAllotment.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllBedAllotments_ReturnsListOfBedAllotments()
        {
            // Arrange
            var expectedBedAllotments = new List<BedAllotment> {
                new BedAllotment{Id=1,Pid=1,BedId=1,AllotmentDate=DateTime.Now,DischargeDate=DateTime.Now,Released=false,Note="ok"},
                new BedAllotment{Id=2,Pid=2,BedId=2,AllotmentDate=DateTime.Now,DischargeDate=DateTime.Now,Released=false,Note="ok"}
            };
            _mockBedAllotment.Setup(repo => repo.GetAllBedAllotments()).ReturnsAsync(expectedBedAllotments);

            // Act
            var result = await _controller.GetAllBedAllotments();

            // Assert
            var actionResult = Assert.IsType<List<BedAllotment>>(result);
            // var resultList = Assert.IsAssignableFrom<IEnumerable<BedAllotment>>(actionResult.Value);
            Assert.Equal(expectedBedAllotments, actionResult);
        }

        [Fact]
        public async Task AddBedAllotment_ValidBedAllotment_ReturnsOkResult()
        {
            // Arrange
            var bedAllotmentDTO = new BedAllotmentDTO { Pid = 1, BedId = 1, AllotmentDate = DateTime.Now, DischargeDate = DateTime.Now, Note = "ok" };
            var bedAllotment = new BedAllotment { Id = 1, Pid = 1, BedId = 1, AllotmentDate = DateTime.Now, DischargeDate = DateTime.Now, Released = false, Note = "ok" };
            var expectedResult = new Result { Success = true, Message = "Bed Allotment added successfully." };

            _mockMapper.Setup(mapper => mapper.Map<BedAllotment>(bedAllotmentDTO)).Returns(bedAllotment);
            _mockBedAllotment.Setup(repo => repo.AddBedAllotment(bedAllotment)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddBedAllotment(bedAllotmentDTO);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Result>(actionResult.Value);
            Assert.Equal(expectedResult, resultValue);
        }

        [Fact]
        public async Task UpdateBedAllotment_ValidBedAllotment_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            bool released = false;
            var bedAllotmentDTO = new BedAllotmentDTO { Pid = 1, BedId = 1, AllotmentDate = DateTime.Now, DischargeDate = DateTime.Now, Note = "ok" };
            var bedAllotment = new BedAllotment { Id = 1, Pid = 1, BedId = 1, AllotmentDate = DateTime.Now, DischargeDate = DateTime.Now, Released = false, Note = "ok" };
            var expectedResult = new Result { Success = true, Message = "Update Success" };

            _mockMapper.Setup(mapper => mapper.Map<BedAllotment>(bedAllotmentDTO)).Returns(bedAllotment);
            _mockBedAllotment.Setup(repo => repo.UpdateBedAllotment(bedAllotment)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdateBedAllotment(id, bedAllotmentDTO, released);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Result>(actionResult.Value);
            Assert.Equal(expectedResult, resultValue);
        }

        [Fact]
        public async Task DeleteBedAllotment_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var expectedResult = new Result { Success = true, Message = "Delete Success" };
            _mockBedAllotment.Setup(repo => repo.DeleteBedAllotment(id)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteBedAllotment(id);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Result>(actionResult.Value);
            Assert.Equal(expectedResult, resultValue);
        }

        [Fact]
        public async Task DeleteBedCategory_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int bedId = 1;

            _mockBedAllotment.Setup(repo => repo.DeleteBedAllotment(bedId)).ReturnsAsync(new Result { Success = false, Message = "Not found" });

            // Act
            var result = await _controller.DeleteBedAllotment(bedId);

            // Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            var operationResult = actionResult.Value as string;
            Assert.NotNull(operationResult);
            Assert.Equal("Not found", operationResult);
        }

    }

}

