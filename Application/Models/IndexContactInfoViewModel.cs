namespace Application.Models
{
    using System;
    using Data.Entities;

    public class IndexContactInfoViewModel
    {
        public IndexContactInfoViewModel(ContactInfo info)
        {
            this.Id = info.Id;
            this.Type = info.Type.ToString();
            this.Value = info.Value;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
