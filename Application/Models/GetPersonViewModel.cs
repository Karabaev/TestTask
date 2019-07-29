namespace Application.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GetPersonViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
