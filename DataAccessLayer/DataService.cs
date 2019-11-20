using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;
using StackOverFlow;

namespace DataAccessLayer
{
    public class DataService : IDataService
    {
        // User
        public User GetUser(string username)
        {
            using var db = new StackoverflowContext();
            
            return db.Users.Find(username);
        }

        public List<User> GetUsers()
        {
            using var db = new StackoverflowContext();
            
            return db.Users.ToList();
        }

        public User CreateUser(string username, string password, string email ,string salt, DateTime date)
        {
            using var db = new StackoverflowContext();
            
            var user = new User()
            {
                UserName = username,
                Password = password,
                Email = email,
                Salt = salt,
                CreationDate = date
            };
            
            db.Users.Add(user);
            
            db.SaveChanges();
            
            return user;
        }
        
        public Boolean UpdateUser(string username, string password, string email, string salt)
        {
            using var db = new StackoverflowContext();
    
            var user = db.Users.Find(username);

            if (user != null)
            {
                if (password != null) user.Password = password;
                if (email != null) user.Email = email;
                if (salt != null) user.Salt = salt;

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public Boolean DeleteUser(string username)
        {
            using var db = new StackoverflowContext();

            var user = db.Users.Find(username);

            if (user != null)
            {
                db.Users.Remove(user);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Marking
        public Marking GetMarking(int idmarking)
        {
            using var db = new StackoverflowContext();
            
            return db.Markings.Find(idmarking);
        }

        public List<Marking> GetMarkings(string username)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from mark in db.Markings
                join post in db.Posts on mark.PostId equals post.Id into ps
                from post in ps.DefaultIfEmpty()
                join comment in db.Comments on mark.CommentId equals comment.Id into ms
                from comment in ms.DefaultIfEmpty()
                where mark.Username == username
                select new Marking()
                {
                    Id = mark.Id, Annotation = mark.Annotation, Date = mark.Date, Username = mark.Username, CommentId = mark.CommentId, PostId = mark.PostId,
                    Post = new Post {Id = post.Id, Body = post.Body, Score = post.Score},
                    Comment = new Comment {Id = comment.Id, TextContain = comment.TextContain, Score = comment.Score}
                };
            
            var markings =  query.ToList();

            foreach (var marking in markings)
            {
                if(marking.PostId != null) marking.Post.Tags = GetTags(marking.Post.Id);
            }

            return markings;
        }
        
        public Boolean UpdateMarking(int markid, string annotation)
        {
            using var db = new StackoverflowContext();
    
            var marking = db.Markings.Find(markid);

            if (marking != null)
            {
                if (annotation != null) marking.Annotation = annotation;

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public Marking CreateMarking(string annotation, string username, int? postid, int? commentid, DateTime date)
        {
            using var db = new StackoverflowContext();
            
            int nextId;
            try
            {
                nextId = db.Markings.Max(x => x.Id) + 1;
            }
            catch (Exception e)
            {
                nextId = 1;
            }
            
            var marking = new Marking()
            {
                Id = nextId,
                Annotation = annotation,
                Date = date,
                Username = username,
                PostId = postid,
                CommentId = commentid
            };
            
            db.Markings.Add(marking);
            
            db.SaveChanges();
            
            return marking;
        }
        
        public Boolean DeleteMarking(int markid)
        {
            using var db = new StackoverflowContext();

            var marking = db.Markings.Find(markid);

            if (marking != null)
            {
                db.Markings.Remove(marking);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        // SearchHistory
        public SearchHistory GetSearchHistory(int searchhistoryid)
        {
            using var db = new StackoverflowContext();
            
            return db.SearchHistories.Find(searchhistoryid);
        }
        
        public List<SearchHistory> GetSearchHistories(string username)
        {
            using var db = new StackoverflowContext();

            var query =
                from searchhistory in db.SearchHistories
                where searchhistory.UserName == username
                select new SearchHistory(){Id = searchhistory.Id, Text = searchhistory.Text, Date = searchhistory.Date, UserName = searchhistory.UserName};
            
            return db.SearchHistories.ToList();
        }
        
        public SearchHistory CreateSearchHistory(string text, string username, DateTime date)
        {
            using var db = new StackoverflowContext();
            
            int nextId;
            try
            {
                nextId = db.SearchHistories.Max(x => x.Id) + 1;
            }
            catch (Exception e)
            {
                nextId = 1;
            }
            
            var searchHistory = new SearchHistory()
            {
                Id = nextId,
                Text = text,
                Date = date,
                UserName = username,
            };
            
            db.SearchHistories.Add(searchHistory);
            
            db.SaveChanges();
            
            return searchHistory;
        }
        
        public Boolean DeleteSearchHistory(int searchhistoryid)
        {
            using var db = new StackoverflowContext();

            var searchhistory = db.SearchHistories.Find(searchhistoryid);

            if (searchhistory != null)
            {
                db.SearchHistories.Remove(searchhistory);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Comment
        public List<Comment> GetComments(int postid)
        {
            using var db = new StackoverflowContext();

            var query =
                from comment in db.Comments
                join qauser in db.QaUsers on comment.UserId equals qauser.Id
                where comment.PostId == postid
                select new Comment()
                {
                    Id = comment.Id, Score = comment.Score, CreationDate = comment.CreationDate,
                    PostId = comment.PostId, TextContain = comment.TextContain, UserId = comment.UserId,
                    QAUser = new QAUser()
                    {
                        Id = qauser.Id, Age = qauser.Age, CreationDate = qauser.CreationDate,
                        DisplayName = qauser.DisplayName, UserLocation = qauser.UserLocation
                    }
                };

            return query.ToList();
        }

        // Tag
        public List<Tag> GetTags(int postid)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from tag in db.Tags
                join describe in db.Describes on tag.Id equals describe.TagId 
                where describe.PostId == postid
                select new Tag(){Id = tag.Id, Name = tag.Name};
            
            return query.ToList();
        }
        
        // Question
        public Question GetDetailQuestion(int postid)
        {
            using var db = new StackoverflowContext();

            var comments = GetComments(postid);
            var tags = GetTags(postid);
            var answers = GetAnswers(postid);

            var query =
                from post in db.Posts
                join question in db.Questions on post.Id equals question.Id
                join answer in db.Posts on question.AcceptAnswer equals answer.Id into ps
                from answer in ps.DefaultIfEmpty()
                where post.Id == postid
                select new Question()
                {
                    Id = question.Id, Title = question.Title, ClosedDate = question.ClosedDate, AcceptAnswer = question.AcceptAnswer,
                    AcceptAnswerPost = new Post()
                    {
                        Id = answer.Id, Body = answer.Body, Score = answer.Score, CreationDate = answer.CreationDate,
                        UserId = answer.UserId
                    },
                    Answers = answers,
                    Post = new Post()
                    {
                        Id = post.Id, Body = post.Body, Score = post.Score,
                        Comments = comments,
                        Tags = tags
                    }
                };
            return query.FirstOrDefault();
        }

        // Post
        public List<Post> GetAnswers(int postid)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from post in db.Posts
                join answer in db.Answers on post.Id equals answer.Id
                where answer.QuestionId == postid
                select new Post() {Id = post.Id, Body = post.Body, Score = post.Score};

            var answers = query.ToList();

            foreach (var answer in answers)
            {
                answer.Comments = GetComments(answer.Id);
                answer.Tags = GetTags(answer.Id);
            }
            
            return answers;
        }
        
        public List<Post> searchPosts(string searchtext)
        {
            using var db = new StackoverflowContext();
            var search = "java";
            var function = db.Posts
                .FromSqlRaw("SELECT * FROM exactMatchQuery(\'"+searchtext+"\')")
                .Select(post => new Post(){Id = post.Id, Body = post.Body, Title = post.Title})
                .ToList(); 

            var posts = function.ToList();

            return posts;
        }

        // Answer
        public Answer getAnswer(int postid)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from answer in db.Answers
                where answer.Id == postid
                select new Answer() {Id = answer.Id, QuestionId = answer.QuestionId};
            
            return query.FirstOrDefault();
        }
    }
}
