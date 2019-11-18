using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace DataBaseService
{
    public class DataService
    {
        public AppUser GetUser(int id)
        {
            using var db = new StackoverflowContext();
            return db.AppUsers.Find(id); //find uses primary key to find entity
        }
        
        public List<AppUser> GetUsers()
        {
            using var db = new StackoverflowContext();
            return db.AppUsers.ToList();
        }
        
        public AppUser CreateAppUser(string displayName, string userPassword, string email)
        {
            using var db = new StackoverflowContext();

            var nextId = db.AppUsers.Max(x => x.UserId) + 1;

            var user = new AppUser()
            {
                UserId = nextId,
                DisplayName = displayName,
                CreationDate = DateTime.Now,
                Password = userPassword,
                Email = email
            };
            
            db.AppUsers.Add(user);
            
            db.SaveChanges();

            return db.AppUsers.Find(nextId);
        }
        
        public Boolean UpdateAppUser(int id, string displayname, string password, string email)
        {
            using var db = new StackoverflowContext();
    
            var user = db.AppUsers.Find(id);

            if (user != null)
            {
                if (displayname != null) user.DisplayName = displayname;
                if (password != null) user.Password = password;
                if (email != null) user.Email = email;

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteAppUser(int userid)
        {
            using var db = new StackoverflowContext();

            var user = db.AppUsers.Find(userid);

            if (user != null)
            {
                db.AppUsers.Remove(user);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
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
        
        public SearchHistory AddToSearchHistory(int userId, string searchString)
        {
            using var db = new StackoverflowContext();

            var query =
                from p in db.SearchHistories
                where p.UserId.Equals(userId)
                where p.SearchText.Equals(searchString)
                select new SearchHistory()
                {
                    SearchHistoryId = p.SearchHistoryId, SearchDate = p.SearchDate, SearchText = p.SearchText,
                    UserId = p.UserId
                };

            var qResult = query.ToList();
            try
            {
                if (qResult.Count == 1)
                {
                    SearchHistory toUpdate = db.SearchHistories.Find(qResult[0].SearchHistoryId);
                    toUpdate.SearchDate = DateTime.Now;
                    db.SaveChanges();
                    return db.SearchHistories.Find(toUpdate.SearchHistoryId);
                }
            }
            catch (Exception e){}

            int nextId;
            try
            {
                nextId = db.SearchHistories.Max(x => x.UserId) + 1;
            }
            catch (Exception e)
            {
                nextId = 1;
            }

            var searchHistory = new SearchHistory()
            {
                SearchHistoryId = nextId,
                SearchDate = DateTime.Now,
                SearchText = searchString,
                UserId = userId
            };
            
            db.SearchHistories.Add(searchHistory);
            
            db.SaveChanges();

            return db.SearchHistories.Find(nextId);
        }
        
        public Boolean DeleteSearchHistory(int shId)
        {
            using var db = new StackoverflowContext();

            var sh = db.SearchHistories.Find(shId);

            if (sh != null)
            {
                db.SearchHistories.Remove(sh);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<MarkingPost> GetMarkedPosts(int userId)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from m in db.MarkingsPost
                where m.UserId.Equals(userId)
                where m.PostId > 0
                select new MarkingPost()
                {
                    UserId = m.UserId, MarkingDate = m.MarkingDate, MarkingId = m.MarkingId, PostId = m.PostId, Annotation = m.Annotation
                };
            return query.ToList();
        }
        
        public List<MarkingComment> GetMarkedComments(int userId)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from m in db.MarkingsComment
                where m.UserId.Equals(userId)
                where m.CommentId > 0
                select new MarkingComment()
                {
                    UserId = m.UserId, CommentId = m.CommentId, MarkingDate = m.MarkingDate, MarkingId = m.MarkingId, Annotation = m.Annotation
                };
            return query.ToList();
        }

        public MarkingPost SetMarkingPost(int userId, int postId, string annotation) //we have decided that the hm_annotation table is superfluous and unnecessary. The AnnotationText should be added to the marking table.
        {
            using var db = new StackoverflowContext();

            var query =
                from m in db.MarkingsPost
                where m.UserId.Equals(userId)
                where m.PostId.Equals(postId)
                select new MarkingPost()
                {
                    MarkingId = m.MarkingId, MarkingDate = m.MarkingDate, UserId = m.UserId, PostId = m.PostId, Annotation = m.Annotation
                };

            var qResult = query.ToList();
            try
            {
                if (qResult.Count == 1)
                {
                    MarkingPost pMarking = db.MarkingsPost.Find(qResult[0].MarkingId);
                    if (pMarking.Annotation == null)
                    {
                        pMarking.Annotation = annotation;
                        pMarking.MarkingDate = DateTime.Now;
                        db.SaveChanges();
                        return db.MarkingsPost.Find(pMarking.MarkingId);
                    }
                    if(!(pMarking.Annotation.Equals(annotation)))
                    {
                        pMarking.Annotation = annotation;
                        pMarking.MarkingDate = DateTime.Now;
                        db.SaveChanges();
                        return db.MarkingsPost.Find(pMarking.MarkingId);
                    }
                    return pMarking;
                }
            }
            catch (Exception e)
            {
                MarkingPost pMarking = db.MarkingsPost.Find(qResult[0].MarkingId);
                return pMarking;
                
            }
            
            int nextId;
            try { nextId = db.MarkingsPost.Max(x => x.MarkingId) + 1; }
            catch (Exception e){ nextId = 1; }
            
            var marking = new MarkingPost()
                {
                    MarkingId = nextId,
                    UserId = userId,
                    MarkingDate = DateTime.Now,
                    PostId = postId,
                    Annotation = annotation
                };
                db.MarkingsPost.Add(marking);
            
                db.SaveChanges();

                return marking;
        }
        
        public MarkingComment SetMarkingComment(int userId, int commentId, string annotation) //we have decided that the hm_annotation table is superfluous and unnecessary. The AnnotationText should be added to the marking table.
        {
            using var db = new StackoverflowContext();

            int nextId;
            try
            {
                nextId = db.MarkingsComment.Max(x => x.MarkingId) + 1;
            }
            catch (Exception e)
            {
                nextId = 1;
            }
            
            var marking = new MarkingComment()
            {
                MarkingId = nextId,
                UserId = userId,
                MarkingDate = DateTime.Now,
                CommentId = commentId,
                Annotation = annotation
            };
            db.MarkingsComment.Add(marking);
            
            db.SaveChanges();

            return marking;
        }

        public Boolean RemoveMarking(int markingId)
        {
            using var db = new StackoverflowContext();

            var marking = db.MarkingsPost.Find(markingId);
            db.MarkingsPost.Remove(marking);
            db.SaveChanges();
            return true;
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

        public List<Comment> GetComments(int postId)
        {
            using var db = new StackoverflowContext();
            
            var query =
                from c in db.Comments
                where c.PostId.Equals(postId)
                select new Comment()
                {
                    CommentId = c.CommentId, TextContain = c.TextContain, Score = c.Score, CreationDate = c.CreationDate, PostId = c.PostId, UserId = c.UserId
                };
            return query.ToList();
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
    }
}