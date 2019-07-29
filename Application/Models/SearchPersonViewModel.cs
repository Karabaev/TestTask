namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SearchPersonViewModel
    {
        [Required]
        public string SearchString { get; set; }
        [Required]
        public SearchTypes SearchType { get; set; }
    }
}
