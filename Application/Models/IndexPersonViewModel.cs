namespace Application.Models
{
    using System;
    using System.Collections.Generic;
    using Data.Entities;

    public class IndexPersonViewModel
    {
        public IndexPersonViewModel(Person person)
        {
            this.Id = person.Id;
            this.FullName = string.Join(' ', person.LastName, person.FirstName, person.MiddleName);
            this.DateOfBirth = person.DateOfBirth;
            this.Organization = new OrganizationViewModel(person.Organization);
            this.Position = new PositionViewModel(person.Position);
            this.Contacts = new List<IndexContactInfoViewModel>();

            foreach (var item in person.Contacts)
            {
                this.Contacts.Add(new IndexContactInfoViewModel(item));
            }

        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<IndexContactInfoViewModel> Contacts { get; set; }
        public OrganizationViewModel Organization { get; set; }
        public PositionViewModel Position { get; set; }
    }
}
