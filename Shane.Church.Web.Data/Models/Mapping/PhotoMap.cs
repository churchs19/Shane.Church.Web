using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class PhotoMap : EntityTypeConfiguration<Photo>
    {
        public PhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.PhotoId);

            // Properties
            this.Property(t => t.PageId)
                .HasMaxLength(50);

            this.Property(t => t.Caption)
                .HasMaxLength(255);

            this.Property(t => t.File)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("photo");
            this.Property(t => t.PhotoId).HasColumnName("ID");
            this.Property(t => t.PageId).HasColumnName("LINK_ID");
            this.Property(t => t.Caption).HasColumnName("CAPTION");
            this.Property(t => t.File).HasColumnName("FILE");
            this.Property(t => t.DisplayWidth).HasColumnName("DISPLAY_WIDTH");
            this.Property(t => t.DisplayHeight).HasColumnName("DISPLAY_HEIGHT");
            this.Property(t => t.SortOrder).HasColumnName("SORT_ORDER");
            this.Property(t => t.UpdatedDate).HasColumnName("UPDATED_DATE");
            this.Property(t => t.Active).HasColumnName("ACTIVE");
            this.Property(t => t.PageDefault).HasColumnName("PAGE_DEFAULT");

            // Relationships
            this.HasOptional(t => t.Page)
                .WithMany(t => t.Photos)
                .HasForeignKey(d => d.PageId);

        }
    }
}
