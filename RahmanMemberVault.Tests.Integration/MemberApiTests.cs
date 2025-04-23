using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RahmanMemberVault.Application.DTOs;
using Xunit;

namespace RahmanMemberVault.Tests.Integration
{
    public class MemberApiTests : IClassFixture<MemberApiTestsFixture>
    {
        private readonly HttpClient _client;

        public MemberApiTests(MemberApiTestsFixture fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMembers()
        {
            var response = await _client.GetAsync("/api/v1/member");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var list = await response.Content.ReadFromJsonAsync<List<MemberDto>>();
            list.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAndRetrieveMember_Succeeds()
        {
            // Create
            var createDto = new CreateMemberDto { Name = "Test Case 1", Email = "test1@test.com", PhoneNumber = "1234567890" };
            var createResponse = await _client.PostAsJsonAsync("/api/v1/member", createDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var location = createResponse.Headers.Location;
            location.Should().NotBeNull();

            // Retrieve
            var getResponse = await _client.GetAsync(location);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var returned = await getResponse.Content.ReadFromJsonAsync<MemberDto>();
            returned.Name.Should().Be(createDto.Name);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotFound()
        {
            var response = await _client.GetAsync("/api/member/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Returns204_WhenExists()
        {
            // Create
            var createDto = new CreateMemberDto { Name = "Test Case 3", Email = "test3@test.com", PhoneNumber = "1234567890" };
            var createResponse = await _client.PostAsJsonAsync("/api/v1/member", createDto);
            var created = await createResponse.Content.ReadFromJsonAsync<MemberDto>();

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/v1/member/{created.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Create_Returns400_OnInvalidPayload()
        {
            var invalid = new CreateMemberDto { Name = "", Email = "invalid", PhoneNumber = "" };
            var response = await _client.PostAsJsonAsync("/api/v1/member", invalid);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Returns404_WhenMissing()
        {
            var updateDto = new UpdateMemberDto { Id = 1234, Name = "Test Case 4", Email = "test4@test.com", PhoneNumber = "1234567890" };
            var response = await _client.PutAsJsonAsync($"/api/member/{updateDto.Id}", updateDto);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_PersistsChanges_WhenValid()
        {
            // Create
            var createDto = new CreateMemberDto { Name = "Test Case 5", Email = "test5@test.com", PhoneNumber = "1234567890" };
            var createResponse = await _client.PostAsJsonAsync("/api/v1/member", createDto);
            var created = await createResponse.Content.ReadFromJsonAsync<MemberDto>();

            // Update
            var updateDto = new UpdateMemberDto { Id = created.Id, Name = "Update_Test", Email = "test5@test.com", PhoneNumber = "1234567890" };
            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/member/{created.Id}", updateDto);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify
            var returned = await updateResponse.Content.ReadFromJsonAsync<MemberDto>();
            returned.Id.Should().Be(created.Id);
        }

    }
}
