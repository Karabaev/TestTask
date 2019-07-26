namespace Application.Models
{
    using Data.Entities;

    public class IndexContactInfoViewModel
    {
        public IndexContactInfoViewModel(ContactInfo info)
        {
            this.Type = info.Type.ToString();
            this.Value = info.Value;
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}
