using AutoMapper;
using Hospital.Services.BedAPI.Controllers;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hospital.Services.Bedapi.Tests
{
    public class BedsControllerTests
    {

        private readonly Mock<IBed> _mockBed;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BedsController _controller;

        public BedsControllerTests()
        {
            _mockBed = new Mock<IBed>();
            _mockMapper = new Mock<IMapper>();
            _controller = new BedsController(_mockBed.Object, _mockMapper.Object);
        }



        [Fact]
        public async Task GetAllBeds_ReturnsListOfBeds()
        {
            // Arrange
            var expectedBeds = new List<Beds>
            {
                new Beds { Id = 1, No = "Bed 1",BedCatId=1,Description="description"  },
                new Beds { Id = 1, No = "Bed 1",BedCatId=1,Description="description"  }
            };

            _mockBed.Setup(repo => repo.GetAllBeds()).ReturnsAsync(expectedBeds);

            // Act
            var result = await _controller.GetAllBeds();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Beds>>>(result);
            var beds = Assert.IsAssignableFrom<IEnumerable<Beds>>(actionResult.Value);
            Assert.Equal(expectedBeds, beds);
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async Task GetBedById_ReturnsBed(int id, int bedcatid)
        {
            // Arrange
            var bedDTO = new BedDTO { No = "New Bed", BedCatId = bedcatid, Description = "description" };

            _mockMapper.Setup(mapper => mapper.Map<Beds>(bedDTO)).Returns(GetTestBed(id, bedcatid));
            _mockBed.Setup(repo => repo.GetBedById(id)).ReturnsAsync(GetTestBed(id, bedcatid));

            // Act
            var result = await _controller.GetBedById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bed = Assert.IsType<Beds>(okResult.Value);
            Assert.Equal(id, bed.Id);
        }


        private Beds GetTestBed(int id, int bedcatid)
        {
            return new Beds { Id = id, No = "Bed 1", BedCatId = bedcatid, Description = "description" };
        }

        public static IEnumerable<object[]> AddBedTestData =>
           new List<object[]>
           {
                new object[] { new BedDTO { No = "NewBed",BedCatId=1,Description="Description" }, new Beds { Id = 1, No = "NewBed",BedCatId=1,Description="Description" }, new Result { Success = true } },
                new object[] { null, null, new Result { Success = false, Message = "Invalid data" } }
           };

        [Theory]
        [MemberData(nameof(AddBedTestData))]
        public async Task AddBed_ReturnsCorrectResult(BedDTO bedDTO, Beds bed, Result expectedResult)
        {
            // Arrange
            _mockMapper.Setup(mapper => mapper.Map<Beds>(bedDTO)).Returns(bed);
            _mockBed.Setup(repo => repo.AddBed(bed)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddBed(bedDTO);

            // Assert
            if (bedDTO != null)
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(expectedResult, actionResult.Value);
            }
            else
            {
                var actionResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(expectedResult, actionResult.Value);
            }
        }

        [Fact]
        public async Task UpdateBed_ValidModel_ReturnsOkResult()
        {
            // Arrange
            int bedId = 1;
            var bedDTO = new BedDTO { No = "Updated Bed", Description = "Description", BedCatId = 1 };
            var existingBed = new Beds { Id = bedId, No = "Existing Bed", Description = "Description", BedCatId = 1 };

            _mockMapper.Setup(mapper => mapper.Map<Beds>(bedDTO)).Returns(existingBed);
            _mockBed.Setup(repo => repo.UpdateBed(existingBed)).ReturnsAsync(new Result { Success = true, Message = "Update Success" });

            // Act
            var result = await _controller.UpdateBed(bedId, bedDTO);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var operationResult = actionResult.Value as Result;
            Assert.NotNull(operationResult);
            Assert.True(operationResult.Success);
            Assert.Equal("Update Success", operationResult.Message);
        }

        public static IEnumerable<object[]> DeleteBedTestData =>
           new List<object[]>
           {
                new object[] { 1, new Result { Success = true } },
                new object[] { 2, new Result { Success = false, Message = "Bed Not Found" } }
           };

        [Theory]
        [MemberData(nameof(DeleteBedTestData))]
        public async Task DeleteBed_ReturnsCorrectResult(int id, Result expectedResult)
        {
            // Arrange

            _mockBed.Setup(repo => repo.DeleteBed(id)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteBed(id);

            // Assert
            if (expectedResult.Success == true)
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);
                var data = actionResult.Value as Result;
                Assert.True(data.Success);
                Assert.Equal(expectedResult, data);
            }
            else
            {
                var actionResult = Assert.IsType<NotFoundObjectResult>(result);
                var data = actionResult.Value as Result;
                Assert.False(data.Success);
                Assert.Equal(expectedResult, data);
            }
        }
    }

}

