using AutoMapper;
using FluentAssertions;
using Hospital.Services.BedAPI.Controllers;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Services.IServices;
using Moq;

namespace Hospital.Services.BedAPI.Tests
{
    public class BedCategoryControllerTests
    {

        private readonly Mock<IBedCategory> mockBedCategory;
        private readonly Mock<IMapper> mockMapper;
        private readonly BedCategoryController bedCategoryController;


        public BedCategoryControllerTests()
        {
            mockBedCategory = new Mock<IBedCategory>();
            mockMapper = new Mock<IMapper>();
            bedCategoryController = new BedCategoryController(mockBedCategory.Object,mockMapper.Object);  
        }


        [Fact]

        public void GetBedCategories_ReturnsOkResult()
        {
            var expectBedCategories = new List<BedCategory>
            {
                new BedCategory {Id=1, Name="ICU",Description="This is ICU beds"},
                new BedCategory {Id=2, Name="General",Description="This is General beds"}
            };

            mockBedCategory.Should().NotBeNull();
          //  mockBedCategory.Setup(c => c.GetAllBedCategories()).Returns(expectBedCategories);

            var result = bedCategoryController.BedCategories();

            Assert.NotNull(result);
            Assert.IsType<Task<IEnumerable<BedCategory>>>(result);
        }



    }
}
