using System;
using System.Collections.Generic;

namespace QuizPracticeApi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserDetail? UserDetail { get; set; }
    }
}
