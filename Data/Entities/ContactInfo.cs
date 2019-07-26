namespace Data.Entities
{
    using System;

    public enum ContactInfoTypes
    {
        Telephone,
        Skype,
        Email,
        Other
    }

    public class ContactInfo : IEntity
    {
        public Guid Id { get; set; }
        public ContactInfoTypes types { get; set; }
        public string Value { get; set; }
        public virtual Guid PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
