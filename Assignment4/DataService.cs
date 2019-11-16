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
        
        // CATEGORY
        //9
        public Category GetCategory(int idquery)
        {
            using var db = new NorthwindContex();
            return db.Categories.Find(idquery);
        }
        
        //10
        public List<Category> GetCategories()
        {
            using var db = new NorthwindContex();
            return db.Categories.ToList();
        }
        
        //11
        public Category CreateCategory(string namequery, string descriptionquery)
        {
            using var db = new NorthwindContex();

            var nextId = db.Categories.Max(x => x.Id) + 1;

            var cat = new Category
            {
                Id = nextId,
                Name = namequery,
                Description = descriptionquery
            };
            
            db.Categories.Add(cat);
            
            db.SaveChanges();

            return db.Categories.Find(nextId);
        }
        
        //12
        public Boolean UpdateCategory(int idquery,string namequery, string descriptionquery)
        {
            using var db = new NorthwindContex();

            var category = db.Categories.Find(idquery);

            if (category != null)
            {
                category.Name = namequery;
                category.Description = descriptionquery;
                
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        //13
        public Boolean DeleteCategory(int idquery)
        {
            using var db = new NorthwindContex();

            var category = db.Categories.Find(idquery);

            if (category != null)
            {
                db.Categories.Remove(category);

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
