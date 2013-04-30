using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class JournalMap : EntityTypeConfiguration<Journal>
    {
        public JournalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Image)
                .HasMaxLength(255);

            this.Property(t => t.ImageText)
                .HasMaxLength(255);

            this.Property(t => t.User)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("journal");
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.EntryDate).HasColumnName("ENTRY_DATE");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Image).HasColumnName("IMAGE");
            this.Property(t => t.ImageText).HasColumnName("IMAGE_TEXT");
            this.Property(t => t.Entry).HasColumnName("ENTRY");
            this.Property(t => t.User).HasColumnName("USER");
        }
    }
}
