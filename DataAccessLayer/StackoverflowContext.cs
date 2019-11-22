using System.Security.Cryptography;
using StackOverFlow;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using StackOverFlow;

namespace DataAccessLayer
{
    public class StackoverflowContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Describes> Describes { get; set; }
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
                "host=localhost;db=stackoverflow;uid=postgres;pwd=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Marking>().ToTable("hm_marking");
            modelBuilder.Entity<Marking>().Property(m => m.Id).HasColumnName("markingid");
            modelBuilder.Entity<Marking>().Property(m => m.Date).HasColumnName("markingdate");
            modelBuilder.Entity<Marking>().Property(m => m.Username).HasColumnName("username");
            modelBuilder.Entity<Marking>().Property(m => m.CommentId).HasColumnName("commentid");
            modelBuilder.Entity<Marking>().Property(m => m.Annotation).HasColumnName("annotation");
            modelBuilder.Entity<Marking>().Property(m => m.PostId).HasColumnName("postid");
            
            modelBuilder.Entity<SearchHistory>().ToTable("hm_searchhistory");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Id).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Date).HasColumnName("searchdate");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Text).HasColumnName("searchtext");
            modelBuilder.Entity<SearchHistory>().Property(m => m.UserName).HasColumnName("username");
            
            modelBuilder.Entity<User>().ToTable("hm_user");
            modelBuilder.Entity<User>().HasKey(o => new {o.UserName});
            modelBuilder.Entity<User>().Property(m => m.UserName).HasColumnName("username");
            modelBuilder.Entity<User>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<User>().Property(m => m.Password).HasColumnName("userpassword");
            modelBuilder.Entity<User>().Property(m => m.Email).HasColumnName("email");
            modelBuilder.Entity<User>().Property(m => m.Salt).HasColumnName("salt");
            
            modelBuilder.Entity<Answer>().ToTable("qa_answer");
            modelBuilder.Entity<Answer>().Property(m => m.Id).HasColumnName("answerid");
            modelBuilder.Entity<Answer>().Property(m => m.QuestionId).HasColumnName("questionid");

            modelBuilder.Entity<Comment>().ToTable("qa_comment");
            modelBuilder.Entity<Comment>().Property(m => m.Id).HasColumnName("commentid");
            modelBuilder.Entity<Comment>().Property(m => m.TextContain).HasColumnName("textcontain");
            modelBuilder.Entity<Comment>().Property(m => m.Score).HasColumnName("score");
            modelBuilder.Entity<Comment>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Comment>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<Comment>().Property(m => m.PostId).HasColumnName("postid");
            
            modelBuilder.Entity<Describes>().ToTable("qa_describes");
            modelBuilder.Entity<Describes>().HasKey(o => new { o.TagId, o.PostId });
            modelBuilder.Entity<Describes>().Property(m => m.TagId).HasColumnName("tagid");
            modelBuilder.Entity<Describes>().Property(m => m.PostId).HasColumnName("postid");            
            
            modelBuilder.Entity<Links>().ToTable("qa_links");
            modelBuilder.Entity<Links>().HasKey(o => new { o.QuestionId, o.PostId });
            modelBuilder.Entity<Links>().Property(m => m.QuestionId).HasColumnName("questionid");
            modelBuilder.Entity<Links>().Property(m => m.PostId).HasColumnName("postid");

            modelBuilder.Entity<Post>().ToTable("qa_post");
            modelBuilder.Entity<Post>().Property(m => m.Id).HasColumnName("postid");
            modelBuilder.Entity<Post>().Property(m => m.Body).HasColumnName("body");
            modelBuilder.Entity<Post>().Property(m => m.Score).HasColumnName("score");
            modelBuilder.Entity<Post>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Post>().Property(m => m.UserId).HasColumnName("userid");
            
            modelBuilder.Entity<Question>().ToTable("qa_question");
            modelBuilder.Entity<Question>().Property(m => m.Id).HasColumnName("questionid");
            modelBuilder.Entity<Question>().Property(m => m.Title).HasColumnName("title");
            modelBuilder.Entity<Question>().Property(m => m.ClosedDate).HasColumnName("closeddate");
            modelBuilder.Entity<Question>().Property(m => m.AcceptAnswer).HasColumnName("acceptanswer");
            
            modelBuilder.Entity<Tag>().ToTable("qa_tag");
            modelBuilder.Entity<Tag>().Property(m => m.Id).HasColumnName("tagid");
            modelBuilder.Entity<Tag>().Property(m => m.Name).HasColumnName("tagname");
            
            modelBuilder.Entity<QAUser>().ToTable("qa_user");
            modelBuilder.Entity<QAUser>().Property(m => m.Id).HasColumnName("userid");
            modelBuilder.Entity<QAUser>().Property(m => m.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<QAUser>().Property(m => m.Age).HasColumnName("age");
            modelBuilder.Entity<QAUser>().Property(m => m.UserLocation).HasColumnName("userlocation");
            modelBuilder.Entity<QAUser>().Property(m => m.CreationDate).HasColumnName("creationdate");
        }
    }
}