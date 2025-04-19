using System.Collections.Generic;
using System.Threading.Tasks;
using RahmanMemberVault.Application.DTOs;

namespace RahmanMemberVault.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RahmanMemberVault.Core.Entities;

    // Defines business operations for managing Member entities.
    public interface IMemberService
    {
        // Retrieves all members.
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();

        // Retrieves a member by unique identifier.
        Task<MemberDto> GetMemberByIdAsync(int id);

        // Creates a new member and returns the created entity.
        Task<MemberDto> AddMemberAsync(CreateMemberDto dto);

        // Updates an existing member and returns the updated entity.
        Task<MemberDto> UpdateMemberAsync(UpdateMemberDto dto);

        // Deletes a member by identifier and returns true if deletion succeeded.
        Task<bool> DeleteMemberAsync(int id);
    }
}
