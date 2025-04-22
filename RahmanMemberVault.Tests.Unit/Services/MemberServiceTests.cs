using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using AutoMapper;
using Moq;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Services;
using RahmanMemberVault.Core.Entities;
using RahmanMemberVault.Core.Interfaces;
using Xunit;

namespace RahmanMemberVault.Tests.Unit.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly MemberService _service;

        public MemberServiceTests()
        {
            _repoMock = new Mock<IMemberRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new MemberService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllMembers_ReturnsMappedDtoList() // Test for GetAllMembersAsync
        {
            // Arrange
            var members = new List<Member>
            {
                new Member { Id = 1, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" }
            }; 
            var dtos = new List<MemberDto>
            {
                new MemberDto { Id = 1, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" }
            };

            _repoMock.Setup(r => r.GetAllMembersAsync())
                     .ReturnsAsync(members); // Mocking the repository call
            _mapperMock.Setup(m => m.Map<IEnumerable<MemberDto>>(members)) // Mocking the mapping
                       .Returns(dtos);

            // Act
            var result = await _service.GetAllMembersAsync(); // Calling the service method

            // Assert
            result.Should().BeEquivalentTo(dtos); // Asserting the result
        }

        [Fact]
        public async Task GetMemberById_ReturnsMappedDto_WhenFound() // Test for GetMemberByIdAsync
        {
            // Arrange
            var member = new Member { Id = 2, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var dto = new MemberDto { Id = 2, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };

            _repoMock.Setup(r => r.GetMemberByIdAsync(2))
                     .ReturnsAsync(member);  
            _mapperMock.Setup(m => m.Map<MemberDto>(member))
                       .Returns(dto);  

            // Act
            var result = await _service.GetMemberByIdAsync(2);  

            // Assert
            result.Should().BeEquivalentTo(dto);  
        }

        [Fact]
        public async Task GetMemberById_Throws_WhenNotFound() // Test for GetMemberByIdAsync when member not found
        {
            // Arrange
            _repoMock.Setup(r => r.GetMemberByIdAsync(It.IsAny<int>()))
                     .ThrowsAsync(new KeyNotFoundException());

            // Act
            Func<Task> act = () => _service.GetMemberByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task AddMember_CallsRepo_AndReturnsMappedDto() // Test for AddMemberAsync
        {
            // Arrange
            var createDto = new CreateMemberDto { Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var entity = new Member { Id = 3, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var dto = new MemberDto { Id = 3, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };

            _mapperMock.Setup(m => m.Map<Member>(createDto)).Returns(entity);
            _repoMock.Setup(r => r.AddMemberAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<MemberDto>(entity)).Returns(dto);

            // Act
            var result = await _service.AddMemberAsync(createDto);

            // Assert
            result.Should().BeEquivalentTo(dto);
            _repoMock.Verify(r => r.AddMemberAsync(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateMember_CallsRepo_AndReturnsMappedDto() // Test for UpdateMemberAsync
        {
            // Arrange
            var updateDto = new UpdateMemberDto { Id = 4, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var entity = new Member { Id = 4, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var dto = new MemberDto { Id = 4, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };

            _mapperMock.Setup(m => m.Map<Member>(updateDto)).Returns(entity);
            _repoMock.Setup(r => r.UpdateMemberAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<MemberDto>(entity)).Returns(dto);

            // Act
            var result = await _service.UpdateMemberAsync(updateDto);

            // Assert
            result.Should().BeEquivalentTo(dto);
            _repoMock.Verify(r => r.UpdateMemberAsync(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateMember_Throws_WhenRepoThrows() // Test for UpdateMemberAsync when repository throws
        {
            // Arrange
            var updateDto = new UpdateMemberDto { Id = 5, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };
            var entity = new Member { Id = 5, Name = "Mustafiz", Email = "Mustafiz@gmail.com", PhoneNumber = "1234567890" };

            _mapperMock.Setup(m => m.Map<Member>(updateDto)).Returns(entity);
            _repoMock.Setup(r => r.UpdateMemberAsync(entity)).ThrowsAsync(new KeyNotFoundException());

            // Act
            Func<Task> act = () => _service.UpdateMemberAsync(updateDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteMember_ReturnsTrue_WhenDeleted() // Test for DeleteMemberAsync
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteMemberAsync(6)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteMemberAsync(6);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteMember_ReturnsFalse_WhenNotFound() // Test for DeleteMemberAsync when member not found
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteMemberAsync(7)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteMemberAsync(7);

            // Assert
            result.Should().BeFalse();
        }
    }
}
