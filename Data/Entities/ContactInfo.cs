namespace Data.Entities
{
    using System;

    public enum ContactInfoTypes
    {
        Telephone,
        Skype,
        Email,
        Other,
        Undefined
    }

    public class ContactInfo : IEntity
    {
        public Guid Id { get; set; }
        public ContactInfoTypes Type { get; set; }
        public string Value { get; set; }
        public virtual Guid PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
