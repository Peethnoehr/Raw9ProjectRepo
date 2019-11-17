using System.Security.Cryptography;
using DatabaseService;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    public class StackoverflowContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Describes> Descriptions { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<MarkingPost> MarkingsPost { get; set; }
        public DbSet<MarkingComment> MarkingsComment { get; set; }
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
            modelBuilder.Entity<MarkingPost>().ToTable("hm_marking_post");
            modelBuilder.Entity<MarkingPost>().HasKey(o => new { o.MarkingId });
            modelBuilder.Entity<MarkingPost>().Property(m => m.MarkingId).HasColumnName("markingid");
            modelBuilder.Entity<MarkingPost>().Property(m => m.MarkingDate).HasColumnName("markingdate");
            modelBuilder.Entity<MarkingPost>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<MarkingPost>().Property(m => m.PostId).HasColumnName("postid");
            modelBuilder.Entity<MarkingPost>().Property(m => m.Annotation).HasColumnName("annotation");

            modelBuilder.Entity<MarkingComment>().ToTable("hm_marking_comment");
            modelBuilder.Entity<MarkingComment>().HasKey(o => new { o.MarkingId });
            modelBuilder.Entity<MarkingComment>().Property(m => m.MarkingId).HasColumnName("markingid");
            modelBuilder.Entity<MarkingComment>().Property(m => m.MarkingDate).HasColumnName("markingdate");
            modelBuilder.Entity<MarkingComment>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<MarkingComment>().Property(m => m.CommentId).HasColumnName("commentid");
            modelBuilder.Entity<MarkingComment>().Property(m => m.Annotation).HasColumnName("annotation");
            
            modelBuilder.Entity<SearchHistory>().ToTable("hm_searchhistory");
            modelBuilder.Entity<SearchHistory>().HasKey(o => new { o.SearchHistoryId });
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchHistoryId).HasColumnName("searchhistoryid");
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchDate).HasColumnName("searchdate");
            modelBuilder.Entity<SearchHistory>().Property(m => m.SearchText).HasColumnName("searchtext");
            modelBuilder.Entity<SearchHistory>().Property(m => m.UserId).HasColumnName("userid");
            
            modelBuilder.Entity<AppUser>().ToTable("hm_user");
            modelBuilder.Entity<AppUser>().HasKey(o => new {o.UserId});
            modelBuilder.Entity<AppUser>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<AppUser>().Property(m => m.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<AppUser>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<AppUser>().Property(m => m.Password).HasColumnName("userpassword");
            modelBuilder.Entity<AppUser>().Property(m => m.Email).HasColumnName("email");
            
            modelBuilder.Entity<Answer>().ToTable("qa_answer");
            modelBuilder.Entity<Answer>().HasKey(o => new { o.AnswerId});
            modelBuilder.Entity<Answer>().Property(m => m.AnswerId).HasColumnName("answerid");
            modelBuilder.Entity<Answer>().Property(m => m.QuestionId).HasColumnName("questionid");

            modelBuilder.Entity<Comment>().ToTable("qa_comment");
            modelBuilder.Entity<Comment>().HasKey(o => new { o.CommentId});
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
            modelBuilder.Entity<Post>().HasKey(o => new { o.PostId });
            modelBuilder.Entity<Post>().Property(m => m.PostId).HasColumnName("postid");
            modelBuilder.Entity<Post>().Property(m => m.Body).HasColumnName("body");
            modelBuilder.Entity<Post>().Property(m => m.Score).HasColumnName("score");
            modelBuilder.Entity<Post>().Property(m => m.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Post>().Property(m => m.UserId).HasColumnName("userid");
            
            modelBuilder.Entity<Question>().ToTable("qa_question");
            modelBuilder.Entity<Question>().HasKey(o => new { o.QuestionId });
            modelBuilder.Entity<Question>().Property(m => m.QuestionId).HasColumnName("questionid");
            modelBuilder.Entity<Question>().Property(m => m.Title).HasColumnName("title");
            modelBuilder.Entity<Question>().Property(m => m.ClosedDate).HasColumnName("closeddate");
            modelBuilder.Entity<Question>().Property(m => m.AcceptAnswer).HasColumnName("acceptanswer");
            
            modelBuilder.Entity<Tag>().ToTable("qa_tag");
            modelBuilder.Entity<Tag>().HasKey(o => new { o.TagId});
            modelBuilder.Entity<Tag>().Property(m => m.TagId).HasColumnName("tagid");
            modelBuilder.Entity<Tag>().Property(m => m.TagName).HasColumnName("tagname");
            
            modelBuilder.Entity<QAUser>().ToTable("qa_user");
            modelBuilder.Entity<QAUser>().HasKey(o => new { o.UserId });
            modelBuilder.Entity<QAUser>().Property(m => m.UserId).HasColumnName("userid");
            modelBuilder.Entity<QAUser>().Property(m => m.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<QAUser>().Property(m => m.Age).HasColumnName("age");
            modelBuilder.Entity<QAUser>().Property(m => m.UserLocation).HasColumnName("userlocation");
            modelBuilder.Entity<QAUser>().Property(m => m.CreationDate).HasColumnName("creationdate");
        }
    }
}