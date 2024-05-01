using ChatHubSolution.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatHubSolution.Data.Entities
{
    public class User : IdentityUser, IDateTracking
    {
        public User()
        {

        }

        public User(string id, string fullName, string email)
        {
            Id = id;
            FullName = fullName;
            Email = email;
        }

        [MaxLength(50)]
        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
