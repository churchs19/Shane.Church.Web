using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Shane.Church.Web.Data.Models.Mapping;

namespace Shane.Church.Web.Data.Models
{
	public class DataContext : DbContext
	{
		static DataContext()
		{
			Database.SetInitializer<DataContext>(null);
		}

		public DataContext()
			: base("Name=DataContext")
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<Journal> Journals { get; set; }
		public DbSet<JournalComments> JournalComments { get; set; }
		public DbSet<JournalTags> JournalTags { get; set; }
		public DbSet<Page> Pages { get; set; }
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UsersInRole> UsersInRoles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new BookMap());
			modelBuilder.Configurations.Add(new JournalMap());
			modelBuilder.Configurations.Add(new JournalCommentsMap());
			modelBuilder.Configurations.Add(new JournalTagsMap());
			modelBuilder.Configurations.Add(new PageMap());
			modelBuilder.Configurations.Add(new PhotoMap());
			modelBuilder.Configurations.Add(new RoleMap());
			modelBuilder.Configurations.Add(new UserMap());
			modelBuilder.Configurations.Add(new UsersInRoleMap());
		}
	}
}
