using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseService;
using StackOverFlow;

namespace DataAccessLayer
{
    public class DataService : IDataService
    {
        // User
        public User GetUser(string username)
        {
            using var db = new StackoverflowContex();
            
            var query =
                from o in db.User
                where o.Username == username
                select new User(){Username = o.Username, Email = o.Email, Password = o.Password, Salt = o.Salt};
            return query.FirstOrDefault();
        }

        public User CreateUser(string username, string password, string email ,string salt)
        {
            using var db = new StackoverflowContex();
            
            var user = new User()
            {
                Username = "Aurélien",
                Password = "test",
                Email = "test",
                Salt = "test"
            };
            
            db.User.Add(user);
            
            db.SaveChanges();
            
            return user;
        }

        // Post
        public Post GetPost(int postid)
        {
            using var db = new StackoverflowContex();

            var query =
                from o in db.Post
                where o.Id == postid
                select new Post() {Id = o.Id, Body = o.Body, Score = o.Score};
            return query.FirstOrDefault();
        }
        
        /*public List<Post> GetPosts(int userId)
        {
            if (_users.FirstOrDefault(x => x.Id == userId) == null)
                throw new ArgumentException("User not found");
            return _posts;
        }*/

    }
}
