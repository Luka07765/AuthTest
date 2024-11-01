using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthLearning.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; }

        // Foreign key to User
        public string UserId { get; set; }

        // Navigation property
        public IdentityUser User { get; set; }
    }
}
