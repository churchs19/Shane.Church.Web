using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
	public class JournalTagsMap : EntityTypeConfiguration<JournalTags>
	{
		public JournalTagsMap()
		{
			// Primary Key
			this.HasKey(t => t.JournalTagId);

			// Properties
			this.Property(t => t.Tag)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("journal_tags");
			this.Property(t => t.JournalTagId).HasColumnName("journal_tag_id");
			this.Property(t => t.JournalId).HasColumnName("journal_id");
			this.Property(t => t.Tag).HasColumnName("tag");

			// Relationships
			this.HasRequired(t => t.Journal)
				.WithMany(t => t.JournalTags)
				.HasForeignKey(t => t.JournalId);
		}
	}
}
