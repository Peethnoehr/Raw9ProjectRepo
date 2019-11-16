using DatabaseService;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    public class StackoverflowContext : DbContext
    { 
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Describes> Descriptions { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Marking> Markings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<QAUser> QaUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "host=localhost;db=stackoverflow;uid=postgres;pwd=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Annotation>().ToTable("hm_annotation");
            modelBuilder.Entity<Annotation>().Property(m => m.AnnotationId).HasColumnName("annotationid");
            modelBuilder.Entity<Annotation>().Property(m => m.AnnotationDate).HasColumnName("annotationdate");
            modelBuilder.Entity<Annotation>().Property(m => m.AnnotationText).HasColumnName("annotationtext");
            modelBuilder.Entity<Annotation>().Property(m => m.MarkingId).HasColumnName("markingid");

            modelBuilder.Entity<Marking>().ToTable("hm_marking");
            modelBuilder.Entity<Marking>().Property(m => m.MarkingId).HasColumnName("markingid");
            modelBuilder.Entity<Marking>().Property(m => m.MarkingDate).HasColumnName("markingdate");
            modelBuilder.Entity<Marking>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<Marking>().Property(m => m.PostId).HasColumnName("postid");
            modelBuilder.Entity<Marking>().Property(m => m.CommentId).HasColumnName("commentid");
            
            modelBuilder.Entity<SearchHistory>().ToTable("hm_searchhistory");
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchHistoryId).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchDate).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchText).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.UserId).HasColumnName("userid");
            
            modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
            modelBuilder.Entity<OrderDetails>().HasKey(o => new { o.OrderId, o.ProductId });
            modelBuilder.Entity<OrderDetails>().Property(m => m.OrderId).HasColumnName("orderid");
            modelBuilder.Entity<OrderDetails>().Property(m => m.ProductId).HasColumnName("productid");
            modelBuilder.Entity<OrderDetails>().Property(m => m.UnitPrice).HasColumnName("unitprice");
            modelBuilder.Entity<OrderDetails>().Property(m => m.Quantity).HasColumnName("quantity");
            modelBuilder.Entity<OrderDetails>().Property(m => m.Discount).HasColumnName("discount");
        }
    }
}