namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Entities;
    using Context;

    public class ContactInfoRepository : IContactInfoRepository
    {
        public async Task<bool> CreateAsync(ContactInfo entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                await db.ContactInfos.AddAsync(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public ContactInfo Get(Guid id)
        {
            using (Context db = new Context())
            {
                return db.ContactInfos.FirstOrDefault(ci => ci.Id == id);
            }
        }

        public IEnumerable<ContactInfo> GetAll()
        {
            using (Context db = new Context())
            {
                return db.ContactInfos.ToList();
            }
        }

        public async Task<bool> RemoveAsync(ContactInfo entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                db.ContactInfos.Remove(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            using (Context db = new Context())
            {
                db.ContactInfos.Remove(this.Get(id));
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
