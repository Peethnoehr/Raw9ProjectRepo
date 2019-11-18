using System;

namespace DataAccess
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}