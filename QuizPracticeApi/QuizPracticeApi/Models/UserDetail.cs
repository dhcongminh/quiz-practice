using System;
using System.Collections.Generic;

namespace QuizPracticeApi.Models
{
    public partial class UserDetail
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? Avatar { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
