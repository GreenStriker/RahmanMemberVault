using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahmanMemberVault.Application.DTOs
{
    // Data Transfer Object for presenting member information.
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
    }
}
