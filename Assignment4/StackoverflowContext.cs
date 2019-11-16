using System.Security.Cryptography;
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
            
            modelBuilder.Entity<AppUser>().ToTable("hm_user");
            modelBuilder.Entity<AppUser>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<AppUser>().Property(m => m.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<AppUser>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<AppUser>().Property(m => m.Password).HasColumnName("userpassword");
            modelBuilder.Entity<AppUser>().Property(m => m.Email).HasColumnName("email");
            
            modelBuilder.Entity<Answer>().ToTable("qa_answer");
            modelBuilder.Entity<Answer>().Property(m => m.AnswerId).HasColumnName("answerid");
            modelBuilder.Entity<Answer>().Property(m => m.QuestionId).HasColumnName("questionid");

            modelBuilder.Entity<Comment>().ToTable("qa_comment");
            modelBuilder.Entity<Comment>().Property(m => m.CommentId).HasColumnName("commentid");
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
            modelBuilder.Entity<Post>().Property(m => m.PostId).HasColumnName("postid");
            modelBuilder.Entity<Post>().Property(m => m.Body).HasColumnName("body");
            modelBuilder.Entity<Post>().Property(m => m.Score).HasColumnName("score");
            modelBuilder.Entity<Post>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Post>().Property(m => m.UserId).HasColumnName("userid");
            
            modelBuilder.Entity<Question>().ToTable("qa_question");
            modelBuilder.Entity<Question>().Property(m => m.QuestionId).HasColumnName("questionid");
            modelBuilder.Entity<Question>().Property(m => m.Title).HasColumnName("title");
            modelBuilder.Entity<Question>().Property(m => m.ClosedDate).HasColumnName("closeddate");
            modelBuilder.Entity<Question>().Property(m => m.AcceptAnswer).HasColumnName("acceptanswer");
            
            modelBuilder.Entity<Tag>().ToTable("qa_tag");
            modelBuilder.Entity<Tag>().Property(m => m.TagId).HasColumnName("tagid");
            modelBuilder.Entity<Tag>().Property(m => m.TagName).HasColumnName("tagname");
            
            modelBuilder.Entity<QAUser>().ToTable("qa_user");
            modelBuilder.Entity<QAUser>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<QAUser>().Property(m => m.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<QAUser>().Property(m => m.Age).HasColumnName("age");
            modelBuilder.Entity<QAUser>().Property(m => m.UserLocation).HasColumnName("userlocation");
            modelBuilder.Entity<QAUser>().Property(m => m.CreationDate).HasColumnName("creationdate");
        }
    }
}