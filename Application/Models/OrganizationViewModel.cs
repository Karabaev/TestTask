namespace Application.Models
{
    using Data.Entities;

    public class OrganizationViewModel
    {
        public OrganizationViewModel(Organization organization)
        {
            if (organization == null)
                throw new System.ArgumentNullException(nameof(organization));

            this.Name = organization.Name;
        }

        public string Name { get; set; }
    }
}
