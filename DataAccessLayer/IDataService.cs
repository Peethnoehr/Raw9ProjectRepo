using System.Collections.Generic;
using StackOverFlow;

namespace DataAccessLayer
{
    public interface IDataService
    {
        User GetUser(string username);
        User CreateUser(string username, string password, string email, string salt);

        Post GetPost(int postid);
    }
}