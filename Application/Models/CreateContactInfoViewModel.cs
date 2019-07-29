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

        public CreateContactInfoViewModel() { }

        public CreateContactInfoViewModel(ContactInfo info)
        {
            this.Value = info.Value;
            this.Type = info.Type.ToString();
        }

        public ContactInfo GetDomain()
        {
            ContactInfo result = new ContactInfo();
            result.Value = this.Value;

            try
            {
                result.Type = (ContactInfoTypes)Enum.Parse(typeof(ContactInfoTypes), this.Type);
            }
            catch(FormatException)
            {
                result.Type = ContactInfoTypes.Undefined;
            }
           
            return result;
        }
    }
}
