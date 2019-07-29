namespace Application.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemovePersonViewModel
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
