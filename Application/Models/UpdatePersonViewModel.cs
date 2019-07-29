namespace Application.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Entities;

    public class UpdatePersonViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string PositionName { get; set; }
        [Required]
        public virtual ICollection<CreateContactInfoViewModel> Contacts { get; set; }

        public UpdatePersonViewModel() { }

        public UpdatePersonViewModel(Person person)
        {
            this.Id = person.Id;
            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.MiddleName = person.MiddleName;
            this.DateOfBirth = person.DateOfBirth;
            this.OrganizationName = person.Organization.Name;
            this.PositionName = person.Position.Name;
            this.Contacts = new List<CreateContactInfoViewModel>();

            foreach (var item in person.Contacts)
            {
                this.Contacts.Add(new CreateContactInfoViewModel(item));
            }
        }

        public Person GetDomain(Guid orgId, Guid posId)
        {
            Person result = new Person
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                MiddleName = this.MiddleName,
                DateOfBirth = this.DateOfBirth,
                OrganizationId = orgId,
                PositionId = posId,
                Contacts = new List<ContactInfo>()
            };

            foreach (var item in this.Contacts)
            {
                result.Contacts.Add(item.GetDomain());
            }

            return result;
        }
    }
}
