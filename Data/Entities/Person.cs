namespace Data.Entities
{
    using System;
    using System.Collections.Generic;

    public class Person : IEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public Guid PositionId { get; set; }
        public virtual Organization Position { get; set; }
        public virtual ICollection<ContactInfo> Contacts { get; set; }
    }
}
