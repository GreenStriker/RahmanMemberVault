using RahmanMemberVault.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahmanMemberVault.Core.Interfaces
{
    public interface IMemberRepository
    {
        
        /// Gets all members.
        Task<IEnumerable<Member>> GetAllMembersAsync();
        
        /// Gets a member by ID.
        Task<Member> GetMemberByIdAsync(int id);
        
        /// Adds a new member.
        Task AddMemberAsync(Member member);
        
        /// Updates an existing member.
        Task UpdateMemberAsync(Member member);

        /// Deletes a member by ID.
        Task DeleteMemberAsync(int id);
    }
}
