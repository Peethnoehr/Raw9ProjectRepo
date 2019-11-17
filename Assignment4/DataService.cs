using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment4;

namespace DatabaseService
{
    public class DataService
    {
        /*
        // ORDER
        public List<Order> GetOrders()
        {
            var listorderdetail = GetOrderDetails();
                
            using var db = new NorthwindContex();
            var query =
                from o in db.Orders
                select new Order(){Id = o.Id, Date = o.Date, Freight = o.Freight, Required = o.Required, Shipped = o.Shipped == null ? DateTime.Now : o.Shipped, ShipCity = o.ShipCity, ShipName = o.ShipName, OrderDetails = listorderdetail};
            return query.ToList();
        }
        
        //2
        public List<Order> GetOrdersByShippingName(string shipnamequery)
        {
            using var db = new NorthwindContex();
            return new List<Order>
            {db.Orders
                .Where(x => x.ShipName == shipnamequery)
                .Select(y => new Order(){Id = y.Id, Date = y.Date, ShipName = y.ShipName, ShipCity = y.ShipCity})
                .FirstOrDefault()};
        }
        
        //3
        public Order GetOrder(int idquery)
        {
            var orderdetails = GetOrderDetailsByOrderId(idquery);
            
            using var db = new NorthwindContex();
            var query =
                from o in db.Orders
                join od in db.OrderDetails on o.Id equals od.OrderId
                where o.Id == idquery
                select new Order(){Id = o.Id, Date = o.Date, Required = o.Required, Shipped = o.Shipped == null ? DateTime.Now : o.Shipped, Freight = o.Freight, ShipName = o.ShipName, ShipCity = o.ShipCity, OrderDetails = orderdetails};
            return query.FirstOrDefault();
        }
        
        // ORDERDETAIL
        public List<OrderDetails> GetOrderDetails()
        {
            using var db = new NorthwindContex();
            var query =
                from od in db.OrderDetails
                join p in db.Products on od.ProductId equals p.Id
                join c in db.Categories on p.CategoryId equals c.Id
                select new OrderDetails(){OrderId = od.OrderId, Quantity = od.Quantity, UnitPrice = od.UnitPrice, Product = new Product(){Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice, QuantityPerUnit = p.QuantityPerUnit, UnitsInStock = p.UnitsInStock, CategoryId = p.CategoryId, Category = new Category(){Id = c.Id, Name = c.Name, Description = c.Description}}};
            return query.ToList();
        }
        
        //4
        public List<OrderDetails> GetOrderDetailsByOrderId(int idquery)
        {
            using var db = new NorthwindContex();
            var query =
                from od in db.OrderDetails
                join p in db.Products on od.ProductId equals p.Id
                join c in db.Categories on p.CategoryId equals c.Id
                where od.OrderId == idquery
                select new OrderDetails(){OrderId = od.OrderId, Quantity = od.Quantity, UnitPrice = od.UnitPrice, Product = new Product(){Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice, QuantityPerUnit = p.QuantityPerUnit, UnitsInStock = p.UnitsInStock, CategoryId = p.CategoryId, Category = new Category(){Id = c.Id, Name = c.Name, Description = c.Description}}};
            return query.ToList();
        }
        
        //5
        public List<OrderDetails> GetOrderDetailsByProductId(int idproductquery)
        {
            using var db = new NorthwindContex();
            var query =
                from od in db.OrderDetails
                join o in db.Orders on od.OrderId equals o.Id
                where od.ProductId == idproductquery
                select new OrderDetails() {OrderId = od.OrderId, Quantity = od.Quantity, UnitPrice = od.UnitPrice, Order = new Order(){Id = o.Id, Date = o.Date, ShipName = o.ShipName, ShipCity = o.ShipCity}};
            return query.ToList();
        } 
        
        // PRODUCT
        public List<Product> GetProducts()
        {
            using var db = new NorthwindContex();
            return db.Products.ToList();
        }
        
        //6
        public Product GetProduct(int idproductquery)
        {
            using var db = new NorthwindContex();
            return db.Products
                .Join(db.Categories,
                    product => product.CategoryId,
                    category => category.Id,
                    (product,category)=> new Product(){Id = product.Id, Name = product.Name, QuantityPerUnit = product.QuantityPerUnit, UnitPrice = product.UnitPrice, UnitsInStock = product.UnitsInStock, Category = new Category(){Id = category.Id, Name = category.Name, Description = category.Description}})
                .FirstOrDefault(x => x.Id == idproductquery);
        }
        
        //7
        public List<Product> GetProductByName(string partstringquery)
        {
            using var db = new NorthwindContex();
            var query =
                from p in db.Products
                join c in db.Categories on p.CategoryId equals c.Id
                where p.Name.Contains(partstringquery)
                select new Product(){Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice, Category = new Category(){Id = c.Id, Name = c.Name, Description = c.Description}};
            return query.ToList();
        }
        
        //8
        public List<Product> GetProductByCategory(int idcategoryquery)
        {
            using var db = new NorthwindContex();
            var query =
                from p in db.Products
                join c in db.Categories on p.CategoryId equals c.Id
                where p.CategoryId == idcategoryquery
                select new Product(){Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice, Category = new Category(){Id = c.Id, Name = c.Name, Description = c.Description}};
            return query.ToList();
        }
        */        
        // Users
        //9
        public AppUser GetUser(int id)
        {
            using var db = new StackoverflowContext();
            return db.AppUsers.Find(id); //find uses primary key to find entity
        }

        //10
        public List<AppUser> GetUsers()
        {
            using var db = new StackoverflowContext();
            return db.AppUsers.ToList();
        }
        
        //11
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
        //12
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
        //13
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
        
        public List<Post> SearchPosts(int userId, string searchString) 
        {
            using var db = new StackoverflowContext();
            var query =
                from p in db.Posts
                where p.Body.Contains(searchString)
                select new Post()
                {
                    PostId = p.PostId, Body = p.Body, Score = p.Score, CreationDate = p.CreationDate, UserId = p.UserId
                };

            var searchHistoryEntity = AddToSearchHistory(userId, searchString);
            
            return query.ToList();
        }
        
        /*public List<Question> SearchQuestions(int userId, string searchString) //currently set up to only search for question posts
        {
            using var db = new StackoverflowContext();
            var query =
                from p in db.Posts
                join u in db.Questions on p.PostId equals u.QuestionId //comment out this line to search for all posts
                where p.Body.Contains(searchString)
                select new Question()
                {
                    PostId = p.PostId, Title = u.Title, Score = p.Score, CreationDate = p.CreationDate, UserId = p.UserId
                };

            var searchHistoryEntity = AddToSearchHistory(userId, searchString);
            
            return query.ToList();
        }*/

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
    }
}