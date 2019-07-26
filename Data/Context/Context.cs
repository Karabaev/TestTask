namespace Data.Context
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    internal class Context : DbContext
    {
        internal DbSet<ContactInfo> ContactInfos { get; set; }
        internal DbSet<Organization> Organizations { get; set; }
        internal DbSet<Person> People { get; set; }
        internal DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasOne(p => p.Organization).WithMany(o => o.People).HasForeignKey(p => p.OrganizationId);
            modelBuilder.Entity<Person>().HasOne(p => p.Position).WithMany(p => p.People).HasForeignKey(p => p.PositionId);
            modelBuilder.Entity<Person>().HasMany(p => p.Contacts).WithOne(c => c.Person).HasForeignKey(c => c.PersonId);
        }
    }
}
