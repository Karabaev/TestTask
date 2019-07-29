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

        public async Task<bool> RemoveContactInfoAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(id));

            ContactInfo info = this.context.ContactInfos.FirstOrDefault(ci => ci.Id == id);

            if (info == null)
                return false;

            this.context.ContactInfos.Remove(info);
            return await this.context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Person> FindPersonByName(string searchStr)
        {
            if (string.IsNullOrWhiteSpace(searchStr))
                throw new ArgumentNullException(nameof(searchStr));

            string searchStrToLower = searchStr.ToLower();
            List<Person> result = this.context.People.Where(p => p.FirstName.ToLower().Contains(searchStrToLower)).ToList();
            result.AddRange(this.context.People.Where(p => p.LastName.ToLower().Contains(searchStrToLower)).ToList());
            result.AddRange(this.context.People.Where(p => p.MiddleName.ToLower().Contains(searchStrToLower)).ToList());

            return result;
        }

        public IEnumerable<Person> FindPersonByOrganization(string searchStr)
        {
            if (string.IsNullOrWhiteSpace(searchStr))
                throw new ArgumentNullException(nameof(searchStr));

            string searchStrToLower = searchStr.ToLower();

            Organization organization = this.context.Organizations.FirstOrDefault(o => o.Name.ToLower() == searchStrToLower);

            if (organization == null)
                return new List<Person>();

            List<Person> result = this.context.People.Where(p => p.OrganizationId == organization.Id).ToList();
            return result;
        }

        public IEnumerable<Person> FindPersonByPosition(string searchStr)
        {
            if (string.IsNullOrWhiteSpace(searchStr))
                throw new ArgumentNullException(nameof(searchStr));

            string searchStrToLower = searchStr.ToLower();

            Position position = this.context.Positions.FirstOrDefault(o => o.Name.ToLower() == searchStrToLower);

            if (position == null)
                return new List<Person>();

            List<Person> result = this.context.People.Where(p => p.PositionId == position.Id).ToList();
            return result;
        }

        public IEnumerable<Person> FindPersonByContactInfo(string searchStr)
        {
            if (string.IsNullOrWhiteSpace(searchStr))
                throw new ArgumentNullException(nameof(searchStr));

            string searchStrToLower = searchStr.ToLower();

            var infos = this.context.ContactInfos.Where(o => o.Value.ToLower() == searchStrToLower);

            if (infos == null || !infos.Any())
                return new List<Person>();

            List<Person> result = new List<Person>(infos.Select(i => i.Person));
            return result;
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
