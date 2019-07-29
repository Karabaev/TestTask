namespace Application.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RemoveContactInfoViewModel
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
