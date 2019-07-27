namespace Application.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Entities;

    public class CreatePersonViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OrganizationName { get; set; }
        public string PositionName { get; set; }
        public virtual ICollection<CreateContactInfoViewModel> Contacts { get; set; }

        public Person GetDomain(Guid orgId, Guid posId)
        {
            Person result = new Person
            {
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
