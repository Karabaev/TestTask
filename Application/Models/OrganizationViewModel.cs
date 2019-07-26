namespace Application.Models
{
    using Data.Entities;

    public class OrganizationViewModel
    {
        public OrganizationViewModel(Organization organization)
        {
            this.Name = organization.Name;
        }

        public string Name { get; set; }
    }
}
