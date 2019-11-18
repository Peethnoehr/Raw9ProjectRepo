using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using StackOverFlow;

namespace DataAccessLayer
{
    public class DataService : IDataService
    {
        // User
        public User GetUser(string username)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from o in db.Users
                where o.UserName == username
                select new User(){UserName = o.UserName, Email = o.Email, Password = o.Password, Salt = o.Salt};
            return query.FirstOrDefault();
        }

        public User CreateUser(string username, string password, string email ,string salt)
        {
            using var db = new StackoverflowContext();
            
            var user = new User()
            {
                UserName = username,
                Password = password,
                Email = email,
                Salt = salt
            };
            
            db.Users.Add(user);
            
            db.SaveChanges();
            
            return user;
        }

        // Post
        public Post GetPost(int postid)
        {
            using var db = new StackoverflowContext();

            var query =
                from o in db.Posts
                join c in db.Questions on o.Id equals c.Id
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
