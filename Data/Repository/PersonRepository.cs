namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Entities;
    using Context;

    public class PersonRepository : IPersonRepository
    {
        public async Task<bool> CreateAsync(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                await db.People.AddAsync(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public Person Get(Guid id)
        {
            using (Context db = new Context())
            {
                return db.People.FirstOrDefault(o => o.Id == id);
            }
        }

        public IEnumerable<Person> GetAll()
        {
            using (Context db = new Context())
            {
                return db.People.ToList();
            }
        }

        public async Task<bool> RemoveAsync(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                db.People.Remove(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            using (Context db = new Context())
            {
                db.People.Remove(this.Get(id));
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
