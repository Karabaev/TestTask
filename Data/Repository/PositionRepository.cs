namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Entities;
    using Context;

    public class PositionRepository : IPositionRepository
    {
        public async Task<bool> CreateAsync(Position entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                await db.Positions.AddAsync(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public Position Get(Guid id)
        {
            using (Context db = new Context())
            {
                return db.Positions.FirstOrDefault(o => o.Id == id);
            }
        }

        public IEnumerable<Position> GetAll()
        {
            using (Context db = new Context())
            {
                return db.Positions.ToList();
            }
        }

        public async Task<bool> RemoveAsync(Position entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (Context db = new Context())
            {
                db.Positions.Remove(entity);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            using (Context db = new Context())
            {
                db.Positions.Remove(this.Get(id));
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
