using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using StackOverFlow;

namespace DatabaseService
{
    public class StackoverflowContex : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<Marking> Marking { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "host=localhost;db=stackoverflow;uid=postgres;pwd=atdeti6!");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchHistory>().ToTable("hm_searchhistory");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Id).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Date).HasColumnName("date");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Text).HasColumnName("description");
            modelBuilder.Entity<SearchHistory>().Property(m => m.Username).HasColumnName("username");
            
            modelBuilder.Entity<User>().ToTable("hm_user");
            modelBuilder.Entity<User>().HasKey(o => new { o.Username });
            modelBuilder.Entity<User>().Property(m => m.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(m => m.Salt).HasColumnName("salt");
            modelBuilder.Entity<User>().Property(m => m.Password).HasColumnName("userpassword");
            modelBuilder.Entity<User>().Property(m => m.Email).HasColumnName("email");
            
            modelBuilder.Entity<Marking>().ToTable("hm_marking");
            modelBuilder.Entity<Marking>().Property(m => m.Id).HasColumnName("markingid");
            modelBuilder.Entity<Marking>().Property(m => m.Annotation).HasColumnName("markingdate");
            modelBuilder.Entity<Marking>().Property(m => m.Username).HasColumnName("username");
            modelBuilder.Entity<Marking>().Property(m => m.PostId).HasColumnName("postid");
            modelBuilder.Entity<Marking>().Property(m => m.CommentId).HasColumnName("commentid");
            
            modelBuilder.Entity<Comment>().ToTable("qa_comment");
            modelBuilder.Entity<Comment>().Property(m => m.Id).HasColumnName("commentid");
            modelBuilder.Entity<Comment>().Property(m => m.Text).HasColumnName("textcontain");
            modelBuilder.Entity<Comment>().Property(m => m.Score).HasColumnName("score");
            
            modelBuilder.Entity<Tag>().ToTable("qa_tag");
            modelBuilder.Entity<Tag>().Property(m => m.Id).HasColumnName("tagid");
            modelBuilder.Entity<Tag>().Property(m => m.Name).HasColumnName("tagname");
            
            modelBuilder.Entity<Post>().ToTable("qa_post");
            modelBuilder.Entity<Post>().Property(m => m.Id).HasColumnName("postid");
            modelBuilder.Entity<Post>().Property(m => m.Body).HasColumnName("body");
            modelBuilder.Entity<Post>().Property(m => m.Score).HasColumnName("score");
            
            modelBuilder.Entity<Question>().ToTable("qa_question");
            modelBuilder.Entity<Question>().Property(m => m.Id).HasColumnName("questionid");
            modelBuilder.Entity<Question>().Property(m => m.ClosedDate).HasColumnName("closeddate");
            modelBuilder.Entity<Question>().Property(m => m.Title).HasColumnName("title");
            modelBuilder.Entity<Question>().Property(m => m.AcceptAnswer).HasColumnName("acceptanswer");
            
            modelBuilder.Entity<Answer>().ToTable("qa_answer");
            modelBuilder.Entity<Answer>().Property(m => m.Id).HasColumnName("questionid");
        }
    }
}
