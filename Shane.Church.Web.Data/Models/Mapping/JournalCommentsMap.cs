using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class JournalCommentsMap : EntityTypeConfiguration<JournalComments>
    {
        public JournalCommentsMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            // Properties
            this.Property(t => t.Username)
                .HasMaxLength(15);

            this.Property(t => t.Email)
                .HasMaxLength(255);

            this.Property(t => t.Link)
                .HasMaxLength(255);

            this.Property(t => t.IPAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("journal_comments");
            this.Property(t => t.CommentId).HasColumnName("COMMENT_ID");
            this.Property(t => t.JournalId).HasColumnName("JOURNAL_ID");
            this.Property(t => t.EntryDate).HasColumnName("ENTRY_DATE");
            this.Property(t => t.Username).HasColumnName("USERNAME");
            this.Property(t => t.Email).HasColumnName("EMAIL");
            this.Property(t => t.Link).HasColumnName("LINK");
            this.Property(t => t.Comments).HasColumnName("COMMENTS");
            this.Property(t => t.IPAddress).HasColumnName("IP");

            // Relationships
            this.HasOptional(t => t.Journal)
                .WithMany(t => t.JournalComments)
                .HasForeignKey(d => d.JournalId);

        }
    }
}
