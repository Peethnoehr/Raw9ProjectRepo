using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using DataBaseService;
using DataAccess;

namespace Assignment4.Tests
{
    public class DataServiceTests
    {
        /* Categories */

        [Fact]
        public void GetAllUsers_NoArgument_ReturnsAllUsers()
        {
            var service = new DataService();
            var users = service.GetUsers();
            Assert.Equal(2, users.Count);
            Assert.Equal("test1", users.First().DisplayName);
        }

        [Fact]
        public void GetUser_ValidId_ReturnsUserObject()
        {
            var service = new DataService();
            var user = service.GetUser(2);
            Assert.Equal("test2", user.DisplayName);
            Assert.Equal(2, user.UserId);
            Assert.Equal("test2pw",user.Password);
            Assert.Equal("test2@test.test",user.Email);
        }

        [Fact]
        public void CreateAppUser_ValidData_CreateUserAndRetunsNewObject()
        {
            var service = new DataService();
            var user = service.CreateAppUser("Test10", "test10pw", "test10@test.test");
            Assert.True(user.UserId > 0);
            Assert.Equal("Test10", user.DisplayName);
            Assert.Equal("test10pw", user.Password);
            Assert.Equal("test10@test.test", user.Email);

            // cleanup
            service.DeleteAppUser(user.UserId);
        }

        [Fact]
        public void DeleteCategory_ValidId_RemoveTheCategory()
        {
            var service = new DataService();
            var user = service.CreateAppUser("Test10", "test10pw", "test10@test.test");
            var result = service.DeleteAppUser(user.UserId);
            Assert.True(result);
            user = service.GetUser(user.UserId);
            Assert.Null(user);
        }
        
        [Fact]
        public void UpdateAppUser_DisplayName_Password_Email()
        {
            var service = new DataService();
            var user = service.CreateAppUser("TestUpdate","testUpdatePW","testUpdateEmail");

            var result1 = service.UpdateAppUser(user.UserId, "UpdatedName1", "UpdatedPassword1","UpdatedEmail1");
            Assert.True(result1);

            user = service.GetUser(user.UserId);
            Assert.Equal("UpdatedName1", user.DisplayName);
            Assert.Equal("UpdatedPassword1", user.Password);
            Assert.Equal("UpdatedEmail1", user.Email);

            var result2 = service.UpdateAppUser(user.UserId, "UpdatedName2",null,null);
            Assert.True(result2);

            user = service.GetUser(user.UserId);
            Assert.Equal("UpdatedName2",user.DisplayName);
            Assert.Equal("UpdatedPassword1",user.Password);
            Assert.Equal("UpdatedEmail1",user.Email);

            var result3 = service.UpdateAppUser(user.UserId, null,"UpdatedPassword2",null);
            Assert.True(result3);

            user = service.GetUser(user.UserId);
            Assert.Equal("UpdatedName2",user.DisplayName);
            Assert.Equal("UpdatedPassword2",user.Password);
            Assert.Equal("UpdatedEmail1",user.Email);
            
            var result4 = service.UpdateAppUser(user.UserId, null,null,"UpdatedEmail2");
            Assert.True(result4);

            user = service.GetUser(user.UserId);
            Assert.Equal("UpdatedName2",user.DisplayName);
            Assert.Equal("UpdatedPassword2",user.Password);
            Assert.Equal("UpdatedEmail2",user.Email);
            // cleanup
            service.DeleteAppUser(user.UserId);
        }
        
        [Fact]
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
        }
    }
}
