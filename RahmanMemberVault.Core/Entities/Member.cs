namespace RahmanMemberVault.Core.Entities
{

    /// Represents a member record in the system.
    public class Member
    {

        /// Unique identifier for the member.
        public int Id { get; set; }


        /// Full name of the member.
        public string Name { get; set; } = string.Empty;


        /// Email address of the member.
        public string Email { get; set; } = string.Empty;


        /// Phone number of the member.
        public string PhoneNumber { get; set; } = string.Empty;


        /// Date when the member joined.
        public DateTime DateJoined { get; set; }


        /// Date when the member record was last updated.
        public DateTime UpdatedOn { get; set; }


        /// Indicates whether the member is currently active.
        public bool IsActive { get; set; } = true;
    }
}
