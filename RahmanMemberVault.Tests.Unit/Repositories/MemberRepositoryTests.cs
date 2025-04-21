using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Core.Entities;
using RahmanMemberVault.Infrastructure.Data;
using RahmanMemberVault.Infrastructure.Repositories;
using Xunit;

namespace RahmanMemberVault.Tests.Unit.Repositories
{
    public class MemberRepositoryTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddMemberAsync_PersistsEntity() // Test for AddMemberAsync
        {
            // Arrange
            await using var context = CreateDbContext(); // Create a new in-memory database context
            var repo = new MemberRepository(context); // Create a new repository instance with inmemory context
            var member = new Member
            {
                Name = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                IsActive = true,
                DateJoined = DateTime.UtcNow
            };

            // Act
            var result = await repo.AddMemberAsync(member); // Call the method to add a member

            // Assert
            result.Id.Should().BeGreaterThan(0); // Assert that the ID is generated
            var fromDb = await context.Members.FindAsync(result.Id); // Retrieve the member from the database
            fromDb.Should().NotBeNull(); // Assert that the member exists in the database
            fromDb.Name.Should().Be(member.Name); // Assert that the name matches
        }

        [Fact]
        public async Task GetAllMembersAsync_ReturnsAllMembers() // Test for GetAllMembersAsync
        {
            // Arrange
            await using var context = CreateDbContext();
            context.Members.AddRange(new List<Member>
            {
                new Member { Name = "A", Email = "a@x.com", PhoneNumber = "111", IsActive = true, DateJoined = DateTime.UtcNow },
                new Member { Name = "B", Email = "b@x.com", PhoneNumber = "222", IsActive = false, DateJoined = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var repo = new MemberRepository(context);

            // Act
            var list = await repo.GetAllMembersAsync();

            // Assert
            list.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetMemberByIdAsync_ReturnsMember_WhenFound() // Test for GetMemberByIdAsync
        {
            // Arrange
            await using var context = CreateDbContext();
            var entity = new Member { Name = "test", Email = "test@test.com", PhoneNumber = "123456789", IsActive = true, DateJoined = DateTime.UtcNow };
            context.Members.Add(entity); // Add the member to the in-memory database
            await context.SaveChangesAsync();
            var repo = new MemberRepository(context);

            // Act
            var result = await repo.GetMemberByIdAsync(entity.Id); // Call the method to get the member by ID

            // Assert
            result.Should().NotBeNull(); // Assert that the result is not null
            result.Id.Should().Be(entity.Id);
        }

        [Fact]
        public async Task GetMemberByIdAsync_Throws_KeyNotFound_WhenNotFound() // Test for GetMemberByIdAsync when member not found
        {
            // Arrange
            await using var context = CreateDbContext();
            var repo = new MemberRepository(context);

            // Act
            Func<Task> act = () => repo.GetMemberByIdAsync(999); // Attempt to get a non-existing member

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>(); // Assert that a KeyNotFoundException is thrown
        }

        [Fact]
        public async Task UpdateMemberAsync_UpdatesExistingEntity() // Test for UpdateMemberAsync
        {
            // Arrange
            await using var context = CreateDbContext();
            var existing = new Member { Name = "test", Email = "test@test.com", PhoneNumber = "1234567890", IsActive = false, DateJoined = DateTime.UtcNow };
            context.Members.Add(existing); // Add the member to the in-memory database
            await context.SaveChangesAsync();
            var repo = new MemberRepository(context); // Create a new repository instance

            // Modify
            existing.Name = "test Updated"; // Update the name
            existing.IsActive = true; // Update the active status

            // Act
            var updated = await repo.UpdateMemberAsync(existing); // Call the method to update the member

            // Assert
            updated.Name.Should().Be("test Updated"); // Assert that the name is updated
            updated.IsActive.Should().BeTrue(); // Assert that the active status is updated
            var fromDb = await context.Members.FindAsync(existing.Id); // Retrieve the updated member from the database
            fromDb.Name.Should().Be("test Updated");
        }

        [Fact]
        public async Task UpdateMemberAsync_Throws_KeyNotFound_WhenNotFound() // Test for UpdateMemberAsync when member not found
        {
            // Arrange
            await using var context = CreateDbContext();
            var repo = new MemberRepository(context);
            var dummy = new Member { Id = 123, Name = "test", Email = "test@test.com", PhoneNumber = "123436790", IsActive = true, DateJoined = DateTime.UtcNow };

            // Act
            Func<Task> act = () => repo.UpdateMemberAsync(dummy); // Attempt to update a non-existing member

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>(); // Assert that a KeyNotFoundException is thrown
        }

        [Fact]
        public async Task DeleteMemberAsync_RemovesEntity_WhenExists() // Test for DeleteMemberAsync
        {
            // Arrange
            await using var context = CreateDbContext();
            var entity = new Member { Name = "test", Email = "test@test.com", PhoneNumber = "1234567890", IsActive = true, DateJoined = DateTime.UtcNow };
            context.Members.Add(entity); // Add the member to the in-memory database
            await context.SaveChangesAsync();  
            var repo = new MemberRepository(context);

            // Act
            var result = await repo.DeleteMemberAsync(entity.Id); // Call the method to delete the member

            // Assert
            result.Should().BeTrue(); // Assert that the result is true
            var all = await context.Members.ToListAsync(); // Retrieve all members from the database
            all.Should().BeEmpty(); // Assert that the member is deleted
        }

        [Fact]
        public async Task DeleteMemberAsync_ReturnsFalse_WhenNotFound()  // Test for DeleteMemberAsync when member not found
        {
            // Arrange
            await using var context = CreateDbContext();
            var repo = new MemberRepository(context);

            // Act
            var result = await repo.DeleteMemberAsync(999); // Attempt to delete a non-existing member

            // Assert
            result.Should().BeFalse(); // Assert that the result is false
        }
    }
}