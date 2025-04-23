using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Core.Entities;
using RahmanMemberVault.Core.Interfaces;
using RahmanMemberVault.Infrastructure.Data;

namespace RahmanMemberVault.Infrastructure.Repositories
{
    // Repository implementation for Member entity using Entity Framework Core.
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        // Initializes the repository with the specified database context.
        public MemberRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Retrieves all members as a read-only list without tracking.
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _dbContext.Members
                .AsNoTracking()
                .ToListAsync();
        }

        // Retrieves a single member by its unique identifier or throws if not found.
        public async Task<Member> GetMemberByIdAsync(int id)
        {
            var member = await _dbContext.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
                throw new KeyNotFoundException($"Member with ID {id} was not found.");

            return member;
        }

        // Adds a new member to the database, unless the email is already taken.
        public async Task<Member> AddMemberAsync(Member member)
        {
            // ◼ Duplicate‐email check
            var emailTaken = await _dbContext.Members
                .AnyAsync(m => m.Email == member.Email);
            if (emailTaken)
                throw new InvalidOperationException($"Email '{member.Email}' is already in use.");

            var entry = await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        // Updates an existing member, unless the new email conflicts with another record.
        public async Task<Member> UpdateMemberAsync(Member member)
        {
            var existing = await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == member.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Member with ID {member.Id} was not found.");

            // ◼ Duplicate‐email check (exclude self)
            var emailTaken = await _dbContext.Members
                .AnyAsync(m => m.Email == member.Email && m.Id != member.Id);
            if (emailTaken)
                throw new InvalidOperationException($"Email '{member.Email}' is already in use.");

            // Apply updates
            existing.Name = member.Name;
            existing.Email = member.Email;
            existing.PhoneNumber = member.PhoneNumber;
            existing.IsActive = member.IsActive;
            existing.UpdatedOn = DateTime.UtcNow;

            _dbContext.Members.Update(existing);
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        // Deletes a member record by its identifier or returns false if not found.
        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
                return false;

            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
