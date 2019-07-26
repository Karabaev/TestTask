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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid PositionId { get; set; }
        public virtual ICollection<CreateContactInfoViewModel> Contacts { get; set; }

        public Person GetDomain()
        {
            Person result = new Person
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                MiddleName = this.MiddleName,
                DateOfBirth = this.DateOfBirth,
                OrganizationId = OrganizationId,
                PositionId = PositionId,
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
