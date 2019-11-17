using System.Collections.Generic;
using System.ComponentModel.Design;
using StackOverFlow;

namespace DataAccessLayer
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }

        List<Marking> Marking { get; set; }
    }
}
