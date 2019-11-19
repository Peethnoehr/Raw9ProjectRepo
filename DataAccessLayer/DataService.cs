﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        
        // Post
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

        
        /*
        public List<Question> SearchPosts(int userId, string searchString) 
        {
            using var db = new StackoverflowContext();

            var query =
                from p in db.Posts
                join q in db.Questions on p.PostId equals q.QuestionId
                where p.Body.Contains(searchString)
                select new Question()
                {
                    QuestionId = q.QuestionId, Title = q.Title, ClosedDate = q.ClosedDate, AcceptAnswer = q.AcceptAnswer, 
                    QuestionPost = new Post(){PostId = q.QuestionId, Body = p.Body, Score = p.Score, CreationDate = p.CreationDate, UserId = p.UserId},
                };

            var searchHistoryEntity = AddToSearchHistory(userId, searchString);
            
            return query.ToList();
        }
        
        public List<Post> GetAllPosts()
        {
            using var db = new StackoverflowContext();
            return db.Posts.ToList();
        }
        
        public Question getQuestion(int questionId)
        {
            using var db = new StackoverflowContext();
            
                var query =
                from p in db.Posts
                join q in db.Questions on p.PostId equals q.QuestionId
                where q.QuestionId.Equals(questionId)
                select new Question()
                {
                    QuestionId = q.QuestionId, Title = q.Title, ClosedDate = q.ClosedDate, AcceptAnswer = q.AcceptAnswer,  
                    QuestionPost = new Post(){PostId = q.QuestionId, Body = p.Body, Score = p.Score, CreationDate = p.CreationDate, UserId = p.UserId}, 
                    Answers = null
                };
                
            List<Question> questions = query.ToList();
            Question question = questions.First();
            question.Answers = GetAnswers(question.QuestionId);
            return question;

        }

        public List<Answer> GetAnswers(int questionId)
        {
            using var db = new StackoverflowContext();

            var queryAnswers =
                from p in db.Posts
                join a in db.Answers on p.PostId equals
                    a.AnswerId // Can maybe be improved by first selecting all answers related to the questionid in a subquery, then joining with post table.
                where a.QuestionId.Equals(questionId)
                select new Answer()
                {
                    AnswerId = a.AnswerId, QuestionId = a.QuestionId,
                    AnswerPost = new Post()
                    {
                        PostId = p.PostId, Body = p.Body, Comments = null, Score = p.Score, //GetComments(p.PostId)
                        CreationDate = p.CreationDate, UserId = p.UserId, 
                    }
                };
            List<Answer> answers = queryAnswers.ToList();
            for (int i = 0; i < answers.Count; i++)
            {
                answers[i].AnswerPost.Comments = GetComments(answers[i].AnswerId);
            }            
            return answers;
        }
        
        /*public List<Post> GetPosts(int userId)
        {
            if (_users.FirstOrDefault(x => x.Id == userId) == null)
                throw new ArgumentException("User not found");
            return _posts;
        }*/
    }
}