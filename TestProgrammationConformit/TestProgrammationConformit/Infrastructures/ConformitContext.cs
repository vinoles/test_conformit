using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestProgrammationConformit.Infrastructures
{
    public class ConformitContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Event)
            .WithMany(p => p.Comments);
        }

        public ConformitContext(DbContextOptions options) : base(options)
        {

        }
    }

    [Table("events")]
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("event_id", Order = 0)]
        public int EventId { get; set; }

        [Column("title", TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Column("person", TypeName = "varchar(50)")]
        public string Person { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        public List<Comment> Comments { get; set; }
    }

    [Table("comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("comment_id", Order = 0)]
        public int CommentId { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("event_id")]
        public int? EventId { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }

}
