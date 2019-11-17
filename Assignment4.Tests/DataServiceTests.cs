using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatabaseService;
using Xunit;

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
            Assert.Equal("test2", users.First().DisplayName);
        }

        [Fact]
        public void GetCategory_ValidId_ReturnsCategoryObject()
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

/*
        [Fact]
        public void DeleteCategory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteCategory(-1);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCategory_InvalidID_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.UpdateCategory(-1, "UpdatedName", "UpdatedDescription");
            Assert.False(result);
        }
*/

        /* products */
/*
        [Fact]
        public void Product_Object_HasIdNameUnitPriceQuantityPerUnitAndUnitsInStock()
        {
            var product = new Product();
            Assert.Equal(0, product.Id);
            Assert.Null(product.Name);
            Assert.Equal(0.0, product.UnitPrice);
            Assert.Null(product.QuantityPerUnit);
            Assert.Equal(0, product.UnitsInStock);
        }

        [Fact]
        public void GetProduct_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var product = service.GetProduct(1);
            Assert.Equal("Chai", product.Name);
            Assert.Equal("Beverages", product.Category.Name);
        }

        [Fact]
        public void GetProductsByCategory_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var products = service.GetProductByCategory(1);
            Assert.Equal(12, products.Count);
            Assert.Equal("Chai", products.First().Name);
            Assert.Equal("Beverages", products.First().Category.Name);
            Assert.Equal("Lakkalikööri", products.Last().Name);
        }

        [Fact]
        public void GetProduct_NameSubString_ReturnsProductsThatMachesTheSubString()
        {
            var service = new DataService();
            var products = service.GetProductByName("em");
            Assert.Equal(4, products.Count);
            Assert.Equal("NuNuCa Nuß-Nougat-Creme", products.First().Name);
            Assert.Equal("Flotemysost", products.Last().Name);
        }
*/
        /* orders */
        /*
         
        [Fact]
        public void Order_Object_HasIdDatesAndOrderDetails()
        {
            var order = new Order();
            Assert.Equal(0, order.Id);
            Assert.Equal(new DateTime(), order.Date);
            Assert.Equal(new DateTime(), order.Required);
            Assert.Null(order.OrderDetails);
            Assert.Null(order.ShipName);
            Assert.Null(order.ShipCity);
        }

        [Fact]
        public void GetOrder_ValidId_ReturnsCompleteOrder()
        {
            var service = new DataService();
            var order = service.GetOrder(10248);
            Assert.Equal(3, order.OrderDetails.Count);
            Assert.Equal("Queso Cabrales", order.OrderDetails.First().Product.Name);
            Assert.Equal("Dairy Products", order.OrderDetails.First().Product.Category.Name);
        }

        [Fact]
        public void GetOrders()
        {
            var service = new DataService();
            var orders = service.GetOrders();
            Assert.Equal(830, orders.Count);
        }
*/

        /* orderdetails */
        /*
        [Fact]
        public void OrderDetails_Object_HasOrderProductUnitPriceQuantityAndDiscount()
        {
            var orderDetails = new OrderDetails();
            Assert.Equal(0, orderDetails.OrderId);
            Assert.Null(orderDetails.Order);
            Assert.Equal(0, orderDetails.ProductId);
            Assert.Null(orderDetails.Product);
            Assert.Equal(0.0, orderDetails.UnitPrice);
            Assert.Equal(0.0, orderDetails.Quantity);
            Assert.Equal(0.0, orderDetails.Discount);
        }

        [Fact]
        public void GetOrderDetailByOrderId_ValidId_ReturnsProductNameUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByOrderId(10248);
            Assert.Equal(3, orderDetails.Count);
            Assert.Equal("Queso Cabrales", orderDetails.First().Product.Name);
            Assert.Equal(14, orderDetails.First().UnitPrice);
            Assert.Equal(12, orderDetails.First().Quantity);
        }

        [Fact]
        public void GetOrderDetailByProductId_ValidId_ReturnsOrderDateUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByProductId(11);
            Assert.Equal(38, orderDetails.Count);
            Assert.Equal("1997-05-06", orderDetails.First().Order.Date.ToString("yyyy-MM-dd"));
            Assert.Equal(21, orderDetails.First().UnitPrice);
            Assert.Equal(3, orderDetails.First().Quantity);
        }
        */
    }
}
