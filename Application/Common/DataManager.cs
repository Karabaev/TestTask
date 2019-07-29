namespace Application.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;

    public class DataManager
    {
        public DataManager(Context context)
        {
            this.context = context;
        }

        public Person GetPerson(Guid id)
        {
            return this.context.People.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Person> GetAllPeople()
        {
            return this.context.People.ToList();
        }

        public async Task<bool> AddPersonAsync(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await this.context.People.AddAsync(entity);
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePersonAsync(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(entity.Id));

            Person person = this.GetPerson(entity.Id);

            if (person == null)
                return false;

            foreach (var item in person.GetType().GetProperties())
            {
                item.SetValue(person, item.GetValue(entity));
            }

            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemovePersonAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(id));

            Person person = this.GetPerson(id);

            if (person == null)
                return false;

            this.context.People.Remove(person);
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddOrganizationAsync(Organization entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await this.context.Organizations.AddAsync(entity);
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPositionAsync(Position entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await this.context.Positions.AddAsync(entity);
            return await this.context.SaveChangesAsync() > 0;
        }

        public Organization FindOrganizationByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return this.context.Organizations.FirstOrDefault(o => o.Name.ToLower() == name.ToLower());
        }

        public Position FindPositionByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return this.context.Positions.FirstOrDefault(o => o.Name.ToLower() == name.ToLower());
        }

        private readonly Context context;
    }
}
