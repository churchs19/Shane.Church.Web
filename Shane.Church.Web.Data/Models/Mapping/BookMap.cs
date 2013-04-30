using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            // Primary Key
            this.HasKey(t => t.BookId);

            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(255);

            this.Property(t => t.Author)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("books");
            this.Property(t => t.BookId).HasColumnName("BOOK_ID");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Author).HasColumnName("AUTHOR");
            this.Property(t => t.PurchaseHtml).HasColumnName("PURCHASE_HTML");
            this.Property(t => t.ImageHtml).HasColumnName("IMAGE_HTML");
            this.Property(t => t.Active).HasColumnName("ACTIVE");
            this.Property(t => t.Comments).HasColumnName("COMMENTS");
            this.Property(t => t.UpdatedDate).HasColumnName("UPDATED_DATE");
        }
    }
}
