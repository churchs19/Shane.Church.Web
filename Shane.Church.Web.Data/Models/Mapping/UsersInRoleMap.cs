using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Shane.Church.Web.Data.Models.Mapping
{
    public class UsersInRoleMap : EntityTypeConfiguration<UsersInRole>
    {
        public UsersInRoleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Username, t.Rolename, t.ApplicationName });

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Rolename)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.ApplicationName)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("usersinroles");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Rolename).HasColumnName("Rolename");
            this.Property(t => t.ApplicationName).HasColumnName("ApplicationName");
        }
    }
}
