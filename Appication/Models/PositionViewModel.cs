namespace Appication.Models
{
    using Data.Entities;

    public class PositionViewModel
    {
        public PositionViewModel(Position position)
        {
            this.Name = position.Name;
        }

        public string Name { get; set; }
    }
}
