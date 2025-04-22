using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RahmanMemberVault.Api.Controllers;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Interfaces;
using RahmanMemberVault.Core.Interfaces;
using Xunit;

namespace RahmanMemberVault.Tests.Unit.Controllers
{
    public class MemberControllerTests
    {
        private readonly Mock<IMemberService> _serviceMock; // Mocking the service
        private readonly MemberController _controller;

        public MemberControllerTests() 
        {
            _serviceMock = new Mock<IMemberService>();
            _controller = new MemberController(_serviceMock.Object); // Initializing the controller with the mocked service
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithList() // Test for GetAllMembersAsync
        {
            // Arrange
            var dtos = new List<MemberDto>
            {
                new MemberDto { Id = 1, Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" }
            };
            _serviceMock.Setup(s => s.GetAllMembersAsync()).ReturnsAsync(dtos); // Mocking the service call

            // Act
            var actionResult = await _controller.GetAll(); // Calling the controller method
            var okResult = actionResult.Result as OkObjectResult;  // Getting the result as OkObjectResult

            // Assert
            okResult.Should().NotBeNull(); // Asserting that the result is not null
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK); // Asserting the status code
            okResult.Value.Should().BeEquivalentTo(dtos); // Asserting the value
        }

        [Fact]
        public async Task GetById_ReturnsOk_WithMember() // Test for GetMemberByIdAsync
        {
            // Arrange
            var dto = new MemberDto { Id = 2, Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" };
            _serviceMock.Setup(s => s.GetMemberByIdAsync(2)).ReturnsAsync(dto);

            // Act
            var actionResult = await _controller.GetById(2);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(dto);
        }


        [Fact]
        public async Task Create_ReturnsCreatedAtAction() // Test for AddMemberAsync
        {
            // Arrange
            var createDto = new CreateMemberDto { Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" };
            var created = new MemberDto { Id = 5, Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" };
            _serviceMock.Setup(s => s.AddMemberAsync(createDto)).ReturnsAsync(created);

            // Act
            var actionResult = await _controller.Create(createDto);
            var createdResult = actionResult.Result as CreatedAtActionResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.Value.Should().BeEquivalentTo(created);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful() // Test for UpdateMemberAsync
        {
            // Arrange
            var updateDto = new UpdateMemberDto { Id = 6, Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" };
            var updated = new MemberDto { Id = 6, Name = "Test", Email = "test@test.com", PhoneNumber = "1234567890" };
            _serviceMock.Setup(s => s.UpdateMemberAsync(updateDto)).ReturnsAsync(updated); // Mocking the service call

            // Act
            var actionResult = await _controller.Update(updateDto.Id, updateDto);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(updated);
        }


        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted() // Test for DeleteMemberAsync
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteMemberAsync(10)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(10);
            var noContent = result as NoContentResult;

            // Assert
            noContent.Should().NotBeNull();
            noContent.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing() // Test for DeleteMemberAsync
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteMemberAsync(11)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(11);
            var notFound = result as NotFoundResult;

            // Assert
            notFound.Should().NotBeNull();
            notFound.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
