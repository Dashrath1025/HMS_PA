using AutoMapper;
using Hospital.Services.BedAPI.Controllers;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hospital.Services.Bedapi.Tests
{
    public class BedCategoryControllerTests
    {
        private readonly Mock<IBedCategory> _mockBedCategory;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BedCategoryController _controller;

        public BedCategoryControllerTests()
        {
            _mockBedCategory = new Mock<IBedCategory>();
            _mockMapper = new Mock<IMapper>();
            _controller = new BedCategoryController(_mockBedCategory.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetBedCategories_ReturnsListOfBedCategories()
        {
            // Arrange
            var expectedBedCategories = new List<BedCategory>
     {
         new BedCategory { Id = 1, Name = "Category 1" },
         new BedCategory { Id = 2, Name = "Category 2" }
     };

            _mockBedCategory.Setup(repo => repo.GetAllBedCategories()).ReturnsAsync(expectedBedCategories);

            // Act
            var result = await _controller.BedCategories();

            // Assert
            var actionResult = Assert.IsType<List<BedCategory>>(result);
            Assert.Equal(expectedBedCategories, actionResult);
        }

        [Fact]
        public async Task AddBedCategory_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var bedCategoryDTO = new BedCategoryDTO { Name = "New Category" };
            var bedCategory = new BedCategory { Name = "New Category" };

            _mockMapper.Setup(mapper => mapper.Map<BedCategory>(bedCategoryDTO)).Returns(bedCategory);
            _mockBedCategory.Setup(repo => repo.AddBedCategory(bedCategory)).ReturnsAsync(new Result { Success = true, Message = "Bed category added successfully." });

            // Act
            var result = await _controller.AddBedCategory(bedCategoryDTO);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var operationResult = actionResult.Value as Result;
            Assert.NotNull(operationResult);
            Assert.True(operationResult.Success);
            Assert.Equal("Bed category added successfully.", operationResult.Message);
        }

        [Fact]
        public async Task UpdateBedCategory_ValidModel_ReturnsOkResult()
        {
            // Arrange
            int categoryId = 1;
            var bedCategoryDTO = new BedCategoryDTO { Name = "Updated Category" };
            var existingBedCategory = new BedCategory { Id = categoryId, Name = "Existing Category" };

            _mockMapper.Setup(mapper => mapper.Map<BedCategory>(bedCategoryDTO)).Returns(existingBedCategory);
            _mockBedCategory.Setup(repo => repo.UpdateBedCategory(existingBedCategory)).ReturnsAsync(new Result { Success = true, Message = "Update Success" });

            // Act
            var result = await _controller.UpdateBedCategory(categoryId, bedCategoryDTO);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var operationResult = actionResult.Value as Result;
            Assert.NotNull(operationResult);
            Assert.True(operationResult.Success);
            Assert.Equal("Update Success", operationResult.Message);
        }

        [Fact]
        public async Task DeleteBedCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            int categoryId = 1;
            var existingBedCategory = new BedCategory { Id = categoryId, Name = "Existing Category" };

            _mockBedCategory.Setup(repo => repo.DeleteBedCategory(categoryId)).ReturnsAsync(new Result { Success = true, Message = "Delete success" });

            // Act
            var result = await _controller.DeleteBedCategory(categoryId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var data = actionResult.Value as Result;
            // Assert.NotNull(data);
            Assert.True(data.Success);
            Assert.Equal("Delete success", data.Message);
        }


        [Fact]
        public async Task DeleteBedCategory_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int categoryId = 1;

            _mockBedCategory.Setup(repo => repo.DeleteBedCategory(categoryId)).ReturnsAsync(new Result { Success = false, Message = "Not found" });

            // Act
            var result = await _controller.DeleteBedCategory(categoryId);

            // Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            var operationResult = actionResult.Value as string;
            Assert.NotNull(operationResult);
            Assert.Equal("Not found", operationResult);
        }

        [Fact]
        public async Task GetBedCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            int categoryId = 1;
            var existingBedCategory = new BedCategory { Id = categoryId, Name = "Existing Category" };

            _mockBedCategory.Setup(repo => repo.GetBedCategoryById(categoryId)).ReturnsAsync(existingBedCategory);

            // Act
            var result = await _controller.GetBedCategory(categoryId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var bedCategory = Assert.IsType<BedCategory>(actionResult.Value);
            Assert.Equal(existingBedCategory, bedCategory);
        }

        [Fact]
        public async Task GetBedCategory_NonExistingCategory_ReturnsNotFoundResult()
        {
            // Arrange
            int categoryId = 1;
            _mockBedCategory.Setup(repo => repo.GetBedCategoryById(categoryId)).ReturnsAsync((BedCategory)null);

            // Act
            var result = await _controller.GetBedCategory(categoryId);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
