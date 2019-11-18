using System;

namespace StackOverFlow
{
    public class User
    {
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
    }
}