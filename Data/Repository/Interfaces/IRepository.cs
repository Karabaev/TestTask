namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    public interface IRepository<T> where T: IEntity
    {
        Task<bool> CreateAsync(T entity);
        T Get(Guid id);
        IEnumerable<T> GetAll();
        Task<bool> RemoveAsync(T entity);
        Task<bool> RemoveAsync(Guid id);
    }
}
