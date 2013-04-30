using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class PageMap : EntityTypeConfiguration<Page>
    {
        public PageMap()
        {
            // Primary Key
            this.HasKey(t => t.PageId);

            // Properties
            this.Property(t => t.PageId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DisplayName)
                .HasMaxLength(50);

            this.Property(t => t.PageTitle)
                .HasMaxLength(100);

            this.Property(t => t.Latitude)
                .HasMaxLength(50);

            this.Property(t => t.Longitude)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("page");
            this.Property(t => t.PageId).HasColumnName("ID");
            this.Property(t => t.DisplayName).HasColumnName("DISPLAY_NAME");
            this.Property(t => t.PageTitle).HasColumnName("PAGE_TITLE");
            this.Property(t => t.SortOrder).HasColumnName("SORT_ORDER");
            this.Property(t => t.Active).HasColumnName("ACTIVE");
            this.Property(t => t.Latitude).HasColumnName("LATITUDE");
            this.Property(t => t.Longitude).HasColumnName("LONGITUDE");
        }
    }
}
