namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Entities;
    using Context;

    public class OrganizationRepository : IOrganizationRepository
    {
        public async Task<bool> CreateAsync(Organization entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                await db.Organizations.AddAsync(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public Organization Get(Guid id)
        {
            using (Context db = new Context())
            {
                return db.Organizations.FirstOrDefault(o => o.Id == id);
            }
        }

        public IEnumerable<Organization> GetAll()
        {
            using (Context db = new Context())
            {
                return db.Organizations.ToList();
            }
        }

        public async Task<bool> RemoveAsync(Organization entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                db.Organizations.Remove(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            using (Context db = new Context())
            {
                db.Organizations.Remove(this.Get(id));
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
