namespace Application.Models
{
    using System;
    using Data.Entities;
    using System.ComponentModel.DataAnnotations;

    public class CreateContactInfoViewModel
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }

        public ContactInfo GetDomain(/*Guid personId*/)
        {
            ContactInfo result = new ContactInfo();
            result.Value = this.Value;
            //result.PersonId = personId;

            if (!Enum.TryParse<ContactInfoTypes>(this.Type, out ContactInfoTypes type))
            {
                result.Type = type;
            }
            else
            {
                result.Type = ContactInfoTypes.Other;
            }

            return result;
        }
    }
}
