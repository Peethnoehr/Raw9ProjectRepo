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
            var answer = service.GetDetailQuestion(19);
            Assert.Equal(19, answer.Id);    
            Assert.Equal("What is the fastest way to get the value of π?", answer.Title);
            Assert.Equal(531, answer.AcceptAnswerPost.Id);
            Assert.Equal(19, answer.Post.Id);
            Assert.Equal("algorithm", answer.Post.Tags.First().Name);
            Assert.Equal("*boggle* If it's a personal challenge, why are you asking us for the solution? =)", answer.Post.Comments.First().TextContain);
            Assert.Equal(71, answer.Answers.First().Id);
        }
        
        [Fact]
        public void GetPost_Postid_ReturnPostTest()
        {
            var service = new DataService();
            var posts = service.searchPosts("java help me");
            Assert.Equal(406790, posts.First().Id);
            Assert.Equal("<p>Okay, I said I'd give a bit more detail on my \"sealed classes\" opinion. I guess one way to show the kind of answer I'm interested in is to give one myself :)</p>&#xA;&#xA;<p><strong>Opinion: Classes should be sealed by default in C#</strong></p>&#xA;&#xA;<p><strong>Reasoning:</strong></p>&#xA;&#xA;<p>There's no doubt that inheritance is powerful. However, it has to be somewhat guided. If someone derives from a base class in a way which is completely unexpected, this can break the assumptions in the base implementation. Consider two methods in the base class, where one calls another - if these methods are both virtual, then that implementation detail has to be documented, otherwise someone could quite reasonably override the second method and expect a call to the first one to work. And of course, as soon as the implementation is documented, it can't be changed... so you lose flexibility.</p>&#xA;&#xA;<p>C# took a step in the right direction (relative to Java) by making methods sealed by default. However, I believe a further step - making <em>classes</em> sealed by default - would have been even better. In particular, it's easy to override methods (or not explicitly seal existing virtual methods which you don't override) so that you end up with unexpected behaviour. This wouldn't actually stop you from doing anything you can currently do - it's just changing a <em>default</em>, not changing the available options. It would be a \"safer\" default though, just like the default access in C# is always \"the most private visibility available at that point.\"</p>&#xA;&#xA;<p>By making people <em>explicitly</em> state that they wanted people to be able to derive from their classes, we'd be encouraging them to think about it a bit more. It would also help me with my laziness problem - while I know I <em>should</em> be sealing almost all of my classes, I rarely actually remember to do so :(</p>&#xA;&#xA;<p><strong>Counter-argument:</strong></p>&#xA;&#xA;<p>I can see an argument that says that a class which has no virtual methods can be derived from relatively safely without the extra inflexibility and documentation usually required.  I'm not sure how to counter this one at the moment, other than to say that I believe the harm of accidentally-unsealed classes is greater than that of accidentally-sealed ones.</p>&#xA;", posts.First().Body);
            Assert.Equal("Levenshtein to Damerau-Levenshtein", posts[1].Title);
        }
        
    }
}
