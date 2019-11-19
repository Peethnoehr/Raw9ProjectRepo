using System;
using System.Linq;
using DataAccessLayer;
using Xunit;

namespace StackOverFlow.Tests
{
    public class DataServiceTests
    {
        // User
        [Fact]
        public void GetAllUsers_NoArgument_ReturnsAllUsers()
        {
            var service = new DataService();
            var users = service.GetUsers();
            Assert.Equal(2, users.Count);
            Assert.Equal("Baptiste", users.First().UserName);
        }

        [Fact]
        public void GetUser_ValidUsername_ReturnsUserObject()
        {
            var service = new DataService();
            var user = service.GetUser("Baptiste");
            Assert.Equal("Baptiste", user.UserName);
            Assert.Equal("auba@ruc.dk",user.Email);
        }

        [Fact]
        public void CreateUser_ValidData_CreateUserAndRetunsNewObject()
        {
            var service = new DataService();
            var user = service.CreateUser("Test", "testpw", "test@test.test","test",DateTime.Now);
            Assert.Equal("Test", user.UserName);
            Assert.Equal("testpw", user.Password);
            Assert.Equal("test@test.test", user.Email);

            // cleanup
            service.DeleteUser(user.UserName);
        }

        [Fact]
        public void DeleteCategory_ValidId_RemoveTheCategory()
        {
            var service = new DataService();
            var user = service.CreateUser("Test", "testpw", "test@test.test","test",DateTime.Now);
            var result = service.DeleteUser(user.UserName);
            Assert.True(result);
            user = service.GetUser(user.UserName);
            Assert.Null(user);
        }
        
        [Fact]
        public void UpdateUser_DisplayName_Password_Email()
        {
            var service = new DataService();
            var user = service.CreateUser("Test", "testpw", "test@test.test","test",DateTime.Now);

            var result1 = service.UpdateUser(user.UserName, "UpdatedPassword1","UpdatedEmail1", "UpdateSalt");
            Assert.True(result1);

            user = service.GetUser(user.UserName);
            Assert.Equal("UpdatedPassword1", user.Password);
            Assert.Equal("UpdatedEmail1", user.Email);

            var result2 = service.UpdateUser(user.UserName, null,null,"Test");
            Assert.True(result2);

            user = service.GetUser(user.UserName);
            Assert.Equal("UpdatedPassword1",user.Password);
            Assert.Equal("UpdatedEmail1",user.Email);

            var result3 = service.UpdateUser(user.UserName, "UpdatedPassword2",null, null);
            Assert.True(result3);

            user = service.GetUser(user.UserName);
            Assert.Equal("UpdatedPassword2",user.Password);
            Assert.Equal("UpdatedEmail1",user.Email);
            
            var result4 = service.UpdateUser(user.UserName, null,"UpdatedEmail2", null);
            Assert.True(result4);

            user = service.GetUser(user.UserName);
            Assert.Equal("UpdatedPassword2",user.Password);
            Assert.Equal("UpdatedEmail2",user.Email);
            // cleanup
            service.DeleteUser(user.UserName);
        }
        
        // Marking
        [Fact]
        public void CreateMarking_ValidData_CreateMarkingrWithPostAndRetunsNewObject()
        {
            var service = new DataService();
            var marking = service.CreateMarking("testpw", "Baptiste",19,null,DateTime.Now);
            Assert.Equal(1, marking.Id);
            Assert.Equal("testpw", marking.Annotation);

            // cleanup
            service.DeleteMarking(marking.Id);
        }

        [Fact]
        public void UpdateMarking_Annotation()
        {
            var service = new DataService();
            var marking = service.CreateMarking("testpw", "Baptiste",19,null,DateTime.Now);

            var result1 = service.UpdateMarking(1,"UpdateAnnotation");
            Assert.True(result1);

            marking = service.GetMarking(marking.Id);
            Assert.Equal("UpdateAnnotation", marking.Annotation);
        }
        
        [Fact]
        public void GetAllMarking_Username_ReturnsAllMarkings()
        {
            var service = new DataService();
            var markings = service.GetMarkings("Baptiste");
            Assert.Equal(19, markings.First().PostId);
            Assert.Equal(19, markings.First().Post.Id);
            Assert.Equal("algorithm", markings.First().Post.Tags.First().Name);
            Assert.Null(markings.First().CommentId);
        }
        
        // SearchHistory
        [Fact]
        public void CreateSearchHistory_ValidData_CreateSearchHistoryWithPostAndRetunsNewObject()
        {
            var service = new DataService();
            var searchHistory = service.CreateSearchHistory("testsearch", "Baptiste",DateTime.Now);
            Assert.Equal(1, searchHistory.Id);
            Assert.Equal("testsearch", searchHistory.Text);

            // cleanup
            service.DeleteMarking(searchHistory.Id);
        }

        [Fact]
        public void GetAllSearchHistories_Username_ReturnsAllSearchHistories()
        {
            var service = new DataService();
            var searchHistory = service.CreateSearchHistory("testsearch", "Baptiste",DateTime.Now);
            var searchHistories = service.GetSearchHistories("Baptiste");
            Assert.Equal(1, searchHistories.First().Id);
            Assert.Equal("testsearch", searchHistories.First().Text);
        }
        
        // Comment
        [Fact]
        public void GetAllComments_postId_ReturnsAllComments()
        {
            var service = new DataService();
            var comments = service.GetComments(19);
            Assert.Equal(16, comments.Count);
            Assert.Equal("*boggle* If it's a personal challenge, why are you asking us for the solution? =)", comments.First().TextContain);
            Assert.Equal("Erik Forbes", comments.First().QAUser.DisplayName);
        }
        
        // Tag
        [Fact]
        public void GetAllTags_Postid_ReturnsAllTags()
        {
            var service = new DataService();
            var tags = service.GetTags(19);
            Assert.Equal(35, tags.First().Id);
            Assert.Equal("algorithm", tags.First().Name);
        }
        
        // Post
        [Fact]
        public void GetPost_Postid_ReturnPost()
        {
            var service = new DataService();
            var tags = service.GetDetailQuestion(19);
        }

        // -----------------
        /*[Fact]
        public void SearchTest()
        {
            var service = new DataService();
            var result = service.SearchPosts(1,"abc");
            Assert.True(result.Count > 0 && result.Count < service.GetAllPosts().Count);
        }
        
        [Fact]
        public void TestAddToSearchHistory()
        {
            var service = new DataService();
            var sh = service.AddToSearchHistory(1, "test");
            Assert.Equal(1, sh.UserId);
            Assert.Equal("test",sh.SearchText);

            // cleanup
            service.DeleteSearchHistory(sh.SearchHistoryId);
        }

        [Fact]
        public void TestSetMarkingWithAnnotation()
        {
            var service = new DataService();
            var marking = service.SetMarkingPost(1, 19,"testAnnotation2");
            Assert.Equal("testAnnotation2",marking.Annotation);
            var markings = service.GetMarkedPosts(1);
            Assert.True(1 == markings.Count);
            Assert.Equal("testAnnotation2",markings.First().Annotation);
            
            //cleanup
//            service.RemoveMarking(markings.First().MarkingId);
        }
        
        [Fact]
        public void TestSetMarkingNoAnnotation()
        {
            var service = new DataService();
            var marking = service.SetMarkingPost(1, 19,null);
            var markings = service.GetMarkedPosts(1);
            Assert.Equal(19,markings.First().PostId);
            Assert.Equal(1,markings.First().UserId);
            Assert.Null(markings.First().Annotation);
        }

        [Fact]
        public void TestGetQuestion()
        {
            var service = new DataService();
            Question q = service.getQuestion(19);
            Assert.Equal(531, q.AcceptAnswer);
            Assert.Equal("What is the fastest way to get the value of π?", q.Title);
            Assert.Equal(71,q.Answers[0].AnswerPost.PostId);
        }

        [Fact]
        public void TestGetAnswers()
        {
            var service = new DataService();
            List<Answer> answers = service.GetAnswers(19);
        }

        [Fact]
        public void TestGetComment()
        {
            var service = new DataService();
            List<Comment> comments = service.GetComments(71);
        }*/
    }
}
