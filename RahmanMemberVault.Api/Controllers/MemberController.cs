using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Interfaces;

namespace RahmanMemberVault.Api.Controllers
{
    // API controller for managing Member resources.
    // Provides endpoints to create, read, update, and delete members.
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _service;

        // Initializes a new instance of <see cref="MemberController"/> with the specified member service.
        public MemberController(IMemberService service)
        {
            _service = service;
        }

        // Retrieves all members.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            //throw new System.NullReferenceException("This is a test exception"); // Simulate an error for testing purposes
            var members = await _service.GetAllMembersAsync();
            return Ok(members); // Return 200 OK with the list of members
        }

        // Retrieves a member by its unique identifier.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _service.GetMemberByIdAsync(id);
            return Ok(member); // Return 200 OK with the member details
        }

        // Creates a new member.
        [HttpPost]
        public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberDto dto)
        {
            var created = await _service.AddMemberAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); // Return 201 Created with the location of the new member
        }


        // Updates an existing member.
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MemberDto>> Update(int id, [FromBody] UpdateMemberDto dto)
        {
            if (id != dto.Id) // Check if the ID in the URL matches the ID in the payload
            {
                return BadRequest("ID in URL does not match ID in payload.");
            }

            var updated = await _service.UpdateMemberAsync(dto);
            return Ok(updated); // Return the updated member
        }

        // Deletes a member by its identifier.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool removed = await _service.DeleteMemberAsync(id);
            if (!removed) // Check if the deletion was successful
            {
                return NotFound(); // memebr not found
            }
            return NoContent(); // Return 204 No Content if deletion was successful
        }
    }
}
