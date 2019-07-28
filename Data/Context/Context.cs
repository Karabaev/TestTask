namespace Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();

        }

        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasOne(p => p.Organization).WithMany(o => o.People).HasForeignKey(p => p.OrganizationId);
            modelBuilder.Entity<Person>().HasOne(p => p.Position).WithMany(p => p.People).HasForeignKey(p => p.PositionId);
            modelBuilder.Entity<Person>().HasMany(p => p.Contacts).WithOne(c => c.Person).HasForeignKey(c => c.PersonId);
        }
    }
}
