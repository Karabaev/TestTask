namespace Data.Entities
{
    using System;
    using System.Collections.Generic;

    public class Position : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
