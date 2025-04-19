using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Interfaces;
using RahmanMemberVault.Core.Entities;
using RahmanMemberVault.Core.Interfaces;

namespace RahmanMemberVault.Application.Services
{
    // Implements business operations for Member management.
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IMapper _mapper;

        // Initializes the service with the specified repository and mapper.
        public MemberService(IMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Retrieves all members and maps them to DTOs.
        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        {
            var members = await _repository.GetAllMembersAsync();
            return _mapper.Map<IEnumerable<MemberDto>>(members); // Map to DTOs
        }

        // Retrieves a member by ID and maps it to a DTO.
        public async Task<MemberDto> GetMemberByIdAsync(int id)
        {
            var member = await _repository.GetMemberByIdAsync(id); // Get member by ID from repository
            return _mapper.Map<MemberDto>(member); // Map to DTO
        }

        // Adds a new member and maps it to a DTO.
        public async Task<MemberDto> AddMemberAsync(CreateMemberDto dto)
        {
            var member = _mapper.Map<Member>(dto); // Map DTO to entity
            var created = await _repository.AddMemberAsync(member); // Add member to repository
            return _mapper.Map<MemberDto>(created); // Map to DTO
        }

        // Updates an existing member and maps it to a DTO.
        public async Task<MemberDto> UpdateMemberAsync(UpdateMemberDto dto)
        {
            var member = _mapper.Map<Member>(dto); // Map DTO to entity
            var updated = await _repository.UpdateMemberAsync(member); // Update member in repository
            return _mapper.Map<MemberDto>(updated); // Map to DTO
        }

        // Deletes a member by ID and returns a boolean indicating success.
        public async Task<bool> DeleteMemberAsync(int id)
        {
            return await _repository.DeleteMemberAsync(id); // Delete member by ID from repository
        }
    }
}

